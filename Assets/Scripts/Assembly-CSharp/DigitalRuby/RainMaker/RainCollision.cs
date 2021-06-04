using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.RainMaker
{
	public class RainCollision : MonoBehaviour
	{
		private static readonly Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		private readonly List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

		public ParticleSystem RainExplosion;

		public ParticleSystem RainParticleSystem;

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void Emit(ParticleSystem p, ref Vector3 pos)
		{
			for (int num = Random.Range(2, 5); num != 0; num--)
			{
				float y = Random.Range(1f, 3f);
				float z = Random.Range(-2f, 2f);
				float x = Random.Range(-2f, 2f);
				float startSize = Random.Range(0.05f, 0.1f);
				ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
				emitParams.position = pos;
				emitParams.velocity = new Vector3(x, y, z);
				emitParams.startLifetime = 0.75f;
				emitParams.startSize = startSize;
				emitParams.startColor = color;
				p.Emit(emitParams, 1);
			}
		}

		private void OnParticleCollision(GameObject obj)
		{
			if (RainExplosion != null && RainParticleSystem != null)
			{
				int num = RainParticleSystem.GetCollisionEvents(obj, collisionEvents);
				for (int i = 0; i < num; i++)
				{
					Vector3 pos = collisionEvents[i].intersection;
					Emit(RainExplosion, ref pos);
				}
			}
		}
	}
}
