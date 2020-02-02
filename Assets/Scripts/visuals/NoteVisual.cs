using UnityEngine;
using TMPro;
using crass;

public class NoteVisual : MonoBehaviour
{
	public TextMeshProUGUI Text;
	public TransitionableColor ColorFader;

	Note note;
	Track track;
	Vector2 start, target;

	Color noAlpha;

	public void Initialize (Note note, Track track, Vector2 start, Vector2 target)
	{
		this.note = note;
		this.track = track;
		this.start = start;
		this.target = target;

		Text.text = note.Symbol.ToRadixRepresentation().ToString();
	}

	void Start ()
	{
		ColorFader.AttachMonoBehaviour(this);
		ColorFader.Value = Text.color;

		noAlpha = new Color(Text.color.r, Text.color.b, Text.color.g, 0);
	}

	void Update ()
	{
		if (note.BeatTicker <= -2)
		{
			Destroy(gameObject);
			return;
		}

		if (note.BeatTicker > 0)
		{
			float x = (float) (note.BeatsUntilThisNote - track.FractionalPartOfPosition) / Track.BEATS_SHOWN_IN_ADVANCE;
			transform.position = Vector3.Lerp(target, start, x);
		}
		else
		{
			transform.position = target;

			ColorFader.StartTransitionToIfNotAlreadyStarted(noAlpha, 60f / track.BPM);
			Text.color = ColorFader.Value;
		}
	}
}
