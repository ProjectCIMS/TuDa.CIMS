using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TuDa.CIMS.Api.Models;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Api.Documents;

public class InvoiceCoverDocument : IDocument
{
    /// <summary>
    /// Logo of the organization to present in the heading.
    /// </summary>
    private readonly Image _logo;

    /// <summary>
    /// The invoice, that should be used.
    /// </summary>
    private readonly Invoice _invoice;

    /// <summary>
    /// The number of the Invoice. Presented in the content heading.
    /// </summary>
    public string InvoiceNumber { private get; init; } = string.Empty;

    /// <summary>
    /// Due date of the invoice. Presented in the summary.
    /// </summary>
    public DateOnly DueDate { private get; init; }

    /// <summary>
    /// Issue date of the invoice. Presented on the right side.
    /// </summary>
    public DateOnly IssueDate { private get; init; } = DateOnly.FromDateTime(DateTime.Now);

    /// <summary>
    /// Address of the Issuer. Used as address block on the right side.
    /// </summary>
    public Address IssuerAddress { private get; init; } =
        new() { Street = "Peter-Grünberg-Str.", BuildingNumber = "8" };

    /// <summary>
    /// Email of the Issuer. Used as address block on the right side.
    /// </summary>
    public string IssuerEmail { private get; init; } = "annette.przewosnik@tu-darmstadt.de";

    /// <summary>
    /// Telephone number of the Issuer. Used as address block on the right side.
    /// </summary>
    public string IssuerTelephoneNumber { private get; init; } = "+49 6151 16 - 23795";

    /// <summary>
    /// Number of the cost center the money is needed to be transfered to.
    /// </summary>
    public string CostCenterNumber { private get; init; } = "070087";

    /// <summary>
    /// Number of the project the money is for.
    /// </summary>
    public string ProjectNumber { private get; init; } = "58000373";

    #region Colors

    private readonly Color _blue = Color.FromHex("#185F99");
    private readonly Color _grey = Color.FromHex("#676767");

    #endregion

    public InvoiceCoverDocument(Invoice invoice, Image logo)
    {
        _invoice = invoice;
        _logo = logo;
    }

    /// <inheritdoc />
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
                .Text(
                    "TU Darmstadt | "
                        + $"{IssuerAddress.Street} {IssuerAddress.BuildingNumber} | "
                        + $"{IssuerAddress.ZipCode} {IssuerAddress.City}"
                )
                .FontSize(7)
                .FontColor(_blue);

            var professor = _invoice.Professor;

            column.Item().PaddingTop(15).Text(professor.Gender.ToSalution());
            column.Item().Text($"{professor.Title} {professor.FirstName} {professor.Name}");
            column.Item().Text($"{professor.Address.Street} {professor.Address.BuildingNumber}");
            column.Item().Text($"{professor.Address.ZipCode} {professor.Address.City}");
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
                .Text($"bis zum {DueDate} auf die Kostenstelle {CostCenterNumber} oder");

            column.Item().Text($"Kostenstelle {CostCenterNumber} / Projekt {ProjectNumber}.");
        });
    }

    private void ComposeInvoiceIntroduction(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem()
                .Column(column =>
                {
                    column.Item().Text($"Rechnung-Nr. {InvoiceNumber}").Bold();
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

                column
                    .Item()
                    .PaddingTop(15)
                    .Text($"{IssuerAddress.Street} {IssuerAddress.BuildingNumber}")
                    .FontSize(8);

                column.Item().Text($"{IssuerAddress.ZipCode} {IssuerAddress.City}").FontSize(8);

                column.Item().PaddingTop(15).Text($"Tel: {IssuerTelephoneNumber}").FontSize(8);

                column.Item().Text(IssuerEmail).FontSize(8);

                column.Item().PaddingTop(15).Text(IssueDate.ToString()).FontSize(8);
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
