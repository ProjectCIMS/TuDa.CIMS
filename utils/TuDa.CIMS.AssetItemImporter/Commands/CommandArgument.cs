using System.CommandLine;

namespace TuDa.CIMS.AssetItemImporter.Commands;

public class CommandArgument<T>(string name, string description, ArgumentArity arity)
{
    public CommandArgument(string name)
        : this(name, string.Empty, ArgumentArity.ExactlyOne) { }

    public CommandArgument(string name, string description)
        : this(name, description, ArgumentArity.ExactlyOne) { }

    public Argument<T> AsArgument() => new(name) { Description = description, Arity = arity };

    public T GetRequiredValue(ParseResult parseResult) => parseResult.GetRequiredValue<T>(name);

    public T? GetValue(ParseResult parseResult) => parseResult.GetValue<T>(name);
}
