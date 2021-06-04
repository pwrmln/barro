using SCJogos.Database;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuInit : MonoBehaviour
{
	public Menu_Manager menuManager;

	public EventSystem eventSystem;

	public GameObject Case01_FirstSelectedGameObject;

	public GameObject Case02_FirstSelectedGameObject;

	public GameObject difficultyObject;

	public GameObject lapsObject;

	private void Start()
	{
		if (BARDataManager.Instance.BARConfig.WeAreOnTrack == 1)
		{
			menuManager.GoToOtherPageWithHisNumber(2);
			eventSystem.SetSelectedGameObject(Case01_FirstSelectedGameObject);
		}
		else
		{
			menuManager.GoToOtherPageWithHisNumber(0);
			eventSystem.SetSelectedGameObject(Case02_FirstSelectedGameObject);
		}
		BARDataManager.Instance.BARConfig.WeAreOnTrack = 0;
		if (BARDataManager.Instance.BARConfig.GameMode == GameModeType.TimeTrial)
		{
			if ((bool)difficultyObject)
			{
				difficultyObject.SetActive(false);
			}
			if ((bool)lapsObject)
			{
				lapsObject.SetActive(false);
			}
		}
		else
		{
			if ((bool)difficultyObject)
			{
				difficultyObject.SetActive(true);
			}
			if ((bool)lapsObject)
			{
				lapsObject.SetActive(true);
			}
		}
	}
}
