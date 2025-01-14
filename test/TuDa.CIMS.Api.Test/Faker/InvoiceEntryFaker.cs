using Bogus;
using TuDa.CIMS.Api.Models;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Test.Faker;
using Person = TuDa.CIMS.Shared.Entities.Person;

namespace TuDa.CIMS.Api.Test.Faker;

public class InvoiceEntryFaker<TAssetItem> : PurchaseEntryFaker<InvoiceEntry, TAssetItem>
    where TAssetItem : AssetItem
{
    public InvoiceEntryFaker(TAssetItem assetItem)
        : this(null, assetItem) { }

    public InvoiceEntryFaker(
        AssetItemFaker<TAssetItem>? assetItemFaker = null,
        TAssetItem? assetItem = null
    )
        : base(assetItem)
    {
        RuleFor(e => e.PurchaseDate, f => f.Date.PastDateOnly());
        RuleFor(
            e => e.Buyer,
            f => f.PickRandom<Person>(new ProfessorFaker(), new PersonFaker<Student>())
        );
    }
}
