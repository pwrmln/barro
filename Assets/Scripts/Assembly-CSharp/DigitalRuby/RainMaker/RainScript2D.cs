using UnityEngine;

namespace DigitalRuby.RainMaker
{
	public class RainScript2D : BaseRainScript
	{
		private static readonly Color32 explosionColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		private float cameraMultiplier = 1f;

		private Bounds visibleBounds = default(Bounds);

		private float yOffset;

		private float visibleWorldWidth;

		private float initialEmissionRain;

		private Vector2 initialStartSpeedRain;

		private Vector2 initialStartSizeRain;

		private Vector2 initialStartSpeedMist;

		private Vector2 initialStartSizeMist;

		private Vector2 initialStartSpeedExplosion;

		private Vector2 initialStartSizeExplosion;

		private readonly ParticleSystem.Particle[] particles = new ParticleSystem.Particle[2048];

		[Tooltip("The starting y offset for rain and mist. This will be offset as a percentage of visible height from the top of the visible world.")]
		public float RainHeightMultiplier = 0.15f;

		[Tooltip("The total width of the rain and mist as a percentage of visible width")]
		public float RainWidthMultiplier = 1.5f;

		[Tooltip("Collision mask for the rain particles")]
		public LayerMask CollisionMask = -1;

		[Tooltip("Lifetime to assign to rain particles that have collided. 0 for instant death. This can allow the rain to penetrate a little bit beyond the collision point.")]
		[Range(0f, 0.5f)]
		public float CollisionLifeTimeRain = 0.02f;

		[Tooltip("Multiply the velocity of any mist colliding by this amount")]
		[Range(0f, 0.99f)]
		public float RainMistCollisionMultiplier = 0.75f;

		protected override bool UseRainMistSoftParticles
		{
			get
			{
				return false;
			}
		}

		private void EmitExplosion(ref Vector3 pos)
		{
			for (int num = Random.Range(2, 5); num != 0; num--)
			{
				float x = Random.Range(-2f, 2f) * cameraMultiplier;
				float y = Random.Range(1f, 3f) * cameraMultiplier;
				float startLifetime = Random.Range(0.1f, 0.2f);
				float startSize = Random.Range(0.05f, 0.1f) * cameraMultiplier;
				ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
				emitParams.position = pos;
				emitParams.velocity = new Vector3(x, y, 0f);
				emitParams.startLifetime = startLifetime;
				emitParams.startSize = startSize;
				emitParams.startColor = explosionColor;
				RainExplosionParticleSystem.Emit(emitParams, 1);
			}
		}

		private void TransformParticleSystem(ParticleSystem p, Vector2 initialStartSpeed, Vector2 initialStartSize)
		{
			if (!(p == null))
			{
				if (FollowCamera)
				{
					p.transform.position = new Vector3(Camera.transform.position.x, visibleBounds.max.y + yOffset, p.transform.position.z);
				}
				else
				{
					p.transform.position = new Vector3(p.transform.position.x, visibleBounds.max.y + yOffset, p.transform.position.z);
				}
				p.transform.localScale = new Vector3(visibleWorldWidth * RainWidthMultiplier, 1f, 1f);
				ParticleSystem.MainModule main = p.main;
				ParticleSystem.MinMaxCurve startSpeed = main.startSpeed;
				ParticleSystem.MinMaxCurve startSize = main.startSize;
				startSpeed.constantMin = initialStartSpeed.x * cameraMultiplier;
				startSpeed.constantMax = initialStartSpeed.y * cameraMultiplier;
				startSize.constantMin = initialStartSize.x * cameraMultiplier;
				startSize.constantMax = initialStartSize.y * cameraMultiplier;
				main.startSpeed = startSpeed;
				main.startSize = startSize;
			}
		}

