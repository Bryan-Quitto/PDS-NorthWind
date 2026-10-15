using NorthWind.Sales.Backend.Repositories.Interfaces;
using NorthWind.Sales.Entities.Enums;

namespace NorthWind.Sales.Backend.Repositories.Repositories;

public class QueriesRepository(INorthWindSalesQueriesDataContext context) : IQueriesRepository
{
    public Task<bool> CustomerExistsAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var exists = context.Customers.Any(c => c.Id == customerId);
        return Task.FromResult(exists);
    }

    public Task<decimal> GetCustomerCurrentBalanceAsync(string customerId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(0m);
    }

    public Task<IReadOnlyList<ProductUnitsInStock>> GetProductsUnitsInStockAsync(
        IEnumerable<int> productIds,
        CancellationToken cancellationToken = default)
    {
        var idList = productIds.ToList();
        var stock = context.Products
            .Where(p => idList.Contains(p.Id))
            .Select(p => new ProductUnitsInStock(p.Id, p.UnitsInStock))
            .ToList();

        return Task.FromResult<IReadOnlyList<ProductUnitsInStock>>(stock);
    }

    public Task<PaginatedResultDto<CustomerDto>> SearchCustomersAsync(
        string? searchTerm,
        CustomerSearchCriteria criteria,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = context.Customers;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var tokens = searchTerm.Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var token in tokens)
            {
                var currentToken = token;
                query = criteria switch
                {
                    CustomerSearchCriteria.Identification => query.Where(c => c.Identification.Contains(currentToken)),
                    CustomerSearchCriteria.Name => query.Where(c => c.Name.Contains(currentToken)),
                    CustomerSearchCriteria.Id => query.Where(c => c.Id.Contains(currentToken)),
                    _ => query.Where(c =>
                        c.Id.Contains(currentToken) ||
                        c.Name.Contains(currentToken) ||
                        c.Identification.Contains(currentToken))
                };
            }
        }

        var totalCount = query.Count();

        var normalizedPageNumber = pageNumber < 1 ? 1 : pageNumber;
        var normalizedPageSize = pageSize < 1 ? 10 : pageSize;

        var items = query
            .OrderBy(c => c.Name)
            .Skip((normalizedPageNumber - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .Select(c => new CustomerDto(c.Id, c.Name, c.Identification))
            .ToList();

        var result = new PaginatedResultDto<CustomerDto>(items, totalCount, normalizedPageNumber, normalizedPageSize);
        return Task.FromResult(result);
    }

    public Task<PaginatedResultDto<ProductDto>> SearchProductsAsync(
        string? searchTerm,
        ProductSearchCriteria criteria,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = context.Products.Where(p => p.UnitsInStock > 0);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var tokens = searchTerm.Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var token in tokens)
            {
                var currentToken = token;
                query = criteria switch
                {
                    ProductSearchCriteria.Id => int.TryParse(currentToken, out var pid)
                        ? query.Where(p => p.Id == pid)
                        : query.Where(p => false),
                    ProductSearchCriteria.Name => query.Where(p => p.Name.Contains(currentToken)),
                    _ => int.TryParse(currentToken, out var pidAll)
                        ? query.Where(p => p.Id == pidAll || p.Name.Contains(currentToken))
                        : query.Where(p => p.Name.Contains(currentToken))
                };
            }
        }

        var totalCount = query.Count();

        var normalizedPageNumber = pageNumber < 1 ? 1 : pageNumber;
        var normalizedPageSize = pageSize < 1 ? 10 : pageSize;

        var items = query
            .OrderBy(p => p.Name)
            .Skip((normalizedPageNumber - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .Select(p => new ProductDto(p.Id, p.Name, p.UnitPrice, p.UnitsInStock))
            .ToList();

        var result = new PaginatedResultDto<ProductDto>(items, totalCount, normalizedPageNumber, normalizedPageSize);
        return Task.FromResult(result);
    }

    public Task<OrderInvoiceDto?> GetOrderInvoiceByIdAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var order = context.Orders.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
        {
            return Task.FromResult<OrderInvoiceDto?>(null);
        }

        var customer = context.Customers.FirstOrDefault(c => c.Id == order.CustomerId);
        var customerName = customer?.Name ?? order.CustomerId;
        var customerIdentification = customer?.Identification ?? string.Empty;

        var rawDetails = context.OrderDetails.Where(od => od.OrderId == orderId).ToList();
        var productIds = rawDetails.Select(od => od.ProductId).Distinct().ToList();
        var productsDict = context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionary(p => p.Id, p => p.Name);

        var details = rawDetails.Select(d => new OrderInvoiceDetailDto(
            d.ProductId,
            productsDict.TryGetValue(d.ProductId, out var name) ? name : $"Producto #{d.ProductId}",
            d.UnitPrice,
            d.Quantity,
            d.UnitPrice * d.Quantity
        )).ToList();

        var invoiceDto = new OrderInvoiceDto(
            order.Id,
            order.OrderDate,
            order.CustomerId,
            customerName,
            customerIdentification,
            order.ShipAddress,
            order.ShipCity,
            order.ShipCountry,
            order.ShipPostalCode,
            order.Subtotal,
            order.Tax,
            order.Total,
            details
        );

        return Task.FromResult<OrderInvoiceDto?>(invoiceDto);
    }

    public Task<PaginatedResultDto<OrderSummaryDto>> SearchOrdersAsync(
        string? searchTerm,
        OrderSearchCriteria criteria,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var ordersQuery = context.Orders;
        var customers = context.Customers;

        var joinedQuery = ordersQuery.Join(
            customers,
            o => o.CustomerId,
            c => c.Id,
            (o, c) => new { Order = o, Customer = c }
        );

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var tokens = searchTerm.Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var token in tokens)
            {
                var currentToken = token;
                joinedQuery = criteria switch
                {
                    OrderSearchCriteria.OrderId => int.TryParse(currentToken, out var parsedOrderId)
                        ? joinedQuery.Where(x => x.Order.Id == parsedOrderId)
                        : joinedQuery.Where(x => false),
                    OrderSearchCriteria.CustomerName => joinedQuery.Where(x => x.Customer.Name.Contains(currentToken)),
                    OrderSearchCriteria.CustomerIdentification => joinedQuery.Where(x => x.Customer.Identification.Contains(currentToken)),
                    OrderSearchCriteria.CustomerId => joinedQuery.Where(x => x.Order.CustomerId.Contains(currentToken)),
                    _ => int.TryParse(currentToken, out var parsedIdAll)
                        ? joinedQuery.Where(x => x.Order.Id == parsedIdAll || x.Customer.Name.Contains(currentToken) || x.Customer.Identification.Contains(currentToken) || x.Order.CustomerId.Contains(currentToken))
                        : joinedQuery.Where(x => x.Customer.Name.Contains(currentToken) || x.Order.CustomerId.Contains(currentToken) || x.Customer.Identification.Contains(currentToken))
                };
            }
        }

        var totalCount = joinedQuery.Count();

        var normalizedPageNumber = pageNumber < 1 ? 1 : pageNumber;
        var normalizedPageSize = pageSize < 1 ? 10 : pageSize;

        var pagedList = joinedQuery
            .OrderByDescending(x => x.Order.OrderDate)
            .Skip((normalizedPageNumber - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .Select(x => new
            {
                x.Order.Id,
                x.Order.OrderDate,
                x.Order.CustomerId,
                CustomerName = x.Customer.Name,
                CustomerIdentification = x.Customer.Identification,
                x.Order.Total
            })
            .ToList();

        var orderIds = pagedList.Select(x => x.Id).ToList();
        var countsByOrderId = context.OrderDetails
            .Where(od => orderIds.Contains(od.OrderId))
            .GroupBy(od => od.OrderId)
            .Select(g => new { OrderId = g.Key, Count = g.Count() })
            .ToDictionary(g => g.OrderId, g => g.Count);

        var summaries = pagedList.Select(x => new OrderSummaryDto(
            x.Id,
            x.OrderDate,
            x.CustomerId,
            x.CustomerName,
            x.CustomerIdentification,
            countsByOrderId.TryGetValue(x.Id, out var count) ? count : 0,
            x.Total
        )).ToList();

        var result = new PaginatedResultDto<OrderSummaryDto>(summaries, totalCount, normalizedPageNumber, normalizedPageSize);
        return Task.FromResult(result);
    }
}


