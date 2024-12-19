using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TuDa.CIMS.Api.Models;

namespace TuDa.CIMS.Api.Documents;

public class InvoiceTablesDocument : IDocument
{
    private readonly Invoice _invoice;
    private readonly Image _logo;

    public InvoiceTablesDocument(Invoice invoice, Image logo)
    {
        _invoice = invoice;
        _logo = logo;
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(50);

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);

            page.Footer()
                .AlignRight()
                .Text(text =>
                {
                    text.Span("Seite: ");
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
        });
    }

    private void ComposeHeader(IContainer container) { }

    private void ComposeContent(IContainer container)
    {
        container.Column(column =>
        {
            if (_invoice.Chemicals.Count != 0)
            {
                column.Item().PaddingBottom(7).Text("Chemikalien: ").ExtraBold().FontSize(20);
                column
                    .Item()
                    .PaddingBottom(15)
                    .Element(container => ComposeTable(container, _invoice.Chemicals));
            }

            if (_invoice.Consumables.Count != 0)
            {
                column.Item().PaddingBottom(7).Text("Laborgeräte: ").ExtraBold().FontSize(20);
                column
                    .Item()
                    .PaddingBottom(15)
                    .Element(container => ComposeTable(container, _invoice.Consumables));
            }

            if (_invoice.Solvents.Count != 0)
            {
                column.Item().PaddingBottom(7).Text("Lösungsmittel: ").ExtraBold().FontSize(20);
                column
                    .Item()
                    .PaddingBottom(15)
                    .Element(container => ComposeTable(container, _invoice.Solvents));
            }

            if (_invoice.GasCylinders.Count != 0)
            {
                column.Item().PaddingBottom(7).Text("Technische Gase: ").ExtraBold().FontSize(20);
                column
                    .Item()
                    .PaddingBottom(15)
                    .Element(container => ComposeTable(container, _invoice.GasCylinders));
            }
        });
    }

    private static void ComposeTable(IContainer container, List<InvoiceEntry> entries)
    {
        // Datum, Artikel, Name, Stück, Stückpreis
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(0.4f);
                columns.RelativeColumn();
                columns.RelativeColumn(3.5f);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Text("#");
                header.Cell().Text("Datum");
                header.Cell().Text("Artikel");
                header.Cell().Text("Name");
                header.Cell().Text("Stück");
                header.Cell().Text("Stückpreis");

                header.Cell().ColumnSpan(6).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
            });

            var cellStyle = (IContainer container) =>
                container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);

            foreach ((int index, InvoiceEntry entry) in entries.Index())
            {
                table.Cell().Element(cellStyle).Text($"{index + 1}");
                table.Cell().Element(cellStyle).Text($"{entry.PurchaseDate}");
                table.Cell().Element(cellStyle).Text($"{entry.AssetItem.Name}"); // TODO: There need to be more
                table
                    .Cell()
                    .Element(cellStyle)
                    .Text($"{entry.Buyer.Name}, {entry.Buyer.FirstName}");
                table.Cell().Element(cellStyle).Text($"{entry.Amount}");
                table.Cell().Element(cellStyle).Text($"{entry.PricePerItem}");
            }
        });
    }
}
