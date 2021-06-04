using SCJogos.Database;
using UnityEngine;

public class MainMenuManageChooses : MonoBehaviour
{
	public void Choose_Solo_Or_Multi(string newMode)
	{
		switch (newMode)
		{
		case "Solo":
			BARDataManager.Instance.BARConfig.PlayersNumber = 1;
			break;
		case "Multi":
			BARDataManager.Instance.BARConfig.PlayersNumber = 2;
			break;
		default:
			BARDataManager.Instance.BARConfig.PlayersNumber = 1;
			break;
		}
		BARDataManager.Instance.SaveBARConfig();
	}

	public void Choose_GameMode(string newMode)
	{
		switch (newMode)
		{
		case "Arcade":
			BARDataManager.Instance.BARConfig.GameMode = GameModeType.Arcade;
			break;
		case "TimeTrial":
			BARDataManager.Instance.BARConfig.GameMode = GameModeType.TimeTrial;
			break;
		default:
			BARDataManager.Instance.BARConfig.GameMode = GameModeType.Arcade;
			break;
		}
		BARDataManager.Instance.SaveBARConfig();
	}
}
