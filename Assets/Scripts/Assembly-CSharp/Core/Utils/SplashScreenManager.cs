using UnityEngine;
using UnityEngine.Video;

namespace Core.Utils
{
	public class SplashScreenManager : MonoBehaviour
	{
		[SerializeField]
		private VideoPlayer videoPlayer;

		private bool done;

		private void Start()
		{
			done = false;
		}

		private void Update()
		{
			if (!videoPlayer.isPlaying && !done)
			{
				done = true;
				PrivacyManager.Instance.Show();
			}
		}
	}
}
