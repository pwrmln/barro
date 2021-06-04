using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckForInputs : MonoBehaviour
{
	[SerializeField]
	private EventSystem eventSystem;

	[SerializeField]
	private Menu_Manager menu_Manager;

	[SerializeField]
	private GameObject obj_Result;

	[SerializeField]
	private GameObject obj_SaveYourScore;

	[SerializeField]
	private Button buttonResume;

	private PauseManager pauseManager;

	private float nextPause;

	private float startDelay;

	private bool paused;

	private void Start()
	{
		pauseManager = Object.FindObjectOfType<PauseManager>();
		paused = false;
		if ((bool)buttonResume)
		{
			buttonResume.onClick.AddListener(OnResumeClick);
		}
		startDelay = Time.time + 3f;
	}

	private void OnResumeClick()
	{
		paused = false;
		nextPause = Time.time + 1f;
	}

	private void Update()
	{
		if (!paused && (bool)obj_SaveYourScore && (bool)obj_Result && !obj_Result.activeInHierarchy && !obj_SaveYourScore.activeInHierarchy && Input.GetButtonDown("Pause") && Time.time > nextPause && Time.time > startDelay)
		{
			menu_Manager.GoToOtherPageWithHisNumber(0, buttonResume);
			buttonResume.Select();
			paused = true;
			nextPause = Time.time + 0.5f;
			pauseManager.PauseGame();
		}
		else if (paused && Input.GetButtonDown("Pause") && Time.time > nextPause)
		{
			OnResumeClick();
			buttonResume.onClick.Invoke();
		}
	}
}
