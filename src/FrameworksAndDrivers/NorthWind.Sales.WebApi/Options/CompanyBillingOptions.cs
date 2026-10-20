namespace NorthWind.Sales.WebApi.Options;

public class CompanyBillingOptions
{
    public const string SectionKey = "CompanyBilling";

    public string CompanyName { get; set; } = "NORTHWIND TRADERS";
    public string Subtitle { get; set; } = "Soluciones Comerciales & Distribución";
    public string Address { get; set; } = "Av. Los Shyris N34-123 y Portugal";
    public string Phone { get; set; } = "Quito - Ecuador | Tel: (02) 299-9000";
    public string TaxId { get; set; } = "1790012345001";
    public decimal VatRate { get; set; } = 0.15m;
    public string EnvironmentType { get; set; } = "EN PRUEBAS";
}
