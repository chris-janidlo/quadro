using UnityEngine;
using TMPro;
using crass;

public class ChordVisual : MonoBehaviour, IDriverSubscriber
{
	public ADriver Driver { get; set; }

	public TextMeshProUGUI CurrentChordText, NextChordText, BeatTickerText;

	void Update ()
	{
		CurrentChordText.text = Driver.Player.Track.ChordGenerator.CurrentChord.ToString();
		NextChordText.text = Driver.Player.Track.ChordGenerator.NextChord.ToString();
		BeatTickerText.text = Driver.Player.Track.ChordGenerator.BeatsUntilNextChord.ToString();
	}
}
