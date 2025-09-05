using System.ComponentModel;
using Cocona;
using TuDa.CIMS.AssetItemImporter.Reader;
using TuDa.CIMS.Shared.Dtos.Create;

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
