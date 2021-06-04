using Core.Database;
using UnityEngine;
using UnityEngine.Audio;

namespace Core.Audio
{
	public class AudioManager : MonoBehaviour
	{
		[SerializeField]
		private AudioMixer audioMixer;

		[SerializeField]
		private AudioMixerGroup audioMixerMain;

		[SerializeField]
		private AudioMixerGroup audioMixerMusic;

		[SerializeField]
		private AudioMixerGroup audioMixerSFX;

		[SerializeField]
		private DataManager dataManager;

		private AudioSource[] audioSourceSFX;

		private MusicData[] playlist;

		private AudioSource audioSourceMusic;

		private int currentMusic;

		private int repeatAmount;

		public static AudioManager Instance { get; private set; }

		public AudioMixerGroup AudioMixerSFX
		{
			get
			{
				return audioMixerSFX;
			}
		}

		private void Start()
		{
			Instance = this;
			audioSourceSFX = new AudioSource[32];
			playlist = new MusicData[32];
		}

		private void Update()
		{
			PlayPlaylist();
		}

		private float AdjustVolume(float volume)
		{
			return volume * 80f - 80f;
		}

		private void SetVolume(string volumeName, float volume)
		{
			audioMixer.SetFloat(volumeName, AdjustVolume(volume));
		}

		public void RefreshVolume()
		{
			SetVolume("MainVolume", dataManager.ConfigData.MainVolume);
			SetVolume("MusicVolume", dataManager.ConfigData.MusicVolume);
			SetVolume("SFXVolume", dataManager.ConfigData.SFXVolume);
		}

		public void AddMusicToPlayList(MusicData musicData)
		{
			for (int i = 0; i < playlist.Length; i++)
			{
				if (playlist[i] == null)
				{
					playlist[i] = musicData;
					break;
				}
			}
		}

		public void ClearPlaylist()
		{
			if (audioSourceMusic != null && audioSourceMusic.isPlaying)
			{
				audioSourceMusic.Stop();
			}
			currentMusic = 0;
			for (int i = 0; i < playlist.Length; i++)
			{
				playlist[i] = null;
			}
		}

		private void PlayPlaylist()
		{
			if (!(playlist[0] != null))
			{
				return;
			}
			SetAudioSourceMusic();
			if (audioSourceMusic.isPlaying)
			{
				return;
			}
			RefreshVolume();
			audioSourceMusic.clip = playlist[currentMusic].AudioClip;
			audioSourceMusic.Play();
			if (repeatAmount <= 0)
			{
				currentMusic++;
				if (playlist[currentMusic] == null)
				{
					currentMusic = 0;
				}
				repeatAmount = playlist[currentMusic].RepeatAmount;
			}
			repeatAmount--;
		}

		private void SetAudioSourceMusic()
		{
			if (audioSourceMusic == null)
			{
				audioSourceMusic = base.gameObject.AddComponent<AudioSource>();
				audioSourceMusic.loop = false;
				audioSourceMusic.playOnAwake = false;
				audioSourceMusic.spatialBlend = 0f;
				audioSourceMusic.outputAudioMixerGroup = audioMixerMusic;
			}
		}

		private void PlaySFX(SFXData sfxData, Vector3? position, float volume, bool useSameAudioSource, bool searchClip)
		{
			AudioSource audioSource = null;
			if (useSameAudioSource)
			{
				if (audioSourceSFX[0] != null)
				{
					audioSource = audioSourceSFX[0];
				}
			}
			else if (searchClip)
			{
				for (int i = 0; i < audioSourceSFX.Length; i++)
				{
					if (audioSourceSFX[i] != null && audioSourceSFX[i].clip == sfxData.AudioClip)
					{
						audioSource = audioSourceSFX[i];
						break;
					}
				}
			}
			else
			{
				for (int j = 0; j < audioSourceSFX.Length; j++)
				{
					if (audioSourceSFX[j] != null && !audioSourceSFX[j].isPlaying)
					{
						audioSource = audioSourceSFX[j];
						break;
					}
				}
			}
			if (audioSource == null)
			{
				for (int k = 0; k < audioSourceSFX.Length; k++)
				{
					if (audioSourceSFX[k] == null)
					{
						GameObject gameObject = new GameObject(k.ToString("D3"));
						gameObject.transform.parent = base.transform;
						audioSource = gameObject.AddComponent<AudioSource>();
						audioSource.playOnAwake = false;
						audioSource.loop = false;
						audioSource.outputAudioMixerGroup = audioMixerSFX;
						audioSourceSFX[k] = audioSource;
						break;
					}
				}
			}
			audioSource.volume = volume;
			audioSource.spatialBlend = (position.HasValue ? 1 : 0);
			if (position.HasValue)
			{
				audioSource.gameObject.transform.position = position.Value;
			}
			audioSource.clip = sfxData.AudioClip;
			audioSource.Play();
		}

		public void PlaySFXUseSameClip(SFXData sfxData)
		{
			PlaySFX(sfxData, null, 1f, false, true);
		}

		public void PlaySFXUseSameClip(SFXData sfxData, float volume)
		{
			PlaySFX(sfxData, null, volume, false, true);
		}

		public void PlaySFX(SFXData sfxData, bool useSameAudioSource = false)
		{
			PlaySFX(sfxData, null, 1f, useSameAudioSource, false);
		}

		public void PlaySFX(SFXData sfxData, float volume, bool useSameAudioSource = false)
		{
			PlaySFX(sfxData, null, volume, useSameAudioSource, false);
		}

		public void PlaySFX(SFXData sfxData, Vector3 position, bool useSameAudioSource = false)
		{
			PlaySFX(sfxData, position, 1f, useSameAudioSource, false);
		}
	}
}
