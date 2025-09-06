using System.CommandLine;

namespace TuDa.CIMS.AssetItemImporter.Commands;

/// <summary>
/// Typed wrapper around <see cref="Argument"/>/<see cref="Argument{T}"/> that centralizes
/// creation and value retrieval for a specific type.
/// </summary>
public class CommandArgument<T>(string name, string description, ArgumentArity arity)
{
    public CommandArgument(string name)
        : this(name, string.Empty, ArgumentArity.ExactlyOne) { }

    public CommandArgument(string name, string description)
        : this(name, description, ArgumentArity.ExactlyOne) { }

    /// <summary>Create the underlying System.CommandLine <see cref="Argument{T}"/>.</summary>
    public Argument<T> AsArgument() => new(name) { Description = description, Arity = arity };

    /// <summary>Get a required value from the <see cref="ParseResult"/>, throwing if missing.</summary>
    public T GetRequiredValue(ParseResult parseResult) => parseResult.GetRequiredValue<T>(name);

    /// <summary>Get an optional value from the <see cref="ParseResult"/>, or default if absent.</summary>
    public T? GetValue(ParseResult parseResult) => parseResult.GetValue<T>(name);
}
