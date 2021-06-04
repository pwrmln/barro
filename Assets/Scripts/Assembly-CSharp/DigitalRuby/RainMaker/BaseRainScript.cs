using UnityEngine;

namespace DigitalRuby.RainMaker
{
	public class BaseRainScript : MonoBehaviour
	{
		[Tooltip("Camera the rain should hover over, defaults to main camera")]
		public Camera Camera;

		[Tooltip("Whether rain should follow the camera. If false, rain must be moved manually and will not follow the camera.")]
		public bool FollowCamera = true;

		[Tooltip("Light rain looping clip")]
		public AudioClip RainSoundLight;

		[Tooltip("Medium rain looping clip")]
		public AudioClip RainSoundMedium;

		[Tooltip("Heavy rain looping clip")]
		public AudioClip RainSoundHeavy;

		[Tooltip("Intensity of rain (0-1)")]
		[Range(0f, 1f)]
		public float RainIntensity;

		[Tooltip("Rain particle system")]
		public ParticleSystem RainFallParticleSystem;

		[Tooltip("Particles system for when rain hits something")]
		public ParticleSystem RainExplosionParticleSystem;

		[Tooltip("Particle system to use for rain mist")]
		public ParticleSystem RainMistParticleSystem;

		[Tooltip("The threshold for intensity (0 - 1) at which mist starts to appear")]
		[Range(0f, 1f)]
		public float RainMistThreshold = 0.5f;

		[Tooltip("Wind looping clip")]
		public AudioClip WindSound;

		[Tooltip("Wind sound volume modifier, use this to lower your sound if it's too loud.")]
		public float WindSoundVolumeModifier = 0.5f;

		[Tooltip("Wind zone that will affect and follow the rain")]
		public WindZone WindZone;

		[Tooltip("X = minimum wind speed. Y = maximum wind speed. Z = sound multiplier. Wind speed is divided by Z to get sound multiplier value. Set Z to lower than Y to increase wind sound volume, or higher to decrease wind sound volume.")]
		public Vector3 WindSpeedRange = new Vector3(50f, 500f, 500f);

		[Tooltip("How often the wind speed and direction changes (minimum and maximum change interval in seconds)")]
		public Vector2 WindChangeInterval = new Vector2(5f, 30f);

		[Tooltip("Whether wind should be enabled.")]
		public bool EnableWind = true;

		protected LoopingAudioSource audioSourceRainLight;

		protected LoopingAudioSource audioSourceRainMedium;

		protected LoopingAudioSource audioSourceRainHeavy;

		protected LoopingAudioSource audioSourceRainCurrent;

		protected LoopingAudioSource audioSourceWind;

		protected Material rainMaterial;

		protected Material rainExplosionMaterial;

		protected Material rainMistMaterial;

		private float lastRainIntensityValue = -1f;

		private float nextWindTime;

		protected virtual bool UseRainMistSoftParticles
		{
			get
			{
				return true;
			}
		}

		private void UpdateWind()
		{
			if (EnableWind && WindZone != null && WindSpeedRange.y > 1f)
			{
				WindZone.gameObject.SetActive(true);
				if (FollowCamera)
				{
					WindZone.transform.position = Camera.transform.position;
				}
				if (!Camera.orthographic)
				{
					WindZone.transform.Translate(0f, WindZone.radius, 0f);
				}
				if (nextWindTime < Time.time)
				{
					WindZone.windMain = Random.Range(WindSpeedRange.x, WindSpeedRange.y);
					WindZone.windTurbulence = Random.Range(WindSpeedRange.x, WindSpeedRange.y);
					if (Camera.orthographic)
					{
						int num = Random.Range(0, 2);
						WindZone.transform.rotation = Quaternion.Euler(0f, (num != 0) ? (-90f) : 90f, 0f);
					}
					else
					{
						WindZone.transform.rotation = Quaternion.Euler(Random.Range(-30f, 30f), Random.Range(0f, 360f), 0f);
					}
					nextWindTime = Time.time + Random.Range(WindChangeInterval.x, WindChangeInterval.y);
					audioSourceWind.Play(WindZone.windMain / WindSpeedRange.z * WindSoundVolumeModifier);
				}
			}
			else
			{
				if (WindZone != null)
				{
					WindZone.gameObject.SetActive(false);
				}
				audioSourceWind.Stop();
			}
			audioSourceWind.Update();
		}

