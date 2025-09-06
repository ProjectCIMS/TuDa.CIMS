using System.CommandLine;
using Refit;
using TuDa.CIMS.AssetItemImporter.Reader;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.AssetItemImporter.Commands;

public static class ImportCommand
{
    private const string Name = "import";

    private const string Description = "Import asset items from typed excel files";

    private static class Arguments
    {
        public static class AssetItemType
        {
            public const string Name = "assetItemType";

            public const string Description = "Asset item type to process";

            public static Argument<AssetItemImporter.AssetItemType> AsArgument() =>
                new(Name) { Description = Description, Arity = ArgumentArity.ExactlyOne };
        }

        public static class ExcelPath
        {
            public const string Name = "excelPath";

            public const string Description = "Path to the Excel .xlsx file";

            public static Argument<string> AsArgument() =>
                new(Name) { Description = Description, Arity = ArgumentArity.ExactlyOne };
        }

        public static class ApiUrl
        {
            public const string Name = "apiUrl";

            public const string Description = "TuDa CIMS API base URL (e.g., https://host)";

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
            Arguments.ApiUrl.AsArgument(),
        };

        command.SetAction(Action);
        return command;
    }

    private static async Task Action(ParseResult parseResult)
    {
        var itemType = parseResult.GetRequiredValue<AssetItemType>(Arguments.AssetItemType.Name);
        var excelPath = parseResult.GetRequiredValue<string>(Arguments.ExcelPath.Name);
        var apiUrl = parseResult.GetRequiredValue<string>(Arguments.ApiUrl.Name);

        var items = AssetItemReader.FromAssetItemType(itemType, excelPath).GetAssetItems();

        var clientUrl = apiUrl + IAssetItemApi.RoutePrefix.TrimEnd('/');
        Console.WriteLine($"Running import against '{clientUrl}'");
        var client = RestService.For<IAssetItemApi>(clientUrl);

        foreach (var item in items)
        {
            var result = await client.CreateAsync(item);

            if (result.IsError)
            {
                var error = result.FirstError;
                Console.WriteLine($"Error occured: '({error.Code}) {error.Description}'");
            }
            else
            {
                Console.WriteLine($"Successfully created '{item.Name}'");
            }
        }
    }
}
