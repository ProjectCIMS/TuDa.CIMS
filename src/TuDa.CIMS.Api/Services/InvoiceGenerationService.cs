using Mapster;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Api.Models;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Services;

public class InvoiceGenerationService : IInvoiceGenerationService
{
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceGenerationService(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }

    public async Task<ErrorOr<Invoice>> CollectInvoiceForWorkingGroup(
        Guid workingGroupId,
        DateOnly beginDate,
        DateOnly endDate
    )
    {
        try
        {
            var purchasesInTimePeriod = await _invoiceRepository.GetPurchasesInTimePeriod(
                workingGroupId,
                beginDate.ToDateTime(TimeOnly.MinValue),
                endDate.ToDateTime(TimeOnly.MinValue)
            );

            var professor = await _invoiceRepository.GetProfessorOfWorkingGroup(workingGroupId);

            var invoiceEntries = purchasesInTimePeriod
                .SelectMany(MapPurchaseToInvoiceEntries)
                .ToLookup(e => e.AssetItem.GetType());

            return new Invoice
            {
                BeginDate = beginDate,
                EndDate = endDate,
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
