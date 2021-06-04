using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Steamworks;
using SCJogos.Database;
using SCJogos.Steamworks;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;

public class LapCounter : MonoBehaviour
{
	public class progressionCompare : IComparable<progressionCompare>
	{
		public CarPathFollow car;

		public int total;

		public progressionCompare(CarPathFollow newCar, int newtotal)
		{
			car = newCar;
			total = newtotal;
		}

		public int CompareTo(progressionCompare other)
		{
			if (other == null)
			{
				return 1;
			}
			return total - other.total;
		}
	}

	public Game_Manager gameManager;

	public bool SeeInspector;

	public bool b_Pause;

	public Color GizmoColor = new Color(1f, 0.92f, 0.016f, 0.5f);

	public bool b_ActivateLapCounter = true;

	public int lapNumber = 3;

	public List<CarPathFollow> car = new List<CarPathFollow>();

	public List<CarController> carController = new List<CarController>();

	public List<float> carProgressDistance = new List<float>();

	public List<int> carLap = new List<int>();

	public List<float> carTime = new List<float>();

	public List<bool> raceFinished = new List<bool>();

	public List<int> carPosition = new List<int>();

	public DateTime Timer = default(DateTime);

	public bool startTimer;

	private WaypointCircuit Track;

	private float trackLengthReference;

	public Text Txt_P1;

	public Text Txt_P2;

	public Text Txt_Timer;

	public Text Txt_P1_Lap;

	public Text Txt_P2_Lap;

	public float RefreshPosTime_ = 0.2f;

	private float RefreshPositionTimer;

	private bool player2IsManageByCPU = true;

	private bool initDone;

	public InventoryCar inventoryItemCar;

	private progressionCompare[] playersPositions;

	private BestLapText bestLapText;

	private float bestLap;

	private float lastTime;

	private float trackRecord;

	private int lastLap;

	private bool timeTrial;

	private Text textNewRecord;

	public float BestLap
	{
		get
		{
			return bestLap;
		}
	}

	private void Start()
	{
		bestLap = float.MaxValue;
		lastTime = 0f;
		lastLap = 1;
		timeTrial = BARDataManager.Instance.BARConfig.GameMode == GameModeType.TimeTrial;
		bestLapText = UnityEngine.Object.FindObjectsOfType<BestLapText>().FirstOrDefault((BestLapText i) => i.P1);
		textNewRecord = UnityEngine.Object.FindObjectOfType<Menu_Manager>().GetComponentInChildren<TextNewRecord>(true).GetComponent<Text>();
		if ((bool)inventoryItemCar)
		{
			b_ActivateLapCounter = inventoryItemCar.b_LapCounter;
		}
		Track = UnityEngine.Object.FindObjectOfType<WaypointCircuit>();
		gameManager = UnityEngine.Object.FindObjectOfType<Game_Manager>();
		if (timeTrial)
		{
			lapNumber = int.MaxValue;
		}
		else
		{
			lapNumber = BARDataManager.Instance.BARConfig.Laps;
		}
		if (gameManager.inventoryItemList.inventoryItem[0].b_TestMode)
		{
			InitCar();
		}
		if (timeTrial)
		{
			Txt_P1_Lap.text = string.Empty;
			Txt_P2_Lap.text = string.Empty;
		}
		else
		{
			Txt_P1_Lap.text = "Lap 1/" + lapNumber;
			Txt_P2_Lap.text = "Lap 1/" + lapNumber;
		}
		playersPositions = new progressionCompare[car.Count];
		for (int j = 0; j < playersPositions.Length; j++)
		{
			playersPositions[j] = new progressionCompare(null, 0);
		}
		trackRecord = float.MaxValue;
		StartCoroutine(StartTrackRecord());
	}

	private IEnumerator StartTrackRecord()
	{
		yield return new WaitForEndOfFrame();
		BARSteamManager.Instance.GetScores(gameManager.TrackData.SceneName);
		LeaderboardResult score = BARSteamManager.Instance.TopScores[gameManager.TrackData.SceneName].FirstOrDefault();
		if (score != null)
		{
			trackRecord = score.PlayerScore;
		}
	}

