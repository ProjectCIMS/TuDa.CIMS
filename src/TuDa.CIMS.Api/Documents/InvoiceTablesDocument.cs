using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TuDa.CIMS.Api.Models;

namespace TuDa.CIMS.Api.Documents;

public class InvoiceTablesDocument : IDocument
{
    private readonly Invoice _invoice;

    private const int ColumnsOnOnePage = 26;

    public InvoiceTablesDocument(Invoice invoice)
    {
        _invoice = invoice;
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(50);

            page.DefaultTextStyle(style => style.FontSize(11));

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
                column
                    .Item()
                    .Element(container =>
                        ComposeAssetItemTypeView(container, "Chemikalien: ", _invoice.Chemicals)
                    );

            if (_invoice.Consumables.Count != 0)
            {
                if (_invoice.Chemicals.Count != 0)
                    column.Item().PageBreak();

                column
                    .Item()
                    .Element(container =>
                        ComposeAssetItemTypeView(container, "Laborgeräte: ", _invoice.Consumables)
                    );
            }

            if (_invoice.Solvents.Count != 0)
            {
                if (_invoice.Chemicals.Count != 0 || _invoice.Consumables.Count != 0)
                    column.Item().PageBreak();

                column
                    .Item()
                    .Element(container =>
                        ComposeAssetItemTypeView(container, "Lösungsmittel: ", _invoice.Solvents)
                    );
            }

            if (_invoice.GasCylinders.Count != 0)
            {
                if (
                    _invoice.Chemicals.Count != 0
                    || _invoice.Consumables.Count != 0
                    || _invoice.Solvents.Count != 0
                )
                    column.Item().PageBreak();

                column
                    .Item()
                    .Element(container =>
                        ComposeAssetItemTypeView(
                            container,
                            "Technische Gase: ",
                            _invoice.GasCylinders
                        )
                    );
            }
        });
    }

    private static void ComposeAssetItemTypeView(
        IContainer container,
        string title,
        List<InvoiceEntry> entries
    )
    {
        container.Column(column =>
        {
            column.Item().PaddingBottom(7).Text(title).ExtraBold().FontSize(20);

            var tables = new List<List<InvoiceEntry>>();

            for (int i = 0; i < entries.Count / ColumnsOnOnePage + 1; i++)
            {
                var table = entries.Skip(i * ColumnsOnOnePage).Take(ColumnsOnOnePage).ToList();
                if (table.Count > 0)
                    tables.Add(table);
            }

            Console.WriteLine(entries.Count);

            for (int i = 0; i < tables.Count; i++)
            {
                column.Item().Element(container => ComposeTable(container, tables[i], i));
                if (i + 1 < tables.Count && tables.Count > 1)
                    column.Item().PageBreak();
            }

            column.Item().LineHorizontal(2);

            column
                .Item()
                .PaddingTop(3)
                .PaddingBottom(20)
                .Text(
                    $"Gesamtpreis: {entries.Aggregate(0d, (all, entry) => all + entry.TotalPrice):F2} €"
                )
                .AlignRight();
        });
    }

    private static void ComposeTable(IContainer container, List<InvoiceEntry> entries, int tableNum)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(0.4f);
                columns.RelativeColumn(1.5f);
                columns.RelativeColumn(3.5f);
                columns.RelativeColumn(2.5f);
                columns.ConstantColumn(1);
                columns.RelativeColumn();
                columns.RelativeColumn(1.5f);
            });

            table.Header(header =>
            {
                header.Cell().Text("#");
                header.Cell().Text("Datum");
                header.Cell().Text("Artikel");
                header.Cell().Text("Name");

                header.Cell();

                header.Cell().Text("Anzahl").AlignRight();
                header.Cell().Text("Stückpreis").AlignRight();

                header.Cell().ColumnSpan(7).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Black);
            });

            foreach (
                (int index, InvoiceEntry entry) in entries.OrderBy(x => x.PurchaseDate).Index()
            )
            {
                table.Cell().Element(CellStyle).Text($"{index + 1 + tableNum * ColumnsOnOnePage}");
                table.Cell().Element(CellStyle).Text($"{entry.PurchaseDate}");
                table.Cell().Element(CellStyle).Text($"{entry.AssetItem.Name}"); // TODO: There need to be more
                table
                    .Cell()
                    .Element(CellStyle)
                    .Text($"{entry.Buyer.Name}, {entry.Buyer.FirstName}");

                table.Cell().Element(VerticalLine);

                table.Cell().Element(CellStyle).Text($"{entry.Amount}").AlignRight();
                table.Cell().Element(CellStyle).Text($"{entry.PricePerItem:F2}€").AlignRight();
            }
        });
    }

    private static IContainer CellStyle(IContainer container) =>
        container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);

    private static IContainer VerticalLine(IContainer container) =>
        container.Background("#000").Width(1).Height(1); // Adjust height as needed
}