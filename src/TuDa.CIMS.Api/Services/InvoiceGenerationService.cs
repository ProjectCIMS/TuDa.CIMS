using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Api.Models;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Models;

namespace TuDa.CIMS.Api.Services;

[ScopedService]
public class InvoiceGenerationService : IInvoiceGenerationService
{
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceGenerationService(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    public async Task<ErrorOr<Invoice>> CollectInvoiceForWorkingGroup(
        Guid workingGroupId,
        DateOnly? beginDate,
        DateOnly? endDate
    )
    {
        try
        {
            beginDate ??= DateOnly.MinValue;
            endDate ??= DateOnly.MaxValue;

            var professor = await _invoiceRepository.GetProfessorOfWorkingGroup(workingGroupId);

            if (professor is null)
                return Error.NotFound(
                    $"{nameof(InvoiceGenerationService)}.{nameof(CollectInvoiceForWorkingGroup)}",
                    $"WorkingGroup with id {workingGroupId} does not exist."
                );

            var purchasesInTimePeriod = await _invoiceRepository.GetPurchasesInTimePeriod(
                workingGroupId,
                beginDate.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
                endDate.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)
            );

            var invoiceEntries = purchasesInTimePeriod
                .SelectMany(MapPurchaseToInvoiceEntries)
                .ToLookup(e => e.AssetItem.GetType());

            return new Invoice
            {
                BeginDate = beginDate.Value,
                EndDate = endDate.Value,
                Professor = professor,
                Consumables = invoiceEntries[typeof(Consumable)].ToList(),
                Chemicals = invoiceEntries[typeof(Chemical)].ToList(),
                Solvents = invoiceEntries[typeof(Solvent)].ToList(),
                GasCylinders = invoiceEntries[typeof(GasCylinder)].ToList(),
            };
        }
        catch (Exception e)
        {
            return Error.Failure(
                $"{nameof(InvoiceGenerationService)}.{nameof(CollectInvoiceForWorkingGroup)}",
                e.Message
            );
        }
    }

    public Task<ErrorOr<InvoiceStatistics>> GetInvoiceStatistics(
        Guid workingGroupId,
        DateOnly? beginDate,
        DateOnly? endDate
    ) =>
        CollectInvoiceForWorkingGroup(workingGroupId, beginDate, endDate)
            .Match(
                value => new InvoiceStatistics
                {
                    TotalPriceChemicals = value.ChemicalsTotalPrice(),
                    TotalPriceConsumables = value.ConsumablesTotalPrice(),
                    TotalPriceSolvents = value.SolventsTotalPrice(),
                    TotalPriceGasCylinders = value.GasCylindersTotalPrice(),
                },
                ErrorOr<InvoiceStatistics>.From
            );

    private static List<InvoiceEntry> MapPurchaseToInvoiceEntries(Purchase purchase) =>
        purchase
            .Entries.Select(e => new InvoiceEntry
            {
                AssetItem = e.AssetItem,
                Amount = e.Amount,
                PricePerItem = e.PricePerItem,
                PurchaseDate = DateOnly.FromDateTime(purchase.CompletionDate!.Value),
                Buyer = purchase.Buyer,
            })
            .ToList();
}
