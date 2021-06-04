using System.Collections;
using System.Collections.Generic;
using SCJogos.Database;
using SCJogos.Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
	public static Game_Manager instance;

	[SerializeField]
	private TrackData trackData;

	public bool b_Pause;

	public bool b_UseCountdown = true;

	public Countdown countdown;

	public LapCounter lapcounter;

	public List<CarController> list_Cars = new List<CarController>();

	public Cam_Follow cam_P1;

	public Cam_Follow cam_P2;

	public GameObject obj_LineSplitScreen_Part_01;

	public GameObject obj_LineSplitScreen_Part_02;

	public InventoryGlobalPrefs inventoryItemList;

	public InventoryCar inventoryItemCar;

	public Menu_Manager canvas_MainMenu;

	public EventSystem eventSystem;

	public StandaloneInputModule inputModule;

	public GameObject buttonRestartGame_FirstSelectedGameObject;

	public GameObject buttonValidateLetter_FirstSelectedGameObject;

	public GameObject Player1Position;

	public GameObject Player1PositionPart2;

	public GameObject Player2Position;

	public GameObject Player2PositionPart2;

	public GameObject Player1LapCounter;

	public GameObject Player2LapCounter;

	public float TimeBetweenCongratulationAndResultBoard = 2f;

	public LeaderboardSystem leaderboard;

	public GameObject canvasMobileInputs;

	public bool b_TuningZone;

	[SerializeField]
	private bool lights;

	[SerializeField]
	private string unlockAchievement;

	public TrackData TrackData
	{
		get
		{
			return trackData;
		}
		set
		{
			trackData = value;
		}
	}

	public bool Lights
	{
		get
		{
			return lights;
		}
	}

	private void Start()
	{
		Application.targetFrameRate = 60;
		BARDataManager.Instance.BARConfig.WeAreOnTrack = 1;
		if ((bool)canvas_MainMenu && !inventoryItemList.inventoryItem[0].b_TestMode && !b_TuningZone)
		{
			canvas_MainMenu.GoToOtherPageWithHisNumber(7);
		}
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		if (countdown == null && (bool)base.gameObject.GetComponent<Countdown>())
		{
			countdown = GetComponent<Countdown>();
		}
		if (lapcounter == null)
		{
			GameObject gameObject = GameObject.Find("StartLine_lapCounter");
			if ((bool)gameObject)
			{
				lapcounter = gameObject.GetComponent<LapCounter>();
			}
		}
		if (inventoryItemList.inventoryItem[0].b_TestMode && inventoryItemList != null)
		{
			GameObject canvas_TestMode = inventoryItemList.inventoryItem[0].Canvas_TestMode;
			if ((bool)canvas_TestMode)
			{
				GameObject gameObject2 = Object.Instantiate(canvas_TestMode);
				gameObject2.name = "Canvas_TestMode";
			}
		}
		if ((bool)inventoryItemCar)
		{
			b_UseCountdown = inventoryItemCar.b_Countdown;
		}
		if (!inventoryItemList.inventoryItem[0].b_TestMode && !b_TuningZone)
		{
			InitGame();
			return;
		}
		CreateCarList();
		if ((bool)countdown && b_UseCountdown)
		{
			countdown.b_ActivateCountdown = true;
		}
		else
		{
			for (int i = 0; i < list_Cars.Count; i++)
			{
				if ((bool)list_Cars[i])
				{
					list_Cars[i].b_CountdownActivate = false;
				}
			}
		}
		if (b_TuningZone && list_Cars[0] != null)
		{
			list_Cars[0].b_InitInputWhenGameStart = false;
			list_Cars[0].b_CountdownActivate = false;
		}
		if (list_Cars[1] != null && (bool)cam_P2 && !list_Cars[1].gameObject.GetComponent<CarAI>().enabled)
		{
			cam_P2.gameObject.SetActive(true);
			cam_P2.InitCamera(list_Cars[1], false);
		}
		if (list_Cars[0] != null && (bool)cam_P1)
		{
			if (list_Cars[1] != null && !list_Cars[1].gameObject.GetComponent<CarAI>().enabled)
			{
				cam_P1.InitCamera(list_Cars[0], true);
				if ((bool)obj_LineSplitScreen_Part_01)
				{
					obj_LineSplitScreen_Part_01.SetActive(true);
				}
				if ((bool)obj_LineSplitScreen_Part_02)
				{
					obj_LineSplitScreen_Part_02.SetActive(true);
				}
			}
			else
			{
				cam_P1.InitCamera(list_Cars[0], false);
				if ((bool)obj_LineSplitScreen_Part_01)
				{
					obj_LineSplitScreen_Part_01.SetActive(false);
				}
				if ((bool)obj_LineSplitScreen_Part_02)
				{
					obj_LineSplitScreen_Part_02.SetActive(false);
				}
			}
		}
		if ((bool)canvas_MainMenu && !inventoryItemList.inventoryItem[0].b_TestMode && !b_TuningZone)
		{
			canvas_MainMenu.GoToOtherPageWithHisNumber(5);
		}
	}

	public void RaceStart()
	{
		for (int i = 0; i < list_Cars.Count; i++)
		{
			if ((bool)list_Cars[i])
			{
				list_Cars[i].StartRace();
				list_Cars[i].b_CountdownActivate = false;
			}
		}
		if ((bool)lapcounter)
		{
			lapcounter.startTimer = true;
		}
	}

	public void RaceIsFinished(int pos = 0)
	{
		if ((bool)canvasMobileInputs)
		{
			canvasMobileInputs.SetActive(false);
		}
		if (BARDataManager.Instance.BARConfig.GameMode == GameModeType.Arcade)
		{
			StartCoroutine(WinProcessARcade(pos));
		}
		else
		{
			StartCoroutine(WinProcessTimeTrial());
		}
	}

	public void FinishRace()
	{
		if (BARDataManager.Instance.BARConfig.GameMode == GameModeType.TimeTrial)
		{
			lapcounter.SaveBestLap();
			StartCoroutine(WinProcessTimeTrial());
		}
		else
		{
			canvas_MainMenu.GoToOtherPageWithHisNumber(7);
			canvas_MainMenu.GetComponent<LoadAScene>().LoadASceneWithThisNumber(1);
		}
	}

	private IEnumerator WinProcessARcade(int pos)
	{
		if ((bool)canvas_MainMenu)
		{
			canvas_MainMenu.GoToOtherPageWithHisNumber(10);
		}
		yield return new WaitForSeconds(TimeBetweenCongratulationAndResultBoard);
		if ((bool)canvas_MainMenu)
		{
			canvas_MainMenu.GoToOtherPageWithHisNumber(6);
		}
		buttonRestartGame_FirstSelectedGameObject.GetComponent<Button>().Select();
		if (!string.IsNullOrEmpty(unlockAchievement) && pos == 1)
		{
			BARSteamManager.Instance.SetAchievement(unlockAchievement);
		}
	}

	private IEnumerator WinProcessTimeTrial()
	{
		if ((bool)canvas_MainMenu)
		{
			canvas_MainMenu.GoToOtherPageWithHisNumber(10);
		}
		yield return new WaitForSeconds(TimeBetweenCongratulationAndResultBoard);
		if ((bool)canvas_MainMenu)
		{
			canvas_MainMenu.GoToOtherPageWithHisNumber(9);
		}
		if (lapcounter.BestLap < float.MaxValue && (bool)leaderboard)
		{
			leaderboard.InitPlayerScore();
		}
		buttonValidateLetter_FirstSelectedGameObject.GetComponent<Button>().Select();
	}

	private void CreateCarList()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Car");
		for (int i = 0; i < 4; i++)
		{
			list_Cars.Add(null);
		}
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if ((bool)gameObject.GetComponent<CarController>())
			{
				if (inventoryItemCar.b_mobile)
				{
					gameObject.GetComponent<CarController>().offsetSpeedForMobile = inventoryItemCar.mobileMaxSpeedOffset;
					gameObject.GetComponent<CarController>().offsetRotationForMobile = inventoryItemCar.mobileWheelStearingOffsetReactivity;
				}
				list_Cars[gameObject.GetComponent<CarController>().playerNumber - 1] = gameObject.GetComponent<CarController>();
			}
		}
		GameObject[] array3 = GameObject.FindGameObjectsWithTag("TriggerAI");
		GameObject[] array4 = array3;
		foreach (GameObject gameObject2 in array4)
		{
			gameObject2.GetComponent<TriggersAI>().InitTriggersAI(list_Cars);
		}
	}

	private void InitGame()
	{
		StartCoroutine("I_InitGame");
	}

	private IEnumerator I_InitGame()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Car");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if ((bool)gameObject.GetComponent<CarController>())
			{
				gameObject.gameObject.SetActive(false);
				Object.Destroy(gameObject);
			}
		}
		if (BARDataManager.Instance.BARConfig.GameMode == GameModeType.Arcade)
		{
			for (int j = 0; j < 4; j++)
			{
				GameObject gameObject2 = Object.Instantiate(inventoryItemCar.inventoryItem[j].Cars[BARDataManager.Instance.BARConfig.PlayerCarSelected[j]]);
				gameObject2.name = gameObject2.name.Replace("(Clone)", string.Empty);
				GameObject gameObject3 = GameObject.Find("Start_Position_0" + (j + 1));
				if (!gameObject3)
				{
					continue;
				}
				gameObject2.GetComponent<CarController>().playerNumber = j + 1;
				if (j == 0)
				{
					gameObject2.GetComponent<CarAI>().enabled = false;
				}
				if (j == 1 && BARDataManager.Instance.BARConfig.PlayersNumber == 2)
				{
					gameObject2.GetComponent<CarAI>().enabled = false;
					if ((bool)Player2Position)
					{
						Player2Position.SetActive(true);
					}
					if ((bool)Player2PositionPart2)
					{
						Player2PositionPart2.SetActive(true);
					}
					if ((bool)Player2LapCounter)
					{
						Player2LapCounter.SetActive(true);
					}
				}
				if (j == 1 && BARDataManager.Instance.BARConfig.PlayersNumber == 1)
				{
					gameObject2.GetComponent<CarAI>().enabled = true;
					if ((bool)Player2Position)
					{
						Player2Position.SetActive(false);
					}
					if ((bool)Player2PositionPart2)
					{
						Player2PositionPart2.SetActive(false);
					}
					if ((bool)Player2LapCounter)
					{
						Player2LapCounter.SetActive(false);
					}
				}
				if (j > 1)
				{
					gameObject2.GetComponent<CarAI>().enabled = true;
				}
				if ((bool)gameObject3)
				{
					RaycastHit hitInfo;
					if (Physics.Raycast(gameObject3.transform.position + Vector3.up, Vector3.down, out hitInfo, 100f))
					{
						gameObject2.transform.position = new Vector3(gameObject3.transform.position.x, hitInfo.point.y + 0.13f, gameObject3.transform.position.z);
					}
					else
					{
						gameObject2.transform.position = new Vector3(gameObject3.transform.position.x, gameObject3.transform.position.y + 0.15f, gameObject3.transform.position.z);
					}
					gameObject2.transform.eulerAngles = gameObject3.transform.eulerAngles;
				}
				while (!gameObject2)
				{
				}
				gameObject2.GetComponent<Rigidbody>().isKinematic = true;
			}
		}
		else if (BARDataManager.Instance.BARConfig.GameMode == GameModeType.TimeTrial)
		{
			GameObject gameObject4 = Object.Instantiate(inventoryItemCar.inventoryItem[0].Cars[BARDataManager.Instance.BARConfig.PlayerCarSelected[0]]);
			gameObject4.name = gameObject4.name.Replace("(Clone)", string.Empty);
			GameObject gameObject5 = GameObject.Find("Start_Position_04");
			gameObject4.GetComponent<CarController>().playerNumber = 1;
			gameObject4.GetComponent<CarAI>().enabled = false;
			if ((bool)gameObject5)
			{
				RaycastHit hitInfo2;
				if (Physics.Raycast(gameObject5.transform.position + Vector3.up, Vector3.down, out hitInfo2, 100f))
				{
					gameObject4.transform.position = new Vector3(gameObject5.transform.position.x, hitInfo2.point.y + 0.13f, gameObject5.transform.position.z);
				}
				else
				{
					gameObject4.transform.position = new Vector3(gameObject5.transform.position.x, gameObject5.transform.position.y + 0.15f, gameObject5.transform.position.z);
				}
				gameObject4.transform.eulerAngles = gameObject5.transform.eulerAngles;
			}
			while (!gameObject4)
			{
			}
			gameObject4.GetComponent<Rigidbody>().isKinematic = true;
			if ((bool)Player1Position)
			{
				Player1Position.SetActive(false);
			}
			if ((bool)Player1PositionPart2)
			{
				Player1PositionPart2.SetActive(false);
			}
			if ((bool)Player2Position)
			{
				Player2Position.SetActive(false);
			}
			if ((bool)Player2PositionPart2)
			{
				Player2PositionPart2.SetActive(false);
			}
			if ((bool)Player2LapCounter)
			{
				Player2LapCounter.SetActive(false);
			}
		}
		CreateCarList();
		if ((bool)lapcounter)
		{
			lapcounter.InitCar();
		}
		if ((bool)countdown && b_UseCountdown)
		{
			countdown.b_ActivateCountdown = true;
		}
		if (list_Cars[1] != null && (bool)cam_P2 && !list_Cars[1].gameObject.GetComponent<CarAI>().enabled)
		{
			cam_P2.gameObject.SetActive(true);
			cam_P2.InitCamera(list_Cars[1], false);
		}
		if (list_Cars[0] != null && (bool)cam_P1)
		{
			if (list_Cars[1] != null && !list_Cars[1].gameObject.GetComponent<CarAI>().enabled)
			{
				cam_P1.InitCamera(list_Cars[0], true);
				if ((bool)obj_LineSplitScreen_Part_01)
				{
					obj_LineSplitScreen_Part_01.SetActive(true);
				}
				if ((bool)obj_LineSplitScreen_Part_02)
				{
					obj_LineSplitScreen_Part_02.SetActive(true);
				}
			}
			else
			{
				cam_P1.InitCamera(list_Cars[0], false);
				if ((bool)obj_LineSplitScreen_Part_01)
				{
					obj_LineSplitScreen_Part_01.SetActive(false);
				}
				if ((bool)obj_LineSplitScreen_Part_02)
				{
					obj_LineSplitScreen_Part_02.SetActive(false);
				}
			}
		}
		if ((bool)canvas_MainMenu && !inventoryItemList.inventoryItem[0].b_TestMode && !b_TuningZone)
		{
			canvas_MainMenu.GoToOtherPageWithHisNumber(5);
		}
		return null;
	}
}
