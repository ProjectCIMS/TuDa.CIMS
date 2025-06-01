using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Dtos.Update;

public record StockUpdateDto(int Amount, TransactionReasons Reason);
