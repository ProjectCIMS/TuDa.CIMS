using System.CommandLine;
using TuDa.CIMS.AssetItemImporter.Reader;

namespace TuDa.CIMS.AssetItemImporter.Commands;

public class CheckCommand : CommandBase
{
    private static readonly ArgumentBase<AssetItemType> s_assetItemTypeArgument =
        new("assetItemType", "Asset item type to process");

    private static readonly ArgumentBase<string> s_excelPathArgument =
        new("excelPath", "Path to excel file");

    public CheckCommand()
        : base(
            "check",
            "Check parsing of excel files",
            [s_assetItemTypeArgument.AsArgument(), s_excelPathArgument.AsArgument()],
            Action
        ) { }

    private static void Action(ParseResult parseResult)
    {
        var itemType = s_assetItemTypeArgument.GetRequiredValue(parseResult);
        var excelPath = s_excelPathArgument.GetRequiredValue(parseResult);

        var items = AssetItemReader.FromAssetItemType(itemType, excelPath).GetAssetItems();

        Console.WriteLine($"{items.Count()} '{itemType}' found");

        foreach (var item in items)
        {
            Console.WriteLine(
                $"Name: '{item.Name}', Item number: '{item.ItemNumber}', Price: '{item.Price}', Room: '{item.Room}'"
            );
        }
    }
}
