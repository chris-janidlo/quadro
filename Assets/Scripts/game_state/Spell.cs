using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using crass;

public class Spell
{
    public readonly Command MainCommand;
    public readonly ReadOnlyCollection<Command> MetaCommands;

    public Command.EffectVector EffectVector =>
        MetaCommands.Aggregate(MainCommand.InitialVector, (vector, metaCommand) => metaCommand.MetaEffect(vector));

    public string Description =>
        MainCommand.DescribeMainEffect(EffectVector);

    public ReadOnlyCollection<Command> AllCommands => 
        new List<Command> { MainCommand }.Concat(MetaCommands).ToList().AsReadOnly();
    
    public Command LastCommand =>
        MetaCommands.Count == 0 ? MainCommand : MetaCommands.Last();

    public Spell (Command mainCommand)
    {
        MainCommand = mainCommand;
        MetaCommands = new List<Command>().AsReadOnly();
    }

    private Spell (Command mainCommand, IList<Command> metaCommands)
    {
        MainCommand = mainCommand;
        MetaCommands = new ReadOnlyCollection<Command>(metaCommands);
    }

    public Spell PlusMetaCommand (Command command)
    {
        return new Spell(MainCommand, MetaCommands.ConcatItems(command));
    }

    public void CastOn (Track input)
    {
        MainCommand.MainEffect(input, EffectVector);
    }

    public bool CanComboInto (InputDirection direction)
    {
        if (MetaCommands.Count == 0)
        {
            return MainCommand.GetMainComboData(direction);
        }
        else
        {
            return MainCommand.GetMetaComboData(MetaCommands.Last().Direction, direction);
        }
    }
}
