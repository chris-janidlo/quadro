using UnityEngine;
using TMPro;

public class NoteVisual : MonoBehaviour
{
	public TextMeshProUGUI Text;

	Note note;
	Track track;
	Vector2 start, target;

	public void Initialize (Note note, Track track, Vector2 start, Vector2 target)
	{
		this.note = note;
		this.track = track;
		this.start = start;
		this.target = target;

		Text.text = note.Symbol.ToRadixRepresentation().ToString();
	}

	void Update ()
	{
		if (note.BeatTicker <= -2)
		{
			Destroy(gameObject);
			return;
		}

		if (note.BeatTicker >= 0)
		{
			double x = (note.BeatsUntilThisNote - track.FractionalPartOfPosition) / Track.BEATS_SHOWN_IN_ADVANCE;

			bool positive = x >= 0;
			transform.position = Vector3.Lerp(target, start, (float) x);
		}
		else
		{
			// TODO: fade out
			Text.alpha = 0;
		}
	}
}
