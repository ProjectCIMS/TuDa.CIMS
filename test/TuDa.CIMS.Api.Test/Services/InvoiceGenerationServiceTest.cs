using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Api.Services;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Test.Faker;

namespace TuDa.CIMS.Api.Test.Services;

[TestSubject(typeof(InvoiceGenerationService))]
public class InvoiceGenerationServiceTest
{
    private readonly Mock<IInvoiceRepository> _mockInvoiceRepository = new();
    private readonly IInvoiceGenerationService _invoiceGeneration;

    public InvoiceGenerationServiceTest()
    {
        _invoiceGeneration = new InvoiceGenerationService(_mockInvoiceRepository.Object);
    }

    [Fact]
    public async Task CollectingInvoiceEntriesCorrectlyFromOnePurchase()
    {
        var workingGroup = new WorkingGroupFaker().Generate();
        var purchase = new PurchaseFaker(workingGroup, completed: true).Generate();
        Consumable consumable = new ConsumableFaker();
        Chemical chemical = new ChemicalFaker();
        Solvent solvent = new SolventFaker();
        GasCylinder gas = new GasCylinderFaker();

        purchase.Entries =
        [
            new PurchaseEntryFaker(consumable),
            new PurchaseEntryFaker(chemical),
            new PurchaseEntryFaker(solvent),
            new PurchaseEntryFaker(gas),
        ];
        workingGroup.Purchases = [purchase];

        _mockInvoiceRepository
            .Setup(r => r.GetProfessorOfWorkingGroup(workingGroup.Id))
            .ReturnsAsync(workingGroup.Professor);

        _mockInvoiceRepository
            .Setup(r =>
                r.GetPurchasesInTimePeriod(
                    workingGroup.Id,
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()
                )
            )
            .ReturnsAsync(workingGroup.Purchases);

        var errorOrInvoice = await _invoiceGeneration.CollectInvoiceForWorkingGroup(
            workingGroup.Id,
            DateOnly.MinValue,
            DateOnly.MaxValue
        );

        errorOrInvoice.IsError.Should().BeFalse();
        errorOrInvoice.Value.Should().NotBeNull();

        var invoice = errorOrInvoice.Value;
        invoice.Professor.Should().Be(workingGroup.Professor);
        invoice.BeginDate.Should().Be(DateOnly.MinValue);
        invoice.EndDate.Should().Be(DateOnly.MaxValue);

        invoice.Consumables.Single().AssetItem.Should().Be(consumable);
        invoice.Chemicals.Single().AssetItem.Should().Be(chemical);
        invoice.Solvents.Single().AssetItem.Should().Be(solvent);
        invoice.GasCylinders.Single().AssetItem.Should().Be(gas);
    }

    [Fact]
    public async Task CollectingInvoiceEntriesCorrectlyFromMultiplePurchase()
    {
        var workingGroup = new WorkingGroupFaker().Generate();
        var purchase = new PurchaseFaker(workingGroup, completed: true).GenerateBetween(2, 5);
        Consumable consumable = new ConsumableFaker();
        Chemical chemical = new ChemicalFaker();
        Solvent solvent = new SolventFaker();
        GasCylinder gas = new GasCylinderFaker();

        workingGroup.Purchases = purchase
            .Select(p =>
                p with
                {
                    Entries =
                    [
                        new PurchaseEntryFaker(consumable),
                        new PurchaseEntryFaker(chemical),
                        new PurchaseEntryFaker(solvent),
                        new PurchaseEntryFaker(gas),
                    ],
                }
            )
            .ToList();

        _mockInvoiceRepository
            .Setup(r => r.GetProfessorOfWorkingGroup(workingGroup.Id))
            .ReturnsAsync(workingGroup.Professor);

        _mockInvoiceRepository
            .Setup(r =>
                r.GetPurchasesInTimePeriod(
                    workingGroup.Id,
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()
                )
            )
            .ReturnsAsync(workingGroup.Purchases);

        var errorOrInvoice = await _invoiceGeneration.CollectInvoiceForWorkingGroup(
            workingGroup.Id,
            DateOnly.MinValue,
            DateOnly.MaxValue
        );

        errorOrInvoice.IsError.Should().BeFalse();
        errorOrInvoice.Value.Should().NotBeNull();

        var invoice = errorOrInvoice.Value;
        invoice.Professor.Should().Be(workingGroup.Professor);
        invoice.BeginDate.Should().Be(DateOnly.MinValue);
        invoice.EndDate.Should().Be(DateOnly.MaxValue);

        invoice.Consumables.Select(e => e.AssetItem).Should().AllBeOfType<Consumable>();
        invoice.Chemicals.Select(e => e.AssetItem).Should().AllBeOfType<Chemical>();
        invoice.Solvents.Select(e => e.AssetItem).Should().AllBeOfType<Solvent>();
        invoice.GasCylinders.Select(e => e.AssetItem).Should().AllBeOfType<GasCylinder>();
    }
}
