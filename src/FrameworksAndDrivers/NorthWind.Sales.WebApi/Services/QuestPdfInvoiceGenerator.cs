using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Services;
using NorthWind.Sales.Entities.Dtos;

namespace NorthWind.Sales.WebApi.Services;

public class QuestPdfInvoiceGenerator : IInvoicePdfGenerator
{
    static QuestPdfInvoiceGenerator()
    {
        // QuestPDF Community License for open-source / education
        QuestPDF.Settings.License = LicenseType.Community;
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
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(header => ComposeHeader(header, invoice));
                page.Content().Element(content => ComposeContent(content, invoice));
                page.Footer().Element(footer => ComposeFooter(footer, invoice));
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, OrderInvoiceDto invoice)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text("NORTHWIND TRADERS").Bold().FontSize(22).FontColor(Colors.Blue.Darken2);
                column.Item().Text("Soluciones Comerciales & Distribución").FontSize(10).FontColor(Colors.Grey.Darken1);
                column.Item().Text("Av. Los Shyris N34-123 y Portugal").FontSize(9).FontColor(Colors.Grey.Darken2);
                column.Item().Text("Quito - Ecuador | Tel: (02) 299-9000").FontSize(9).FontColor(Colors.Grey.Darken2);
                column.Item().Text("RUC: 1790012345001").FontSize(9).FontColor(Colors.Grey.Darken2);
            });

            row.ConstantItem(200).Column(column =>
            {
                column.Item().Border(1).BorderColor(Colors.Blue.Darken2).Padding(8).Column(box =>
                {
                    box.Item().AlignCenter().Text("FACTURA DE VENTA").Bold().FontSize(14).FontColor(Colors.Blue.Darken2);
                    box.Item().AlignCenter().Text($"No. {invoice.OrderId:D8}").Bold().FontSize(12);
                    box.Item().PaddingTop(4).Text($"Fecha: {invoice.OrderDate:dd/MM/yyyy HH:mm}").FontSize(9);
                    box.Item().Text("Estado: AUTORIZADA").FontSize(9).FontColor(Colors.Green.Darken2).Bold();
                });
            });
        });
    }

    private static void ComposeContent(IContainer container, OrderInvoiceDto invoice)
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
                    columns.ConstantColumn(50);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
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
            column.Item().Row(row =>
            {
                row.RelativeItem(); // Espacio a la izquierda

                row.ConstantItem(220).Column(totalsCol =>
                {
                    totalsCol.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Subtotal:");
                        r.RelativeItem().AlignRight().Text(invoice.Subtotal.ToString("C2"));
                    });

                    totalsCol.Item().Row(r =>
                    {
                        r.RelativeItem().Text("IVA (15%):");
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
