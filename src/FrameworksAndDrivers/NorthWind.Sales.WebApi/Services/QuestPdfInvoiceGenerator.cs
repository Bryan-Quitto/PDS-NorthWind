using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Services;
using NorthWind.Sales.Entities.Dtos;
using NorthWind.Sales.Entities;
using Microsoft.Extensions.Options;
using NorthWind.Sales.WebApi.Options;

namespace NorthWind.Sales.WebApi.Services;

public class QuestPdfInvoiceGenerator : IInvoicePdfGenerator
{
    private readonly CompanyBillingOptions _billingOptions;

    static QuestPdfInvoiceGenerator()
    {
        // QuestPDF Community License for open-source / education
        QuestPDF.Settings.License = LicenseType.Community;
        // QuestPDF 2025.1.0 aplicaba dos veces el offset de AlignRight/AlignCenter
        // sobre Text(), desplazando columnas fuera de la tabla. Corregido en 2026.9.x.
    }

    public QuestPdfInvoiceGenerator(IOptions<CompanyBillingOptions>? options = null)
    {
        _billingOptions = options?.Value ?? new CompanyBillingOptions();
    }

    public byte[] GenerateInvoicePdf(OrderInvoiceDto invoice)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(35);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Lato"));

                page.Header().Element(header => ComposeHeader(header, invoice));
                page.Content().Element(content => ComposeContent(content, invoice));
                page.Footer().Element(footer => ComposeFooter(footer, invoice));
            });
        });

        return document.GeneratePdf();
    }

    private void ComposeHeader(IContainer container, OrderInvoiceDto invoice)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text(_billingOptions.CompanyName).Bold().FontSize(22).FontColor(Colors.Blue.Darken2);
                column.Item().Text(_billingOptions.Subtitle).FontSize(10).FontColor(Colors.Grey.Darken1);
                column.Item().Text(_billingOptions.Address).FontSize(9).FontColor(Colors.Grey.Darken2);
                column.Item().Text(_billingOptions.Phone).FontSize(9).FontColor(Colors.Grey.Darken2);
                column.Item().Text($"RUC: {_billingOptions.TaxId}").FontSize(9).FontColor(Colors.Grey.Darken2);
            });

            row.ConstantItem(200).Column(column =>
            {
                column.Item().Border(1).BorderColor(Colors.Blue.Darken2).Padding(8).Column(box =>
                {
                    box.Item().AlignCenter().Text("FACTURA DE VENTA").Bold().FontSize(14).FontColor(Colors.Blue.Darken2);
                    box.Item().AlignCenter().Text($"No. {invoice.OrderId:D8}").Bold().FontSize(12);
                    var localOrderDate = invoice.OrderDate.ToEcuadorTime();
                    box.Item().PaddingTop(4).Text($"Fecha: {localOrderDate:dd/MM/yyyy HH:mm}").FontSize(9);
                    box.Item().Text($"Estado: {_billingOptions.EnvironmentType}").FontSize(9).FontColor(Colors.Orange.Darken2).Bold();
                });
            });
        });
    }

    private void ComposeContent(IContainer container, OrderInvoiceDto invoice)
    {
        container.PaddingVertical(15).Column(column =>
        {
            // Bloque de información del cliente y envío
            column.Item().Background(Colors.Grey.Lighten4).Padding(10).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("DATOS DEL CLIENTE").Bold().FontSize(10).FontColor(Colors.Blue.Darken2);
                    col.Item().Text($"Razón Social: {invoice.CustomerName}");
                    col.Item().Text($"Identificación/RUC: {invoice.CustomerIdentification}");
                    col.Item().Text($"Código Cliente: {invoice.CustomerId}");
                });

                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("DATOS DE ENVÍO").Bold().FontSize(10).FontColor(Colors.Blue.Darken2);
                    col.Item().Text($"Dirección: {invoice.ShipAddress}");
                    col.Item().Text($"Ciudad / País: {invoice.ShipCity}, {invoice.ShipCountry}");
                    col.Item().Text($"Código Postal: {invoice.ShipPostalCode}");
                });
            });

            column.Item().PaddingTop(15);

            // Tabla de productos
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(40);
                    columns.RelativeColumn(4);
                    columns.RelativeColumn(1.5f);
                    columns.RelativeColumn(1.2f);
                    columns.RelativeColumn(1.5f);
                });

                table.Header(header =>
                {
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("ID").Bold().FontColor(Colors.White);
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Descripción").Bold().FontColor(Colors.White);
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5).AlignRight().Text("P. Unit").Bold().FontColor(Colors.White);
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5).AlignCenter().Text("Cant.").Bold().FontColor(Colors.White);
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5).AlignRight().Text("Total").Bold().FontColor(Colors.White);
                });

                foreach (var detail in invoice.Details)
                {
                    table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(detail.ProductId.ToString());
                    table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(detail.ProductName);
                    table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignRight().Text(detail.UnitPrice.ToString("C2"));
                    table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignCenter().Text(detail.Quantity.ToString());
                    table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignRight().Text(detail.LineTotal.ToString("C2"));
                }
            });

            column.Item().PaddingTop(15);

            // Resumen de Totales
            column.Item().AlignRight().Width(240).Column(totalsCol =>
            {
                totalsCol.Item().Row(r =>
                {
                    r.RelativeItem().Text("Subtotal:");
                    r.RelativeItem().AlignRight().Text(invoice.Subtotal.ToString("C2"));
                });

                totalsCol.Item().Row(r =>
                {
                    r.RelativeItem().Text($"IVA ({_billingOptions.VatRate * 100:0.#}%):");
                    r.RelativeItem().AlignRight().Text(invoice.Tax.ToString("C2"));
                });

                totalsCol.Item().PaddingTop(4).LineHorizontal(1);

                totalsCol.Item().PaddingTop(4).Row(r =>
                {
                    r.RelativeItem().Text("TOTAL:").Bold().FontSize(12).FontColor(Colors.Blue.Darken2);
                    r.RelativeItem().AlignRight().Text(invoice.Total.ToString("C2")).Bold().FontSize(12).FontColor(Colors.Blue.Darken2);
                });
            });
        });
    }

    private static void ComposeFooter(IContainer container, OrderInvoiceDto invoice)
    {
        container.BorderTop(1).BorderColor(Colors.Grey.Lighten2).PaddingTop(8).Row(row =>
        {
            row.RelativeItem().Text("Gracias por su compra. Documento generado por NorthWind POS.").FontSize(8).FontColor(Colors.Grey.Darken1);
            row.RelativeItem().AlignRight().Text(text =>
            {
                text.Span("Página ").FontSize(8);
                text.CurrentPageNumber().FontSize(8);
                text.Span(" de ").FontSize(8);
                text.TotalPages().FontSize(8);
            });
        });
    }
}
