using System.Linq;
using SCJogos.Database;
using SCJogos.Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField]
	private TrackData[] trackList;

	[SerializeField]
	private Image background;

	[SerializeField]
	private Text textCircuitName;

	[SerializeField]
	private Text textLaps;

	[SerializeField]
	private Button buttoPlay;

	[SerializeField]
	private ButtonDLCController buttonDLC;

	private void Start()
	{
		int laps = BARDataManager.Instance.BARConfig.Laps;
		NextScene(0);
		BARDataManager.Instance.BARConfig.Laps = laps;
		textLaps.text = BARDataManager.Instance.BARConfig.Laps.ToString("D2");
	}

	public string GetCurrentTrackSceneName()
	{
		return trackList[BARDataManager.Instance.BARConfig.CurrentTrackSelected].SceneName;
	}

	public string GetCurrentCircuitName()
	{
		return trackList[BARDataManager.Instance.BARConfig.CurrentTrackSelected].CircuitName;
	}

	private bool CheckDLCOwnership()
	{
		DLCType[] dLC = trackList[BARDataManager.Instance.BARConfig.CurrentTrackSelected].DLC;
		foreach (DLCType dlc in dLC)
		{
			if (BARSteamManager.Instance.IsDLCInstalled(dlc))
			{
				return true;
			}
		}
		return false;
	}

	public void AddLaps(int value)
	{
		BARDataManager.Instance.BARConfig.Laps += value;
		if (BARDataManager.Instance.BARConfig.Laps == 0)
		{
			BARDataManager.Instance.BARConfig.Laps = 99;
		}
		else if (BARDataManager.Instance.BARConfig.Laps == 100)
		{
			BARDataManager.Instance.BARConfig.Laps = 1;
		}
		textLaps.text = BARDataManager.Instance.BARConfig.Laps.ToString("D2");
	}

	public bool NextScene(int index)
	{
		BARDataManager.Instance.BARConfig.CurrentTrackSelected += index;
		if (BARDataManager.Instance.BARConfig.CurrentTrackSelected < 0)
		{
			BARDataManager.Instance.BARConfig.CurrentTrackSelected = trackList.Length - 1;
		}
		else if (BARDataManager.Instance.BARConfig.CurrentTrackSelected >= trackList.Length)
		{
			BARDataManager.Instance.BARConfig.CurrentTrackSelected = 0;
		}
		BARDataManager.Instance.BARConfig.Laps = trackList[BARDataManager.Instance.BARConfig.CurrentTrackSelected].NumberOfLaps;
		textLaps.text = BARDataManager.Instance.BARConfig.Laps.ToString("D2");
		textCircuitName.text = trackList[BARDataManager.Instance.BARConfig.CurrentTrackSelected].CircuitName;
		background.sprite = trackList[BARDataManager.Instance.BARConfig.CurrentTrackSelected].Background;
		if (trackList[BARDataManager.Instance.BARConfig.CurrentTrackSelected].DLC.Contains(DLCType.Supporters))
		{
			buttonDLC.DLCType = DLCType.Supporters;
			buttonDLC.GetComponentInChildren<Text>().text = "Available on: \nBarro - Supporters";
		}
		else if (trackList[BARDataManager.Instance.BARConfig.CurrentTrackSelected].DLC.Contains(DLCType.Expansion2020))
		{
			buttonDLC.DLCType = DLCType.Expansion2020;
			buttonDLC.GetComponentInChildren<Text>().text = "Available on: \nBarro - 2020 Expansion";
		}
		else
		{
			buttonDLC.DLCType = DLCType.None;
		}
		buttonDLC.CheckEnable();
		if (CheckDLCOwnership())
		{
			background.color = Color.white;
			buttoPlay.interactable = true;
		}
		else
		{
			buttoPlay.interactable = false;
			background.color = Color.red;
		}
		return true;
	}
}
