using Core.Database;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Utils
{
	public class PrivacyManager : MonoBehaviour
	{
		[SerializeField]
		private string nextScene = "MainMenu";

		[SerializeField]
		private CanvasGroup panelPrivacy;

		[SerializeField]
		private Button buttonYes;

		[SerializeField]
		private Button buttonNo;

		[SerializeField]
		private DataManager dataManager;

		public static PrivacyManager Instance { get; private set; }

		private void Start()
		{
			Instance = this;
			ActivatePanel(false);
			buttonYes.onClick.AddListener(OnYesClick);
			buttonNo.onClick.AddListener(OnNoClick);
		}

		private void OnYesClick()
		{
			OnClick(true);
		}

		private void OnNoClick()
		{
			OnClick(false);
		}

		private void OnClick(bool activate)
		{
			dataManager.ConfigData.Achievements = activate;
			dataManager.ConfigData.InitialAchievementMessage = true;
			dataManager.SaveConfig();
			LoadingManager.Instance.LoadScene(nextScene);
		}

		private void ActivatePanel(bool active)
		{
			panelPrivacy.alpha = (active ? 1 : 0);
			panelPrivacy.interactable = active;
			panelPrivacy.blocksRaycasts = active;
		}

		public void Show()
		{
			if (dataManager.ConfigData.InitialAchievementMessage)
			{
				LoadingManager.Instance.LoadScene(nextScene);
			}
			else
			{
				ActivatePanel(true);
			}
		}
	}
}
