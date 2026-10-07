using NorthWind.Sales.Entities.Dtos;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.Services;

public interface IInvoicePdfGenerator
{
    byte[] GenerateInvoicePdf(OrderInvoiceDto invoice);
}
