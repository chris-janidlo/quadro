using System;
using UnityEngine;

public class NoteSpawner : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

	public Transform NoteStart, NoteEnd;
	public NoteVisual NoteVisualPrefab;

	void Start ()
	{
		// TODO: horizontal position corresponding to position in measure
		Driver.Player.Track.NoteSpawned += note =>
		{
			var newNote = Instantiate(NoteVisualPrefab, transform);
			newNote.transform.position = NoteStart.position;
			newNote.Initialize(note, Driver.Player.Track, NoteStart.position, NoteEnd.position);
		};
	}
}
