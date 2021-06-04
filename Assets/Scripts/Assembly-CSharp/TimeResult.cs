using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeResult : MonoBehaviour
{
	public List<Text> Player_Name = new List<Text>();

	public List<Text> Player_Time = new List<Text>();

	public List<bool> TimeIsCalulated = new List<bool>();

	public LapCounter lapCounter;

	public string TextWhenPlayerIsInRace = "Waiting...";

	public bool useCarName;

	public string stringForPlayerText = "P";

	public string stringForCPUText = "CPU ";

	private void Update()
	{
		if (!(lapCounter != null))
		{
			return;
		}
		for (int i = 0; i < Player_Name.Count; i++)
		{
			if (lapCounter.carPosition[i] == 1)
			{
				PlayerName(0, i);
			}
			if (lapCounter.carPosition[i] == 2)
			{
				PlayerName(1, i);
			}
			if (lapCounter.carPosition[i] == 3)
			{
				PlayerName(2, i);
			}
			if (lapCounter.carPosition[i] == 4)
			{
				PlayerName(3, i);
			}
		}
	}

	private void PlayerName(int numText, int value)
	{
		if (lapCounter.carController[value] != null && lapCounter.carController[value].playerNumber == 1)
		{
			if (useCarName)
			{
				Player_Name[numText].text = lapCounter.carController[value].name;
			}
			else
			{
				Player_Name[numText].text = stringForPlayerText + 1;
			}
		}
		else if (lapCounter.carController[value] != null && lapCounter.carController[value].playerNumber == 2 && !lapCounter.carController[value].b_AutoAcceleration)
		{
			if (useCarName)
			{
				Player_Name[numText].text = lapCounter.carController[value].name;
			}
			else
			{
				Player_Name[numText].text = stringForPlayerText + 2;
			}
		}
		else if (useCarName)
		{
			Player_Name[numText].text = lapCounter.carController[value].name;
		}
		else
		{
			Player_Name[numText].text = stringForCPUText;
		}
		if (lapCounter.raceFinished[value])
		{
			Player_Time[numText].text = F_Timer(lapCounter.carTime[value]);
		}
		else
		{
			Player_Time[numText].text = TextWhenPlayerIsInRace;
		}
	}

	private string F_Timer(float value)
	{
		string text = string.Empty;
		if (Mathf.Floor(value / 60f) > 0f && Mathf.Floor(value / 60f) < 10f)
		{
			text = Mathf.Floor(value / 60f).ToString("0");
		}
		if (Mathf.Floor(value / 60f) > 10f)
		{
			text = Mathf.Floor(value / 60f).ToString("00");
		}
		string text2 = Mathf.Floor(value % 60f).ToString("00");
		string text3 = Mathf.Floor(value * 1000f % 1000f).ToString("000");
		string empty = string.Empty;
		if (Mathf.Floor(value / 60f) == 0f)
		{
			return text2 + ":" + text3;
		}
		return text + ":" + text2 + ":" + text3;
	}
}
