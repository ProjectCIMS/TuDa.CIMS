using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Models;

public record AdditionalInvoiceInformation
{
    /// <summary>
    /// The number of the Invoice. Presented in the content heading.
    /// </summary>
    public required string InvoiceNumber { get; init; } = string.Empty;

    /// <summary>
    /// Due date of the invoice. Presented in the summary.
    /// </summary>
    public required DateOnly DueDate { get; init; }

    /// <summary>
    /// Issue date of the invoice. Presented on the right side.
    /// </summary>
    public DateOnly IssueDate { get; init; } = DateOnly.FromDateTime(DateTime.Now);

    /// <summary>
    /// Address of the Issuer. Used as address block on the right side.
    /// </summary>
    public Address IssuerAddress { get; init; } =
        new() { Street = "Peter-Grünberg-Str.", Number = 8 };

    /// <summary>
    /// Email of the Issuer. Used as address block on the right side.
    /// </summary>
    public string IssuerEmail { get; init; } = "annette.przewosnik@tu-darmstadt.de";

    /// <summary>
    /// Telephone number of the Issuer. Used as address block on the right side.
    /// </summary>
    public string IssuerTelephoneNumber { get; init; } = "+49 6151 16 - 23795";

    /// <summary>
    /// Number of the cost center the money is needed to be transfered to.
    /// </summary>
    public string CostCenterNumber { get; init; } = "070087";

    /// <summary>
    /// Number of the project the money is for.
    /// </summary>
    public string ProjectNumber { get; init; } = "58000373";
}
