using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Steamworks;
using SCJogos.Database;
using SCJogos.Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardSystem : MonoBehaviour
{
	public Text txt_PlayerName;

	public int OffsetLetter = 4;

	public Text txt_Leaderboard_Score;

	public GameObject obj_PauseButton;

	public GameObject obj_PanelLeaderboard;

	public Text txt_OnLeaderboard_LCD_Name;

	public Text txt_OnLeaderboard_Name;

	public Text txt_OnLeaderboard_Position;

	public Text txt_OnLeaderboard_Score;

	public Text txt_OnLeaderboard_Loop;

	public int MaxScoreDisplay = 50;

	[SerializeField]
	private GameObject panelLeaderboardDeactivated;

	private LapCounter lapCounter;

	private MainMenu mainMenu;

	private void Start()
	{
		lapCounter = UnityEngine.Object.FindObjectOfType<LapCounter>();
		mainMenu = UnityEngine.Object.FindObjectOfType<MainMenu>();
		if ((bool)txt_PlayerName)
		{
			txt_PlayerName = txt_PlayerName.gameObject.GetComponent<Text>();
		}
		if ((bool)txt_PlayerName)
		{
			txt_PlayerName.text = BARSteamManager.Instance.PlayerName;
		}
	}

	public void SaveBestLap()
	{
		lapCounter.SaveBestLap();
	}

	public void InitPlayerScore()
	{
		if ((bool)txt_Leaderboard_Score)
		{
			txt_Leaderboard_Score.text = F_Timer(lapCounter.BestLap);
		}
	}

	private string F_Timer(float value)
	{
		return default(DateTime).AddSeconds(value).ToString("mm:ss:fff");
	}

	public IEnumerator UpdateLeaderboard()
	{
		panelLeaderboardDeactivated.SetActive(!BARDataManager.Instance.ConfigData.UseLeaderboard);
		if ((bool)txt_OnLeaderboard_LCD_Name)
		{
			txt_OnLeaderboard_LCD_Name.text = mainMenu.GetCurrentCircuitName();
		}
		if ((bool)txt_OnLeaderboard_Position)
		{
			txt_OnLeaderboard_Position.text = "Loading...";
		}
		if ((bool)txt_OnLeaderboard_Name)
		{
			txt_OnLeaderboard_Name.text = string.Empty;
		}
		if ((bool)txt_OnLeaderboard_Score)
		{
			txt_OnLeaderboard_Score.text = string.Empty;
		}
		WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
		yield return waitForEndOfFrame;
		BARSteamManager.Instance.GetScores(mainMenu.GetCurrentTrackSceneName());
		yield return waitForEndOfFrame;
		if ((bool)txt_OnLeaderboard_Position)
		{
			txt_OnLeaderboard_Position.text = string.Empty;
		}
		List<LeaderboardResult> scores = BARSteamManager.Instance.TopScores[mainMenu.GetCurrentTrackSceneName()];
		if (scores.Count <= 0)
		{
			yield break;
		}
		int playerPos = -1;
		LeaderboardResult player = scores.FirstOrDefault((LeaderboardResult i) => i.PlayerID == BARSteamManager.Instance.PlayerID);
		if (player != null)
		{
			playerPos = scores.IndexOf(player);
		}
		int max = ((playerPos <= 9) ? Mathf.Min(10, scores.Count) : Mathf.Min(9, scores.Count));
		for (int j = 0; j < max; j++)
		{
			yield return waitForEndOfFrame;
			string color = ((j != playerPos) ? "<color=#FFFFFFFF>" : "<color=#FF0000FF>");
			if ((bool)txt_OnLeaderboard_Position)
			{
				Text text = txt_OnLeaderboard_Position;
				text.text = text.text + color + (j + 1) + "\n</color>";
			}
			if ((bool)txt_OnLeaderboard_Name)
			{
				Text text2 = txt_OnLeaderboard_Name;
				text2.text = text2.text + color + scores[j].PlayerName.Substring(0, Mathf.Min(scores[j].PlayerName.Length, 16)) + "\n</color>";
			}
			if ((bool)txt_OnLeaderboard_Score)
			{
				Text text3 = txt_OnLeaderboard_Score;
				text3.text = text3.text + color + F_Timer(scores[j].PlayerScore) + "\n</color>";
			}
		}
		yield return waitForEndOfFrame;
		if (playerPos > 9)
		{
			string color = "<color=#FF0000FF>";
			if ((bool)txt_OnLeaderboard_Position)
			{
				Text text4 = txt_OnLeaderboard_Position;
				text4.text = text4.text + color + (playerPos + 1) + "\n</color>";
			}
			if ((bool)txt_OnLeaderboard_Name)
			{
				Text text5 = txt_OnLeaderboard_Name;
				text5.text = text5.text + color + scores[playerPos].PlayerName + "\n</color>";
			}
			if ((bool)txt_OnLeaderboard_Score)
			{
				Text text6 = txt_OnLeaderboard_Score;
				text6.text = text6.text + color + F_Timer(scores[playerPos].PlayerScore) + "\n</color>";
			}
		}
	}

	public void FinishRace()
	{
		Game_Manager.instance.FinishRace();
	}
}
