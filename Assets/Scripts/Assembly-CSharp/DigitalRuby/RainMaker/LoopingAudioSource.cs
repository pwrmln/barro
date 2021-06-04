using Core.Audio;
using UnityEngine;

namespace DigitalRuby.RainMaker
{
	public class LoopingAudioSource
	{
		public AudioSource AudioSource { get; private set; }

		public float TargetVolume { get; private set; }

		public LoopingAudioSource(MonoBehaviour script, AudioClip clip)
		{
			AudioSource = script.gameObject.AddComponent<AudioSource>();
			AudioSource.outputAudioMixerGroup = AudioManager.Instance.AudioMixerSFX;
			AudioSource.loop = true;
			AudioSource.clip = clip;
			AudioSource.playOnAwake = false;
			AudioSource.volume = 0f;
			AudioSource.Stop();
			TargetVolume = 1f;
		}

		public void Play(float targetVolume)
		{
			if (!AudioSource.isPlaying)
			{
				AudioSource.volume = 0f;
				AudioSource.Play();
			}
			TargetVolume = targetVolume;
		}

		public void Stop()
		{
			TargetVolume = 0f;
		}

		public void Update()
		{
			if (AudioSource.isPlaying)
			{
				float num = Mathf.Lerp(AudioSource.volume, TargetVolume, Time.deltaTime);
				AudioSource.volume = num;
				if (num == 0f)
				{
					AudioSource.Stop();
				}
			}
		}
	}
}
