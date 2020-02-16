using System;
using UnityEngine;

public class NoteSpawner : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

	public float TrackZDepth, TrackWidth;
	public NoteVisual NoteVisualPrefab;

	void Start ()
	{
		Driver.Player.Track.NoteSpawned += note =>
		{
			var visual = Instantiate(NoteVisualPrefab, transform);
			visual.transform.rotation = Quaternion.identity;

			float x = Mathf.Lerp(-TrackWidth / 2, TrackWidth / 2, (float) note.PositionInMeasure / Track.BEATS_PER_MEASURE);

			Vector3 startPos = new Vector3(x, 0, TrackZDepth);
			Vector3 endPos = new Vector3(x, 0, 0);

			visual.transform.localPosition = startPos;
			visual.Initialize(note, Driver.Player.Track, startPos, endPos);
		};
	}
}
