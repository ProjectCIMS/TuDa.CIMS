using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TuDa.CIMS.Api.Models;

namespace TuDa.CIMS.Api.Documents;

public class InvoiceCoverDocument : IDocument
{
    private readonly Image _logo;
    private readonly Invoice _invoice;

    #region Colors

    private readonly Color _blue = Color.FromHex("#185F99");
    private readonly Color _grey = Color.FromHex("#676767");

    #endregion

    public InvoiceCoverDocument(Invoice invoice, Image logo)
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
            page.Content().PaddingTop(100).Element(ComposeContent);

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

    private void ComposeHeader(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().LineHorizontal(14).LineColor(_blue);

            column.Item().LineHorizontal(2).LineColor(_grey);

            column
                .Item()
                .PaddingTop(20)
                .Row(row =>
                {
                    row.RelativeItem().Element(ComposeProfessorSalution);
                    row.RelativeItem(0.6f).Image(_logo);
                });
        });
    }

    private void ComposeProfessorSalution(IContainer container)
    {
        container.Column(column =>
        {
            column
                .Item()
                .Text("TU Darmstadt | Peter-Grünberg-Str. 8 | 64287 Darmstadt")
                .FontSize(7)
                .FontColor(_blue);

            column.Item().PaddingTop(15).Text("Herrn");
            column.Item().Text($"Prof. Dr. {Placeholders.Name()}");
            column.Item().Text("Strasse");
            column.Item().Text("TU Darmstadt");
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().PaddingTop(30).Element(ComposeInvoiceIntroduction);

            column.Item().PaddingTop(30).Element(ComposePriceSummary);

            column
                .Item()
                .PaddingTop(70)
                .Text("bis zum 16.09.2024 auf die Kostenstelle 070087 oder");

            column.Item().Text("Kostenstelle 070087 / Projekt 58000373.");
        });
    }

    private void ComposeInvoiceIntroduction(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem()
                .Column(column =>
                {
                    column.Item().Text("Rechnung-Nr. 01/2024").Bold();
                    column
                        .Item()
                        .PaddingTop(50)
                        .Text("Wir bitten um Überweisung für den Verbrauch von:");
                });
            row.AutoItem().Element(ComposeChemicalInventoryAddress);
        });
    }

    private void ComposeChemicalInventoryAddress(IContainer container)
    {
        container
            .AlignRight()
            .Column(column =>
            {
                column.Item().Text("Fachbereich Chemie").FontSize(10).FontColor(_blue);
                column
                    .Item()
                    .PaddingTop(15)
                    .Text("Chemikalienausgabe")
                    .FontSize(10)
                    .FontColor(_blue);

                column.Item().PaddingTop(15).Text("Peter-Grünberg-Str. 8").FontSize(8);

                column.Item().Text("64287 Darmstadt").FontSize(8);

                column.Item().PaddingTop(15).Text("Tel. +49 6151 16 - 23795").FontSize(8);

                column.Item().Text("annette.przewosnik@tu-darmstadt.de").FontSize(8);

                column
                    .Item()
                    .PaddingTop(15)
                    .Text(DateOnly.FromDateTime(DateTime.Now).ToString())
                    .FontSize(8);
            });
    }

    private void ComposePriceSummary(IContainer container)
    {
        container
            .MaxWidth(250)
            .Column(column =>
            {
                if (_invoice.Chemicals.Count != 0)
                    column
                        .Item()
                        .PaddingBottom(3)
                        .Row(row =>
                        {
                            row.AutoItem().Text("Chemikalien: ").AlignLeft();
                            row.RelativeItem()
                                .Text($"{_invoice.ChemicalsTotalPrice:F2} €")
                                .AlignRight();
                        });

                if (_invoice.Consumables.Count != 0)
                    column
                        .Item()
                        .PaddingBottom(3)
                        .Row(row =>
                        {
                            row.AutoItem().Text("Laborgeräte: ").AlignLeft();
                            row.RelativeItem()
                                .Text($"{_invoice.ConsumablesTotalPrice:F2} €")
                                .AlignRight();
                        });

                if (_invoice.Solvents.Count != 0)
                    column
                        .Item()
                        .PaddingBottom(3)
                        .Row(row =>
                        {
                            row.AutoItem().Text("Lösungsmittel: ").AlignLeft();
                            row.RelativeItem()
                                .Text($"{_invoice.SolventsTotalPrice:F2} €")
                                .AlignRight();
                        });

                if (_invoice.GasCylinders.Count != 0)
                    column
                        .Item()
                        .PaddingBottom(3)
                        .Row(row =>
                        {
                            row.AutoItem().Text("Technische Gase: ").AlignLeft();
                            row.RelativeItem()
                                .Text($"{_invoice.GasCylindersTotalPrice:F2} €")
                                .AlignRight();
                        });

                column.Item().PaddingTop(3).PaddingBottom(2).LineHorizontal(1);

                column
                    .Item()
                    .Row(row =>
                    {
                        row.AutoItem().Text("Gesamtsumme: ").Bold().AlignLeft();
                        row.RelativeItem().Text($"{_invoice.TotalPrice:F2} €").Bold().AlignRight();
                    });
            });
    }
}
