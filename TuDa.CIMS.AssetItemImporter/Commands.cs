using System.ComponentModel;
using Cocona;
using Refit;
using TuDa.CIMS.AssetItemImporter.Reader;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.AssetItemImporter;

public static class Commands
{
    public static void CheckExcel(
        [Argument(Description = "Asset item type to process")] AssetItemType itemType,
        [Argument(Description = "Path to the Excel .xlsx file")] string excelPath
    )
    {
        var items = GetItems(itemType, excelPath);

        Console.WriteLine($"{items.Count()} '{itemType}' found");

        foreach (var item in items)
        {
            Console.WriteLine(
                $"Name: '{item.Name}', Item number: '{item.ItemNumber}', Price: '{item.Price}', Room: '{item.Room}'"
            );
        }
    }

    public static async Task ImportExcel(
        [Argument(Description = "Asset item type to import")] AssetItemType itemType,
        [Argument(Description = "Path to the Excel .xlsx file")] string excelPath,
        [Argument(Description = "TuDa CIMS API base URL (e.g., https://host)")] string apiUrl
    )
    {
        var items = GetItems(itemType, excelPath);
        var client = RestService.For<IAssetItemApi>(apiUrl);

        foreach (var item in items)
        {
            var result = await client.CreateAsync(item);

            if (result.IsError)
            {
                var error = result.FirstError;
                Console.WriteLine($"Error occured: ({error.Code}) {error.Description}");
            }
            else
            {
                Console.WriteLine($"Successfully created {item.Name}");
            }
        }
    }

    private static IEnumerable<CreateAssetItemDto> GetItems(
        AssetItemType itemType,
        string excelPath
    )
    {
        AssetItemReader reader = itemType switch
        {
            AssetItemType.Lösungsmittel => new SolventReader(excelPath),
            AssetItemType.Laborgeräte => new ConsumableReader(excelPath),
            AssetItemType.Gase => new GasReader(excelPath),
            AssetItemType.Chemikalien => new ChemicalReader(excelPath),
            _ => throw new InvalidEnumArgumentException(),
        };

        return reader.GetAssetItems();
    }
}
