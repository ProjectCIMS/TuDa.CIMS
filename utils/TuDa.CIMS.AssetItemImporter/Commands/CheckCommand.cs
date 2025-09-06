using System.CommandLine;
using TuDa.CIMS.AssetItemImporter.Reader;

namespace TuDa.CIMS.AssetItemImporter.Commands;

public static class CheckCommand
{
    private const string Name = "check";

    private const string Description = "Check parsing of excel files";

    private static class Arguments
    {
        public static class AssetItemType
        {
            public const string Name = "assetItemType";

            private const string Description = "Asset item type to process";

            public static Argument<AssetItemImporter.AssetItemType> AsArgument() =>
                new(Name) { Description = Description, Arity = ArgumentArity.ExactlyOne };
        }

        public static class ExcelPath
        {
            public const string Name = "excelPath";

            private const string Description = "Path to the Excel .xlsx file";

            public static Argument<string> AsArgument() =>
                new(Name) { Description = Description, Arity = ArgumentArity.ExactlyOne };
        }
    }

    public static Command AsCommand()
    {
        var command = new Command(Name, Description)
        {
            Arguments.AssetItemType.AsArgument(),
            Arguments.ExcelPath.AsArgument(),
        };

        command.SetAction(Action);

        return command;
    }

    private static void Action(ParseResult parseResult)
    {
        var itemType = parseResult.GetRequiredValue<AssetItemType>(Arguments.AssetItemType.Name);
        var excelPath = parseResult.GetRequiredValue<string>(Arguments.ExcelPath.Name);

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
