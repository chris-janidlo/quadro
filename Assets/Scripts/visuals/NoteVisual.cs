using UnityEngine;
using TMPro;
using crass;

public class NoteVisual : MonoBehaviour
{
	public TextMeshPro Text;
	public TransitionableColor ColorFader;

	Note note;
	Track track;
	Vector3 start, target;

	Color noAlpha;

	public void Initialize (Note note, Track track, Vector3 start, Vector3 target)
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
		if (note.BeatsUntilThisNote > 0)
		{
			float lerpAmount = (float) (note.BeatsUntilThisNote - track.FractionalPartOfPosition) / Track.BEATS_SHOWN_IN_ADVANCE;
			transform.localPosition = Vector3.Lerp(target, start, lerpAmount);
		}
		else
		{
			transform.localPosition = target;

			ColorFader.StartTransitionToIfNotAlreadyStarted(noAlpha, 60f / track.BPM);
			Text.color = ColorFader.Value;
		}
	}
}