		private void CheckForCollisionsRainParticles()
		{
			int num = 0;
			bool flag = false;
			if ((int)CollisionMask != 0)
			{
				num = RainFallParticleSystem.GetParticles(particles);
				for (int i = 0; i < num; i++)
				{
					Vector3 vector = particles[i].position + RainFallParticleSystem.transform.position;
					RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, particles[i].velocity.normalized, particles[i].velocity.magnitude * Time.deltaTime);
					if (raycastHit2D.collider != null && ((1 << raycastHit2D.collider.gameObject.layer) & (int)CollisionMask) != 0)
					{
						if (CollisionLifeTimeRain == 0f)
						{
							particles[i].remainingLifetime = 0f;
						}
						else
						{
							particles[i].remainingLifetime = Mathf.Min(particles[i].remainingLifetime, Random.Range(CollisionLifeTimeRain * 0.5f, CollisionLifeTimeRain * 2f));
							vector += particles[i].velocity * Time.deltaTime;
						}
						flag = true;
					}
				}
			}
			if (RainExplosionParticleSystem != null)
			{
				if (num == 0)
				{
					num = RainFallParticleSystem.GetParticles(particles);
				}
				for (int j = 0; j < num; j++)
				{
					if (particles[j].remainingLifetime < 0.24f)
					{
						Vector3 pos = particles[j].position + RainFallParticleSystem.transform.position;
						EmitExplosion(ref pos);
					}
				}
			}
			if (flag)
			{
				RainFallParticleSystem.SetParticles(particles, num);
			}
		}

		private void CheckForCollisionsMistParticles()
		{
			if (RainMistParticleSystem == null || (int)CollisionMask == 0)
			{
				return;
			}
			int num = RainMistParticleSystem.GetParticles(particles);
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				Vector3 vector = particles[i].position + RainMistParticleSystem.transform.position;
				if (Physics2D.Raycast(vector, particles[i].velocity.normalized, particles[i].velocity.magnitude * Time.deltaTime, CollisionMask).collider != null)
				{
					particles[i].velocity *= RainMistCollisionMultiplier;
					flag = true;
				}
			}
			if (flag)
			{
				RainMistParticleSystem.SetParticles(particles, num);
			}
		}

		protected override void Start()
		{
			base.Start();
			initialEmissionRain = RainFallParticleSystem.emission.rateOverTime.constant;
			initialStartSpeedRain = new Vector2(RainFallParticleSystem.main.startSpeed.constantMin, RainFallParticleSystem.main.startSpeed.constantMax);
			initialStartSizeRain = new Vector2(RainFallParticleSystem.main.startSize.constantMin, RainFallParticleSystem.main.startSize.constantMax);
			if (RainMistParticleSystem != null)
			{
				initialStartSpeedMist = new Vector2(RainMistParticleSystem.main.startSpeed.constantMin, RainMistParticleSystem.main.startSpeed.constantMax);
				initialStartSizeMist = new Vector2(RainMistParticleSystem.main.startSize.constantMin, RainMistParticleSystem.main.startSize.constantMax);
			}
			if (RainExplosionParticleSystem != null)
			{
				initialStartSpeedExplosion = new Vector2(RainExplosionParticleSystem.main.startSpeed.constantMin, RainExplosionParticleSystem.main.startSpeed.constantMax);
				initialStartSizeExplosion = new Vector2(RainExplosionParticleSystem.main.startSize.constantMin, RainExplosionParticleSystem.main.startSize.constantMax);
			}
		}

		protected override void Update()
		{
			base.Update();
			cameraMultiplier = Camera.orthographicSize * 0.25f;
			visibleBounds.min = Camera.main.ViewportToWorldPoint(Vector3.zero);
			visibleBounds.max = Camera.main.ViewportToWorldPoint(Vector3.one);
			visibleWorldWidth = visibleBounds.size.x;
			yOffset = (visibleBounds.max.y - visibleBounds.min.y) * RainHeightMultiplier;
			TransformParticleSystem(RainFallParticleSystem, initialStartSpeedRain, initialStartSizeRain);
			TransformParticleSystem(RainMistParticleSystem, initialStartSpeedMist, initialStartSizeMist);
			TransformParticleSystem(RainExplosionParticleSystem, initialStartSpeedExplosion, initialStartSizeExplosion);
			CheckForCollisionsRainParticles();
			CheckForCollisionsMistParticles();
		}

		protected override float RainFallEmissionRate()
		{
			return initialEmissionRain * RainIntensity;
		}
	}
}
