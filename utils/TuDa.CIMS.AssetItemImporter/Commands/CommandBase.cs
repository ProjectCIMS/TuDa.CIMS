using System.CommandLine;

namespace TuDa.CIMS.AssetItemImporter.Commands;

public class CommandBase
{
    private readonly string _name;
    private readonly string _description;
    private readonly IList<Argument> _arguments;
    private readonly Action<ParseResult>? _syncAction;
    private readonly Func<ParseResult, Task>? _asyncAction;

    protected CommandBase(
        string name,
        string description,
        IList<Argument> arguments,
        Action<ParseResult> syncAction
    )
        : this(name, description, arguments, syncAction, null) { }

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
