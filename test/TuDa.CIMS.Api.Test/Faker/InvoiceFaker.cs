using Bogus;
using TuDa.CIMS.Api.Models;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Faker;

public class InvoiceFaker : Faker<Invoice>
{
    public InvoiceFaker()
    {
        RuleFor(i => i.Professor, () => new ProfessorFaker());
        RuleFor(i => i.BeginDate, f => f.Date.PastDateOnly());
        RuleFor(i => i.EndDate, f => f.Date.FutureDateOnly());
        RuleFor(i => i.Chemicals, () => new InvoiceEntryFaker<Chemical>().GenerateBetween(0, 30));
        RuleFor(
            i => i.Consumables,
            () => new InvoiceEntryFaker<Consumable>().GenerateBetween(0, 30)
        );
        RuleFor(i => i.Solvents, () => new InvoiceEntryFaker<Solvent>().GenerateBetween(0, 30));
        RuleFor(
            i => i.GasCylinders,
            () => new InvoiceEntryFaker<GasCylinder>().GenerateBetween(0, 30)
        );
    }
}