		private void CheckForRainChange()
		{
			if (lastRainIntensityValue == RainIntensity)
			{
				return;
			}
			lastRainIntensityValue = RainIntensity;
			if (RainIntensity <= 0.01f)
			{
				if (audioSourceRainCurrent != null)
				{
					audioSourceRainCurrent.Stop();
					audioSourceRainCurrent = null;
				}
				if (RainFallParticleSystem != null)
				{
					ParticleSystem.EmissionModule emission = RainFallParticleSystem.emission;
					emission.enabled = false;
					RainFallParticleSystem.Stop();
				}
				if (RainMistParticleSystem != null)
				{
					ParticleSystem.EmissionModule emission2 = RainMistParticleSystem.emission;
					emission2.enabled = false;
					RainMistParticleSystem.Stop();
				}
				return;
			}
			LoopingAudioSource loopingAudioSource = ((RainIntensity >= 0.67f) ? audioSourceRainHeavy : ((!(RainIntensity >= 0.33f)) ? audioSourceRainLight : audioSourceRainMedium));
			if (audioSourceRainCurrent != loopingAudioSource)
			{
				if (audioSourceRainCurrent != null)
				{
					audioSourceRainCurrent.Stop();
				}
				audioSourceRainCurrent = loopingAudioSource;
				audioSourceRainCurrent.Play(1f);
			}
			if (RainFallParticleSystem != null)
			{
				ParticleSystem.EmissionModule emission3 = RainFallParticleSystem.emission;
				bool flag = true;
				RainFallParticleSystem.GetComponent<Renderer>().enabled = flag;
				emission3.enabled = flag;
				if (!RainFallParticleSystem.isPlaying)
				{
					RainFallParticleSystem.Play();
				}
				ParticleSystem.MinMaxCurve rateOverTime = emission3.rateOverTime;
				rateOverTime.mode = ParticleSystemCurveMode.Constant;
				float num3 = (rateOverTime.constantMin = (rateOverTime.constantMax = RainFallEmissionRate()));
				emission3.rateOverTime = rateOverTime;
			}
			if (RainMistParticleSystem != null)
			{
				ParticleSystem.EmissionModule emission4 = RainMistParticleSystem.emission;
				bool flag = true;
				RainMistParticleSystem.GetComponent<Renderer>().enabled = flag;
				emission4.enabled = flag;
				if (!RainMistParticleSystem.isPlaying)
				{
					RainMistParticleSystem.Play();
				}
				float num4 = ((!(RainIntensity < RainMistThreshold)) ? MistEmissionRate() : 0f);
				ParticleSystem.MinMaxCurve rateOverTime2 = emission4.rateOverTime;
				rateOverTime2.mode = ParticleSystemCurveMode.Constant;
				float num3 = (rateOverTime2.constantMin = (rateOverTime2.constantMax = num4));
				emission4.rateOverTime = rateOverTime2;
			}
		}

		protected virtual void Start()
		{
			if (Camera == null)
			{
				Camera = Camera.main;
			}
			audioSourceRainLight = new LoopingAudioSource(this, RainSoundLight);
			audioSourceRainMedium = new LoopingAudioSource(this, RainSoundMedium);
			audioSourceRainHeavy = new LoopingAudioSource(this, RainSoundHeavy);
			audioSourceWind = new LoopingAudioSource(this, WindSound);
			if (RainFallParticleSystem != null)
			{
				ParticleSystem.EmissionModule emission = RainFallParticleSystem.emission;
				emission.enabled = false;
				Renderer component = RainFallParticleSystem.GetComponent<Renderer>();
				component.enabled = false;
				rainMaterial = new Material(component.material);
				rainMaterial.EnableKeyword("SOFTPARTICLES_OFF");
				component.material = rainMaterial;
			}
			if (RainExplosionParticleSystem != null)
			{
				ParticleSystem.EmissionModule emission2 = RainExplosionParticleSystem.emission;
				emission2.enabled = false;
				Renderer component2 = RainExplosionParticleSystem.GetComponent<Renderer>();
				rainExplosionMaterial = new Material(component2.material);
				rainExplosionMaterial.EnableKeyword("SOFTPARTICLES_OFF");
				component2.material = rainExplosionMaterial;
			}
			if (RainMistParticleSystem != null)
			{
				ParticleSystem.EmissionModule emission3 = RainMistParticleSystem.emission;
				emission3.enabled = false;
				Renderer component3 = RainMistParticleSystem.GetComponent<Renderer>();
				component3.enabled = false;
				rainMistMaterial = new Material(component3.material);
				if (UseRainMistSoftParticles)
				{
					rainMistMaterial.EnableKeyword("SOFTPARTICLES_ON");
				}
				else
				{
					rainMistMaterial.EnableKeyword("SOFTPARTICLES_OFF");
				}
				component3.material = rainMistMaterial;
			}
		}

		protected virtual void Update()
		{
			CheckForRainChange();
			UpdateWind();
			audioSourceRainLight.Update();
			audioSourceRainMedium.Update();
			audioSourceRainHeavy.Update();
		}

		protected virtual float RainFallEmissionRate()
		{
			return (float)RainFallParticleSystem.main.maxParticles / RainFallParticleSystem.main.startLifetime.constant * RainIntensity;
		}

		protected virtual float MistEmissionRate()
		{
			return (float)RainMistParticleSystem.main.maxParticles / RainMistParticleSystem.main.startLifetime.constant * RainIntensity * RainIntensity;
		}
	}
}
