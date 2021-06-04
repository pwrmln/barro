using SCJogos.Database;
using SCJogos.Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class MenuPrivacySettings : MonoBehaviour
{
	[SerializeField]
	private Toggle toogleAchievements;

	[SerializeField]
	private Toggle toogleLeaderboard;

	[SerializeField]
	private Button buttonDeleteAll;

	private void Start()
	{
		toogleAchievements.isOn = BARDataManager.Instance.ConfigData.Achievements;
		toogleLeaderboard.isOn = BARDataManager.Instance.ConfigData.UseLeaderboard;
		buttonDeleteAll.onClick.AddListener(OnDeleteAllClick);
	}

	private void OnDeleteAllClick()
	{
		BARSteamManager.Instance.DeleteAllData();
	}
}
