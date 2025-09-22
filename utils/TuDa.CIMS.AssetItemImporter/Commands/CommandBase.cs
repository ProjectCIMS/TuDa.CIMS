using System.CommandLine;

namespace TuDa.CIMS.AssetItemImporter.Commands;

/// <summary>
/// Small helper to build a System.CommandLine <see cref="Command"/> from
/// a name, description, arguments and either a sync or async handler
/// (<see cref="Action{T}"/> with <see cref="ParseResult"/> or
/// <see cref="Func{T,TResult}"/> with <see cref="ParseResult"/> and <see cref="Task"/>).
/// </summary>
public class CommandBase
{
    private readonly string _name;
    private readonly string _description;
    private readonly IList<Argument> _arguments;
    private readonly Action<ParseResult>? _syncAction;
    private readonly Func<ParseResult, Task>? _asyncAction;

    /// <summary>
    /// Create a command with a synchronous handler (<see cref="Action{T}"/> with <see cref="ParseResult"/>).
    /// </summary>
    protected CommandBase(
        string name,
        string description,
        IList<Argument> arguments,
        Action<ParseResult> syncAction
    )
        : this(name, description, arguments, syncAction, null) { }

    /// <summary>
    /// Create a command with an asynchronous handler (<see cref="Func{T,TResult}"/> with <see cref="ParseResult"/> and <see cref="Task"/>).
    /// </summary>
    protected CommandBase(
        string name,
        string description,
        IList<Argument> arguments,
        Func<ParseResult, Task> asyncAction
    )
        : this(name, description, arguments, null, asyncAction) { }

    private CommandBase(
        string name,
        string description,
        IList<Argument> arguments,
        Action<ParseResult>? syncAction,
        Func<ParseResult, Task>? asyncAction
    ) =>
        (_name, _description, _arguments, _syncAction, _asyncAction) = (
            name,
            description,
            arguments,
            syncAction,
            asyncAction
        );

    /// <summary>
    /// Build the <see cref="Command"/> instance with the configured handler.
    /// </summary>
    public Command AsCommand()
    {
        var command = new Command(_name, _description);
        foreach (Argument argument in _arguments)
        {
            command.Arguments.Add(argument);
        }

        if (_syncAction is not null)
            command.SetAction(_syncAction);
        else
            command.SetAction(_asyncAction!);

        return command;
    }
}
