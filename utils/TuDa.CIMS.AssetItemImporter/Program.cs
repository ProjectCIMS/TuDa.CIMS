using System.CommandLine;
using TuDa.CIMS.AssetItemImporter.Commands;

RootCommand root =
    new("Import asset items from typed excel files")
    {
        CheckCommand.AsCommand(),
        ImportCommand.AsCommand(),
    };

var result = root.Parse(args);
await result.InvokeAsync();