	public void InitCar()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Car");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if ((bool)gameObject.GetComponent<CarController>())
			{
				car[gameObject.GetComponent<CarController>().playerNumber - 1] = gameObject.GetComponent<CarPathFollow>();
				carController[gameObject.GetComponent<CarController>().playerNumber - 1] = gameObject.GetComponent<CarController>();
				if (gameObject.GetComponent<CarController>().playerNumber == 2 && !gameObject.GetComponent<CarAI>().enabled)
				{
					player2IsManageByCPU = false;
				}
			}
		}
		initDone = true;
	}

	private void Update()
	{
		if (b_Pause || !initDone)
		{
			return;
		}
		for (int i = 0; i < car.Count; i++)
		{
			if (car[i] != null)
			{
				carLap[i] = car[i].iLapCounter;
				carProgressDistance[i] = car[i].progressDistance;
			}
		}
		if (Track != null && trackLengthReference == 0f)
		{
			trackLengthReference = Track.Length;
		}
		positionOnRace();
		if (startTimer)
		{
			F_Timer();
		}
	}

	private void F_Timer()
	{
		try
		{
			Timer = Timer.AddSeconds(Time.deltaTime);
		}
		catch
		{
		}
	}

	private void positionOnRace()
	{
		RefreshPositionTimer = Mathf.MoveTowards(RefreshPositionTimer, RefreshPosTime_, Time.deltaTime);
		if (RefreshPositionTimer < RefreshPosTime_)
		{
			return;
		}
		for (int i = 0; i < car.Count; i++)
		{
			float f = ((float)carLap[i] * trackLengthReference + carProgressDistance[i]) * 1000f;
			playersPositions[i].car = car[i];
			playersPositions[i].total = Mathf.RoundToInt(f);
		}
		Array.Sort(playersPositions);
		for (int num = playersPositions.Length - 1; num >= 0; num--)
		{
			if (!raceFinished[0] && playersPositions[num].car == car[0] && (bool)Txt_P1)
			{
				if (RefreshPositionTimer == RefreshPosTime_)
				{
					Txt_P1.text = car.Count - num + "/" + car.Count;
				}
				carPosition[0] = car.Count - num;
			}
			if (!raceFinished[1] && playersPositions[num].car == car[1] && (bool)Txt_P2)
			{
				if (RefreshPositionTimer == RefreshPosTime_)
				{
					Txt_P2.text = car.Count - num + "/" + car.Count;
				}
				carPosition[1] = car.Count - num;
			}
			if (!raceFinished[2] && playersPositions[num].car == car[2])
			{
				carPosition[2] = car.Count - num;
			}
			if (!raceFinished[3] && playersPositions[num].car == car[3])
			{
				carPosition[3] = car.Count - num;
			}
		}
		if (RefreshPositionTimer == RefreshPosTime_)
		{
			RefreshPositionTimer = 0f;
		}
	}

	public void Pause()
	{
		b_Pause = !b_Pause;
	}

	public void SaveBestLap()
	{
		if (bestLap < float.MaxValue && timeTrial)
		{
			StartCoroutine(SaveData());
		}
	}

	private IEnumerator SaveData()
	{
		BARSteamManager.Instance.PostScore(gameManager.TrackData.SceneName, bestLap);
		yield return null;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Car") || !b_ActivateLapCounter)
		{
			return;
		}
		CarPathFollow component = other.GetComponent<CarPathFollow>();
		try
		{
			if (car[0] != null && car[0].name == other.name && lastLap < car[0].iLapCounter && timeTrial)
			{
				float num = (float)TimeSpan.FromTicks(Timer.Ticks).TotalSeconds;
				float num2 = num - lastTime;
				lastTime = num;
				lastLap = car[0].iLapCounter;
				bestLapText.ShowTime(num2, bestLap);
				if (num2 < trackRecord)
				{
					textNewRecord.text = "New Record!";
				}
				if (num2 < bestLap)
				{
					bestLap = num2;
				}
			}
		}
		catch
		{
		}
		if (component.iLapCounter <= lapNumber)
		{
			if (car[0] != null && car[0].name == other.name && (bool)Txt_P1_Lap)
			{
				Txt_P1_Lap.text = "Lap " + car[0].iLapCounter;
				if (!timeTrial)
				{
					Text txt_P1_Lap = Txt_P1_Lap;
					txt_P1_Lap.text = txt_P1_Lap.text + "/" + lapNumber;
				}
			}
			else if (car[1] != null && car[1].name == other.name && (bool)Txt_P2_Lap)
			{
				Txt_P2_Lap.text = "Lap " + car[1].iLapCounter;
				if (!timeTrial)
				{
					Text txt_P2_Lap = Txt_P2_Lap;
					txt_P2_Lap.text = txt_P2_Lap.text + "/" + lapNumber;
				}
			}
			return;
		}
		for (int i = 0; i < car.Count; i++)
		{
			if (car[i] != null && car[i].name == other.name && !raceFinished[i])
			{
				raceFinished[i] = true;
				carTime[i] = (float)TimeSpan.FromTicks(Timer.Ticks).TotalSeconds;
				carController[i].raceIsFinished = true;
				if (i == 0 && player2IsManageByCPU)
				{
					SaveBestLap();
					gameManager.RaceIsFinished(carPosition[0]);
				}
				else if ((i == 0 || i == 1) && !player2IsManageByCPU && carController[0] != null && carController[0].raceIsFinished && carController[1] != null && carController[1].raceIsFinished)
				{
					gameManager.RaceIsFinished();
				}
				break;
			}
		}
	}

	public void displayLap(CarPathFollow carAddLap)
	{
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = GizmoColor;
		Matrix4x4 matrix4x2 = (Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, base.transform.localScale));
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}
}
