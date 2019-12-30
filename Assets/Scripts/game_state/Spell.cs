using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using crass;

public class Spell
{
    public readonly Note MainNote;
    public readonly ReadOnlyCollection<Note> MetaNotes;

    public float Power =>
        MetaNotes.Aggregate(MainNote.InitialPower, (power, metaNote) => metaNote.MetaEffect(power));

    public string Description =>
        MainNote.DescribeMainEffect(Power);

    public List<Note> AllNotes => 
        new List<Note> { MainNote }.Concat(MetaNotes).ToList();

    public Spell (Note mainNote)
    {
        MainNote = mainNote;
        MetaNotes = new List<Note>().AsReadOnly();
    }

    private Spell (Note mainNote, IList<Note> metaNotes)
    {
        MainNote = mainNote;
        MetaNotes = new ReadOnlyCollection<Note>(metaNotes);
    }

    public Spell PlusMetaNote (Note note)
    {
        return new Spell(MainNote, MetaNotes.ConcatItems(note));
    }

    public void CastOn (Track input)
    {
        MainNote.MainEffect(input, Power);
    }

    public bool CanComboInto (Direction direction)
    {
        if (MetaNotes.Count == 0)
        {
            return MainNote.GetMainComboData(direction);
        }
        else
        {
            return MetaNotes.Last().GetMetaComboData(MainNote.Direction, direction);
        }
    }
}
