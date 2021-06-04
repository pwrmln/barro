using SCJogos.Database;
using UnityEngine;
using UnityEngine.UI;

public class TrackSelection : MonoBehaviour
{
	public GameObject J2;

	public GameObject J3;

	public GameObject J4;

	public CarSelection carSelection;

	public GameObject button_Trial;

	public Text TextPlayer2;

	public GameObject Buttons_Choose_Car;

	public void BackFromTrackSelection()
	{
		if (BARDataManager.Instance.BARConfig.GameMode == GameModeType.Arcade)
		{
			if ((bool)J2)
			{
				J2.SetActive(true);
			}
			if ((bool)J3)
			{
				J3.SetActive(true);
			}
			if ((bool)J4)
			{
				J4.SetActive(true);
			}
		}
		else if (BARDataManager.Instance.BARConfig.GameMode == GameModeType.TimeTrial)
		{
			if ((bool)J2)
			{
				J2.SetActive(false);
			}
			if ((bool)J3)
			{
				J3.SetActive(false);
			}
			if ((bool)J4)
			{
				J4.SetActive(false);
			}
		}
		if (BARDataManager.Instance.BARConfig.PlayersNumber == 1)
		{
			if ((bool)button_Trial)
			{
				button_Trial.SetActive(true);
			}
			if ((bool)TextPlayer2)
			{
				TextPlayer2.text = "CPU";
			}
			if ((bool)Buttons_Choose_Car)
			{
				Buttons_Choose_Car.SetActive(false);
			}
		}
		else if (BARDataManager.Instance.BARConfig.PlayersNumber == 2)
		{
			if ((bool)button_Trial)
			{
				button_Trial.SetActive(false);
			}
			if ((bool)TextPlayer2)
			{
				TextPlayer2.text = "P2";
			}
			if ((bool)Buttons_Choose_Car)
			{
				Buttons_Choose_Car.SetActive(true);
			}
		}
		if ((bool)carSelection)
		{
			carSelection.initCarSelection(0);
		}
	}
}
