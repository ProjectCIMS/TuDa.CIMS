using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using TuDa.CIMS.Api.Models;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Shared.Models;

namespace TuDa.CIMS.Api.Documents;

public class InvoiceCoverDocument : IDocument
{
    /// <summary>
    /// The invoice, that should be used.
    /// </summary>
    private readonly Invoice _invoice;

    private readonly AdditionalInvoiceInformation _information;

    #region Colors

    private readonly Color _blue = Color.FromHex("#185F99");
    private readonly Color _grey = Color.FromHex("#676767");

    #endregion

    public InvoiceCoverDocument(Invoice invoice, AdditionalInvoiceInformation information)
    {
        _invoice = invoice;
        _information = information;
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
                        + $"{_information.IssuerAddress.Street} {_information.IssuerAddress.Number} | "
                        + $"{_information.IssuerAddress.ZipCode} {_information.IssuerAddress.City}"
                )
                .FontSize(7)
                .FontColor(_blue);

            var professor = _invoice.Professor;

            column.Item().PaddingTop(15).Text(professor.Gender.ToSalutation());
            column.Item().Text($"{professor.Title} {professor.FirstName} {professor.Name}");
            column.Item().Text($"{professor.Address.Street} {professor.Address.Number}");
            column.Item().Text($"{professor.Address.ZipCode} {professor.Address.City}");
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().PaddingTop(30).Element(ComposeInvoiceTitleAndAddress);

            column
                .Item()
                .PaddingTop(90)
                .Text("Bitte überweisen Sie den Betrag innerhalb von 14 Tagen auf die");

            column.Item().Text($"Kostenstelle {_information.CostCenterNumber} oder");

            column
                .Item()
                .Text(
                    $"Kostenstelle {_information.CostCenterNumber} / Projekt {_information.ProjectNumber}."
                );
        });
    }

    private void ComposeInvoiceTitleAndAddress(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem()
                .Column(column =>
                {
                    column.Item().Text($"Rechnung-Nr. {_information.InvoiceNumber}").Bold();

                    column.Item().PaddingTop(60).Element(ComposePriceSummary);
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
                    .Text(
                        $"{_information.IssuerAddress.Street} {_information.IssuerAddress.Number}"
                    )
                    .FontSize(8);

                column
                    .Item()
                    .Text($"{_information.IssuerAddress.ZipCode} {_information.IssuerAddress.City}")
                    .FontSize(8);

                column
                    .Item()
                    .PaddingTop(15)
                    .Text($"Tel: {_information.IssuerTelephoneNumber}")
                    .FontSize(8);

                column.Item().Text(_information.IssuerEmail).FontSize(8);

                column.Item().PaddingTop(15).Text(_information.IssueDate.ToString()).FontSize(8);
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
                                .Text($"{_invoice.ChemicalsTotalPrice():F2} €")
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
                                .Text($"{_invoice.ConsumablesTotalPrice():F2} €")
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
                                .Text($"{_invoice.SolventsTotalPrice():F2} €")
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
                                .Text($"{_invoice.GasCylindersTotalPrice():F2} €")
                                .AlignRight();
                        });

                column.Item().PaddingTop(3).PaddingBottom(2).LineHorizontal(1);

                column
                    .Item()
                    .Row(row =>
                    {
                        row.AutoItem().Text("Gesamtsumme: ").Bold().AlignLeft();
                        row.RelativeItem()
                            .Text($"{_invoice.TotalPrice():F2} €")
                            .Bold()
                            .AlignRight();
                    });
            });
    }
}
