using Core.Audio;
using UnityEngine;

namespace SCJogos.Audio
{
	public class AudioPlayer : MonoBehaviour
	{
		[SerializeField]
		private MusicData[] playlist;

		private bool isPlaylist;

		public static AudioPlayer Instance { get; private set; }

		private void Start()
		{
			Instance = this;
			if (playlist.Length > 0)
			{
				StartPlaylist();
			}
		}

		public void StartPlaylist()
		{
			if (!isPlaylist)
			{
				isPlaylist = true;
				AudioManager.Instance.ClearPlaylist();
				for (int i = 0; i < playlist.Length; i++)
				{
					AudioManager.Instance.AddMusicToPlayList(playlist[i]);
				}
			}
		}

		public void StopPlaylist()
		{
			AudioManager.Instance.ClearPlaylist();
		}
	}
}
