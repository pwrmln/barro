using System.Collections;
using SCJogos.Database;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{
	private MainMenu obj_MainMenu;

	public GameObject Loading_Screen;

	public Text LoadingText;

	public Text TextTableName;

	public Text TextDifficulty;

	public Menu_Manager menuManager;

	private LeaderboardSystem leaderBoardSystem;

	private void Start()
	{
		Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
		leaderBoardSystem = Object.FindObjectOfType<LeaderboardSystem>();
		if (!obj_MainMenu)
		{
			obj_MainMenu = Object.FindObjectOfType<MainMenu>();
		}
		if ((bool)TextDifficulty)
		{
			TextDifficulty.text = BARDataManager.Instance.BARConfig.Difficulty.ToString();
		}
	}

	public void IncLaps()
	{
		obj_MainMenu.AddLaps(1);
	}

	public void DecLaps()
	{
		obj_MainMenu.AddLaps(-1);
	}

	public void LastScene()
	{
		if (!obj_MainMenu.NextScene(-1))
		{
			LastScene();
		}
	}

	public void NextScene()
	{
		if (!obj_MainMenu.NextScene(1))
		{
			NextScene();
		}
	}

	public void LoadASceneWhenPlayIsPressed_MainMenu()
	{
		StartCoroutine(AsynchronousLoad());
	}

	public void chooseDifficulty()
	{
		switch (BARDataManager.Instance.BARConfig.Difficulty)
		{
		case DifficultyType.Easy:
			BARDataManager.Instance.BARConfig.Difficulty = DifficultyType.Medium;
			break;
		case DifficultyType.Medium:
			BARDataManager.Instance.BARConfig.Difficulty = DifficultyType.Expert;
			break;
		case DifficultyType.Expert:
			BARDataManager.Instance.BARConfig.Difficulty = DifficultyType.Easy;
			break;
		}
		if ((bool)TextDifficulty)
		{
			TextDifficulty.text = BARDataManager.Instance.BARConfig.Difficulty.ToString();
		}
		BARDataManager.Instance.SaveBARConfig();
	}

	private IEnumerator AsynchronousLoad()
	{
		if (menuManager != null)
		{
			menuManager.GoToOtherPageWithHisNumber(10);
		}
		yield return new WaitForEndOfFrame();
		BARDataManager.Instance.SaveBARConfig();
		AsyncOperation a = SceneManager.LoadSceneAsync(obj_MainMenu.GetCurrentTrackSceneName());
		a.allowSceneActivation = false;
		while (!a.isDone)
		{
			float progress = Mathf.Clamp01(a.progress / 0.9f);
			if ((bool)LoadingText)
			{
				LoadingText.text = "Loading " + (progress * 100f).ToString("n0") + "%";
			}
			if (a.progress == 0.9f)
			{
				a.allowSceneActivation = true;
			}
			yield return null;
		}
	}

	public void F_QuitGame()
	{
		Application.Quit();
	}

	public void UIButtonUpdateLeaderboardSystem()
	{
		StartCoroutine(leaderBoardSystem.UpdateLeaderboard());
	}
}
