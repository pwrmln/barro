using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.Utils
{
	public class LoadingManager : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup panelLoading;

		[SerializeField]
		private Image imageProgress;

		public static LoadingManager Instance { get; private set; }

		private void Awake()
		{
			Instance = this;
			ActivatePanel(false);
			imageProgress.fillAmount = 0f;
		}

		private void ActivatePanel(bool active)
		{
			panelLoading.alpha = (active ? 1 : 0);
			panelLoading.blocksRaycasts = active;
		}

		public void LoadScene(string scene, bool unlockCursor = true)
		{
			ActivatePanel(true);
			StartCoroutine(LoadNextScene(scene, unlockCursor));
		}

		private IEnumerator LoadNextScene(string scene, bool unlockCursor)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			AsyncOperation asc = SceneManager.LoadSceneAsync(scene);
			while (!asc.isDone)
			{
				imageProgress.fillAmount = asc.progress;
				if (unlockCursor && asc.progress >= 0.9f)
				{
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;
				}
				yield return null;
			}
		}
	}
}
