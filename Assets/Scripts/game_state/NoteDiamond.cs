using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class NoteDiamond
{
    public Note this[Direction direction] => notes[direction];

    DirectionBox<Note> _notes;
    DirectionBox<Note> notes
    {
        get
        {
            if (_notes == null)
            {
                _notes = initializeNotes();
            }
            return (DirectionBox<Note>) _notes;
        }
    }

    protected abstract DirectionBox<Note> initializeNotes ();
}
