using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TuDa.CIMS.Api.Models;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Api.Documents;

public class InvoiceTablesDocument : IDocument
{
    private readonly Invoice _invoice;

    public InvoiceTablesDocument(Invoice invoice)
    {
        _invoice = invoice;
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(40);

            page.DefaultTextStyle(style => style.FontSize(11));

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

            column.Item().Element(container => ComposeTable(container, entries));

            column.Item().LineHorizontal(2);

            column
                .Item()
                .PaddingTop(3)
                .PaddingBottom(20)
                .Text(
                    $"Gesamtpreis: {entries.Aggregate(0d, (all, entry) => all + entry.TotalPrice):C}"
                )
                .AlignRight();
        });
    }

    private static void ComposeTable(IContainer container, List<InvoiceEntry> entries)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(0.3f);
                columns.ConstantColumn(1); // Vertical Line
                columns.RelativeColumn(1.3f);
                columns.RelativeColumn(2.3f);
                columns.RelativeColumn(2.0f);
                columns.ConstantColumn(1); // Vertical Line
                columns.RelativeColumn();
                columns.RelativeColumn(1.5f);
                columns.ConstantColumn(1); // Vertical Line
                columns.RelativeColumn(1.1f);
            });

            table.Header(header =>
            {
                header.Cell().Text("#");

                header.Cell(); // Vertical Line

                header.Cell().Text("Datum");
                header.Cell().Text("Artikel");
                header.Cell().Text("Name");

                header.Cell(); // Vertical Line

                header.Cell().Text("Anzahl").AlignCenter();
                header.Cell().Text("Preis").AlignCenter();

                header.Cell(); // Vertical Line

                header.Cell().Text("Endpreis").AlignRight();

                header
                    .Cell()
                    .ColumnSpan(10)
                    .PaddingTop(5)
                    .BorderBottom(1)
                    .BorderColor(Colors.Black);
            });

            foreach (
                (int index, InvoiceEntry entry) in entries.OrderBy(x => x.PurchaseDate).Index()
            )
            {
                var priceUnit = AssetItemToPriceUnit(entry.AssetItem);
                table.Cell().Element(CellStyle).Text($"{index + 1}");

                table.Cell().Element(VerticalLine);

                table.Cell().Element(CellStyle).Text($"{entry.PurchaseDate}").AlignCenter();
                table.Cell().Element(CellStyle).Text($"{entry.AssetItem.Name}"); // TODO: There need to be more
                table
                    .Cell()
                    .Element(CellStyle)
                    .PaddingRight(2)
                    .Text($"{entry.Buyer.Name}, {entry.Buyer.FirstName}");

                table.Cell().Element(VerticalLine);

                table
                    .Cell()
                    .Element(CellStyle)
                    .Text($"{AmountString(entry)} {priceUnit}")
                    .AlignCenter();
                table
                    .Cell()
                    .Element(CellStyle)
                    .PaddingRight(3)
                    .Text($"{entry.PricePerItem:C}/{priceUnit}")
                    .AlignCenter();

                table.Cell().Element(VerticalLine);

                table.Cell().Element(CellStyle).Text($"{entry.TotalPrice:C}").AlignRight();
            }
        });
    }

    private static IContainer CellStyle(IContainer container) =>
        container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(3);

    private static IContainer VerticalLine(IContainer container) =>
        container.Background("#000").Width(1).Height(1); // Adjust height as needed

    private static string AssetItemToPriceUnit(AssetItem assetItem) =>
        assetItem switch
        {
            Substance substance => substance.PriceUnit.ToDocumentAbbreviation(),
            _ => MeasurementUnits.Piece.ToDocumentAbbreviation(),
        };

    private static string AmountString(InvoiceEntry entry) =>
        entry.AssetItem switch
        {
            Substance { PriceUnit: not MeasurementUnits.Piece } => $"{entry.Amount:0.00}",
            _ => $"{(int)entry.Amount}",
        };
}
