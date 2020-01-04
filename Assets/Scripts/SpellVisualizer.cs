using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellVisualizer : MonoBehaviour
{
    public ADriver Driver;
    public Image ChipPrefab;

    Spell spell => Driver.State.CurrentSpell;
    List<Note> noteCache = new List<Note>();

    void Update ()
    {
        if (spell != null && !noteCache.SequenceEqual(spell.AllNotes)) updateVisuals();

        if (Driver.State.Rhythm.ComboCounter == 0 && transform.childCount != 0)
        {
            noteCache = new List<Note>();
            clearVisuals();
        }
    }

    void updateVisuals ()
    {
        noteCache = new List<Note>(spell.AllNotes);

        clearVisuals();

        foreach (Note note in noteCache)
        {
            Image chip = Instantiate(ChipPrefab);
            chip.transform.SetParent(transform);
            chip.color = note.Color;
        }
    }

    void clearVisuals ()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
