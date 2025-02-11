using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Extensions;

public static class PurchaseExtension
{
    public static CreatePurchaseDto ToCreateDto(this Purchase purchase) =>
        new()
        {
            Buyer = purchase.Buyer.Id,
            Signature = purchase.Signature,
            Entries = purchase.Entries.Select(PurchaseEntryExtension.ToCreateDto).ToList(),
            CompletionDate = purchase.CompletionDate,
        };

    public static CreatePurchaseDto ToCreateDto(this PurchaseResponseDto purchase) =>
        new()
        {
            Buyer = purchase.Buyer.Id,
            Entries = purchase.Entries.Select(PurchaseEntryExtension.ToCreateDto).ToList(),
            CompletionDate = purchase.CompletionDate,
        };

    public static PurchaseResponseDto ToResponseDto(this Purchase purchase) =>
        new()
        {
            Id = purchase.Id,
            Buyer = purchase.Buyer,
            Entries = purchase.Entries,
            CompletionDate = purchase.CompletionDate,
            SuccessorId = purchase.Successor?.Id,
            PredecessorId = purchase.Predecessor?.Id,
        };

    public static List<PurchaseResponseDto> ToResponseDtos(this List<Purchase> purchases) =>
        purchases.Select(ToResponseDto).ToList();
}
