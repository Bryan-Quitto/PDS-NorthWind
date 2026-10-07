using NorthWind.Sales.Backend.Repositories.Interfaces;

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
        var balance = context.Customers
            .Where(c => c.Id == customerId)
            .Select(c => c.CurrentBalance)
            .FirstOrDefault();

        return Task.FromResult(balance);
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
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = context.Customers;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var trimmedTerm = searchTerm.Trim();
            query = query.Where(c => c.Name.Contains(trimmedTerm) || c.Identification.Contains(trimmedTerm));
        }

        var totalCount = query.Count();

        var normalizedPageNumber = pageNumber < 1 ? 1 : pageNumber;
        var normalizedPageSize = pageSize < 1 ? 10 : pageSize;

        var items = query
            .OrderBy(c => c.Name)
            .Skip((normalizedPageNumber - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .Select(c => new CustomerDto(c.Id, c.Name, c.Identification, c.CurrentBalance))
            .ToList();

        var result = new PaginatedResultDto<CustomerDto>(items, totalCount, normalizedPageNumber, normalizedPageSize);
        return Task.FromResult(result);
    }

    public Task<PaginatedResultDto<ProductDto>> SearchProductsAsync(
        string? searchTerm,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = context.Products.Where(p => p.UnitsInStock > 0);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var trimmedTerm = searchTerm.Trim();
            if (int.TryParse(trimmedTerm, out var id))
            {
                query = query.Where(p => p.Id == id || p.Name.Contains(trimmedTerm));
            }
            else
            {
                query = query.Where(p => p.Name.Contains(trimmedTerm));
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
}

