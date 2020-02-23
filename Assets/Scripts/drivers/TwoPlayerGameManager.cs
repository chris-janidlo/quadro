using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TwoPlayerGameManager : MonoBehaviour
{
	public ADriver Player1Driver, Player2Driver;
	public int Seed;
	public bool UseSeed;

	public HealthBar Player1HealthBar, Player2HealthBar;
	public string GameOverSceneName;

	void Awake ()
	{
		int seedToUse = UseSeed ? Seed : Environment.TickCount;

		Player1Driver.Initialize(seedToUse);
		Player2Driver.Initialize(seedToUse);

		Player1Driver.Player.Opponent = Player2Driver.Player;
		Player2Driver.Player.Opponent = Player1Driver.Player;
	}
	
	void Update ()
	{
		if (Player2HealthBar.VisibleValue.Value == 0) gameOver();
		else if (Player1HealthBar.VisibleValue.Value == 0) gameOver();
	}

	void gameOver ()
	{
		SceneManager.LoadScene(GameOverSceneName);
	}
}
