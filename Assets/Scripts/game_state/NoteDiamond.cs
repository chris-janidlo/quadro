using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class NoteDiamond
{
    public Note this[InputDirection direction] => notes[direction];

    InputDirectionBox<Note> _notes;
    InputDirectionBox<Note> notes
    {
        get
        {
            if (_notes == null)
            {
                _notes = initializeNotes();
            }
            return _notes;
        }
    }

    protected abstract InputDirectionBox<Note> initializeNotes ();
}
