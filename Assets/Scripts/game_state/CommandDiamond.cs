using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class CommandDiamond
{
    public Command this[InputDirection direction] => commands[direction];

    InputDirectionBox<Command> _commands;
    InputDirectionBox<Command> commands
    {
        get
        {
            if (_commands == null)
            {
                _commands = initializeCommands();
            }
            return _commands;
        }
    }

    protected abstract InputDirectionBox<Command> initializeCommands ();
}
