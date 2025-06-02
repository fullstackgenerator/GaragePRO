using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using GaragePRO.Models;
using System.Globalization;

namespace GaragePRO.PdfDocuments
{
    public class InvoiceDocument : IDocument
    {
        public Invoice Model { get; }
        private readonly CultureInfo _slCulture;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InvoiceDocument(Invoice invoice, IWebHostEnvironment webHostEnvironment)
        {
            Model = invoice;
            _slCulture = new CultureInfo("sl-SI");
            _webHostEnvironment = webHostEnvironment;
        }
        
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50); //margins

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
                    page.Footer().Element(ComposeFooter);
                });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                //path to logo
                var logoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "4800.jpg");

                if (System.IO.File.Exists(logoPath))
                {
                    row.ConstantColumn(100).Image(logoPath);
                }
                else
                {
                    row.ConstantColumn(150).Text("Company Logo").FontSize(10).AlignLeft().AlignCenter();
                }
  
                row.RelativeColumn().Column(column =>
                {
                    column.Item().AlignRight().Text($"INVOICE").FontSize(24).Bold().FontColor(Colors.Blue.Medium);
                    column.Item().AlignRight().Text($"# {Model.Id}").FontSize(16).SemiBold();
                    column.Item().AlignRight().Text($"Date Issued: {Model.DateIssued.ToString("dd. MM.yyyy")}").FontSize(10);
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(20);

                //company details
                column.Item().Text(text =>
                {
                    text.Span("GaragePRO Ltd.").SemiBold().FontSize(12);
                    text.Span("\nCompany Street 123, 1234 New City").FontSize(10);
                    text.Span("\nPhone: +386 12 345 678").FontSize(10);
                    text.Span("\nEmail: info@garagepro.com").FontSize(10);
                    text.Span("\nVAT ID: SI12345678").FontSize(10);
                });

                //customer Details
                column.Item().Component(new SectionTitle("Customer Details"));
                column.Item().Text(text =>
                {
                    text.Span($"{Model.WorkOrder?.Vehicle?.Customer?.FullName}").SemiBold().FontSize(12);
                    text.Span($"\n{Model.WorkOrder?.Vehicle?.Customer?.Address}, {Model.WorkOrder?.Vehicle?.Customer?.PostalCode} {Model.WorkOrder?.Vehicle?.Customer?.City}").FontSize(10);
                    text.Span($"\nPhone: {Model.WorkOrder?.Vehicle?.Customer?.Phone}").FontSize(10);
                    text.Span($"\nEmail: {Model.WorkOrder?.Vehicle?.Customer?.Email}").FontSize(10);
                });

                //vehicle Details
                column.Item().Component(new SectionTitle("Vehicle Details"));
                column.Item().Text(text =>
                {
                    text.Span($"Make: {Model.WorkOrder?.Vehicle?.Make}").FontSize(10);
                    text.Span($"\nModel: {Model.WorkOrder?.Vehicle?.Model}").FontSize(10);
                    text.Span($"\nVIN: {Model.WorkOrder?.Vehicle?.VIN}").FontSize(10);
                });

                //service details
                column.Item().Component(new SectionTitle("Service Details"));
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                    });

                    table.Header(header =>
                    {
                        header.Cell().BorderBottom(1).Padding(5).Text("Description").SemiBold();
                        header.Cell().BorderBottom(1).Padding(5).AlignRight().Text("Hours").SemiBold();
                        header.Cell().BorderBottom(1).Padding(5).AlignRight().Text("Rate (€)").SemiBold();
                        header.Cell().BorderBottom(1).Padding(5).AlignRight().Text("Total (€)").SemiBold();
                    });

                    if (Model.WorkOrder?.ServiceDetails != null)
                    {
                        foreach (var detail in Model.WorkOrder.ServiceDetails)
                        {
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(detail.Description);
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignRight().Text(detail.LaborHours.ToString(_slCulture));
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignRight().Text(detail.HourlyRate.ToString("N2", _slCulture));
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignRight().Text((detail.LaborHours * detail.HourlyRate).ToString("N2", _slCulture));
                        }
                    }
                });

                //parts used table
                column.Item().Component(new SectionTitle("Parts Used"));
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                    });

                    table.Header(header =>
                    {
                        header.Cell().BorderBottom(1).Padding(5).Text("Part Name").SemiBold();
                        header.Cell().BorderBottom(1).Padding(5).AlignRight().Text("Price (€)").SemiBold();
                        header.Cell().BorderBottom(1).Padding(5).AlignRight().Text("Quantity").SemiBold();
                        header.Cell().BorderBottom(1).Padding(5).AlignRight().Text("Total (€)").SemiBold();
                    });

                    if (Model.WorkOrder?.PartsUsed != null)
                    {
                        foreach (var part in Model.WorkOrder.PartsUsed)
                        {
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(part.PartCatalog?.PartName);
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignRight().Text(part.PartCatalog?.PartPrice.ToString("N2", _slCulture));
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignRight().Text(part.Quantity.ToString(_slCulture));
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignRight().Text((part.Quantity * (part.PartCatalog?.PartPrice ?? 0m)).ToString("N2", _slCulture));
                        }
                    }
                });

                //price specification
                column.Item().Component(new SectionTitle("Price Specification"));
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    //calculations
                    var partsBase = Model.WorkOrder?.PartsUsed?.Sum(p => p.Quantity * (p.PartCatalog?.PartPrice ?? 0m)) ?? 0m;
                    var partsVAT = partsBase * 0.22m;

                    var laborBase = Model.WorkOrder?.ServiceDetails?.Sum(s => s.HourlyRate * s.LaborHours) ?? 0m;
                    var laborVAT = laborBase * 0.095m;

                    var grandTotalExclVAT = partsBase + laborBase;
                    var grandTotal = grandTotalExclVAT + partsVAT + laborVAT;

                    table.Cell().Padding(2).Text("Total Parts (Excl. VAT):").AlignRight();
                    table.Cell().Padding(2).AlignRight().Text($"{partsBase.ToString("N2", _slCulture)} €");

                    table.Cell().Padding(2).Text("Total Parts VAT (22%):").AlignRight();
                    table.Cell().Padding(2).AlignRight().Text($"{partsVAT.ToString("N2", _slCulture)} €");

                    table.Cell().Padding(2).Text("Total Labour (Excl. VAT):").AlignRight();
                    table.Cell().Padding(2).AlignRight().Text($"{laborBase.ToString("N2", _slCulture)} €");

                    table.Cell().Padding(2).Text("Total Labour VAT (9.5%):").AlignRight();
                    table.Cell().Padding(2).AlignRight().Text($"{laborVAT.ToString("N2", _slCulture)} €");

                    table.Cell().Padding(2).Text("Grand Total (Excl. VAT):").AlignRight();
                    table.Cell().Padding(2).AlignRight().Text($"{grandTotalExclVAT.ToString("N2", _slCulture)} €");

                    table.Cell().BorderTop(1).Padding(2).Text("Grand Total (Incl. VAT):").AlignRight().SemiBold();
                    table.Cell().BorderTop(1).Padding(2).AlignRight().Text($"{grandTotal.ToString("N2", _slCulture)} €").SemiBold();

                    //display payment details if needed
                    table.Cell().Padding(2).Text("Payment Type:").AlignRight();
                    table.Cell().Padding(2).AlignRight().Text($"{Model.PaymentType}");

                    table.Cell().Padding(2).Text("Invoice Status:").AlignRight();
                    table.Cell().Padding(2).AlignRight().Text($"{Model.Status}");
                });
            });
        }

        void ComposeFooter(IContainer container)
        {
            container.AlignRight().Text(x =>
            {
                x.Span("Page ").FontSize(10);
                x.CurrentPageNumber().FontSize(10);
                x.Span(" of ").FontSize(10);
                x.TotalPages().FontSize(10);
            });
        }

        //helper class for consistent section titles
        public class SectionTitle : IComponent
        {
            private readonly string _title;
            public SectionTitle(string title) => _title = title;

            public void Compose(IContainer container)
            {
                container.BorderBottom(1).PaddingBottom(5).Text(_title).SemiBold().FontSize(14).FontColor(Colors.Grey.Darken2);
            }
        }
    }
}