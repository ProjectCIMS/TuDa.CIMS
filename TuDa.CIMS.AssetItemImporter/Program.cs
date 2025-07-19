using Cocona;
using TuDa.CIMS.AssetItemImporter;
using TuDa.CIMS.AssetItemImporter.Reader;

await CoconaApp.RunAsync(
    ([Argument] string path, [Argument] AssetItemType type) =>
    {
        AssetItemReader reader = type switch
        {
            AssetItemType.Laborgeräte => new ConsumableReader(path),
            _ => throw new NotImplementedException(),
        };

        var items = reader.GetAssetItems();

        Console.Out.WriteLine(items.ToList());
    }
);
