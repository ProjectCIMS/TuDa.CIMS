using System.ComponentModel;
using Cocona;
using Refit;
using TuDa.CIMS.AssetItemImporter.Reader;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.AssetItemImporter;

public static class Commands
{
    public static void CheckExcel([Argument] AssetItemType type, [Argument] string path)
    {
        var items = GetItems(type, path);

        Console.WriteLine($"{items.Count()} {type} gefunden");

        foreach (var item in items)
        {
            Console.WriteLine(
                $"Name: '{item.Name}', Produktnummer: '{item.ItemNumber}', Preis: '{item.Price}', Raum: '{item.Room}'"
            );
        }
    }

    public static async Task ImportExcel(
        [Argument] AssetItemType type,
        [Argument] string path,
        [Argument] string url
    )
    {
        var items = GetItems(type, path);
        var client = RestService.For<IAssetItemApi>(url);

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

    private static IEnumerable<CreateAssetItemDto> GetItems(AssetItemType type, string path)
    {
        AssetItemReader reader = type switch
        {
            AssetItemType.Lösungsmittel => new SolventReader(path),
            AssetItemType.Laborgeräte => new ConsumableReader(path),
            AssetItemType.Gase => new GasReader(path),
            AssetItemType.Chemikalien => new ChemicalReader(path),
            _ => throw new InvalidEnumArgumentException(),
        };

        return reader.GetAssetItems();
    }
}
