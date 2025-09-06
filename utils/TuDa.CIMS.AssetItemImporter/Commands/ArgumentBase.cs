using System.CommandLine;

namespace TuDa.CIMS.AssetItemImporter.Commands;

public class ArgumentBase<T>(string name, string description, ArgumentArity arity)
{
    public ArgumentBase(string name)
        : this(name, string.Empty, ArgumentArity.ExactlyOne) { }

    public ArgumentBase(string name, string description)
        : this(name, description, ArgumentArity.ExactlyOne) { }

    public Argument<T> AsArgument() => new(name) { Description = description, Arity = arity };

    public T GetRequiredValue(ParseResult parseResult) => parseResult.GetRequiredValue<T>(name);

    public T? GetValue(ParseResult parseResult) => parseResult.GetValue<T>(name);
}
