using System;
using UnityEngine;

public class NoteSpawner : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

	public Transform NoteStartLeft, NoteStartRight, NoteEnd;
	public NoteVisual NoteVisualPrefab;

	void Start ()
	{
		Driver.Player.Track.NoteSpawned += note =>
		{
			var visual = Instantiate(NoteVisualPrefab, transform);

			Vector3 startPos = Vector3.Lerp(NoteStartLeft.position, NoteStartRight.position, (float) note.PositionInMeasure / (Track.BEATS_PER_MEASURE - 1));
			Vector3 endPos = new Vector3(startPos.x, NoteEnd.transform.position.y, NoteEnd.transform.position.z);

			visual.transform.position = startPos;
			visual.Initialize(note, Driver.Player.Track, startPos, endPos);
		};
	}
}
