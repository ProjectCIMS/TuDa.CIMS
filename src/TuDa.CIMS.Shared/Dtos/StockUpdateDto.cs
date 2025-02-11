using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Dtos;

public record StockUpdateDto(int Amount, TransactionReasons Reason);
