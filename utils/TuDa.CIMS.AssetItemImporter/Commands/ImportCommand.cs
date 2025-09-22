using System.CommandLine;
using Refit;
using TuDa.CIMS.AssetItemImporter.Reader;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.AssetItemImporter.Commands;

/// <summary>
/// Implements the 'import' command which parses the Excel file via
/// <see cref="Reader.AssetItemReader"/> and creates asset items via
/// the TuDa CIMS HTTP API (<see cref="TuDa.CIMS.Web.Services.IAssetItemApi"/>).
/// </summary>
public class ImportCommand : CommandBase
{
    private static readonly CommandArgument<AssetItemType> s_assetItemTypeArgument =
        new("assetItemType", "Asset item type to process");

    private static readonly CommandArgument<string> s_excelPathArgument =
        new("excelPath", "Path to the Excel .xlsx file");

    private static readonly CommandArgument<string> s_apiUrlArgument =
        new("apiUrl", "TuDa CIMS API base URL (e.g., https://host)");

    public ImportCommand()
        : base(
            "import",
            "Import asset items from typed excel files",
            [
                s_assetItemTypeArgument.AsArgument(),
                s_excelPathArgument.AsArgument(),
                s_apiUrlArgument.AsArgument(),
            ],
            Action
        ) { }

    private static async Task Action(ParseResult parseResult)
    {
        var itemType = s_assetItemTypeArgument.GetRequiredValue(parseResult);
        var excelPath = s_excelPathArgument.GetRequiredValue(parseResult);
        var apiUrl = s_apiUrlArgument.GetRequiredValue(parseResult);

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
                Console.WriteLine($"Error occurred: '({error.Code}) {error.Description}'");
            }
            else
            {
                Console.WriteLine($"Successfully created '{item.Name}'");
            }
        }
    }
}
