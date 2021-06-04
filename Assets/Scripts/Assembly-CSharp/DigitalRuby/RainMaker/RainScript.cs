using UnityEngine;

namespace DigitalRuby.RainMaker
{
	public class RainScript : BaseRainScript
	{
		[Tooltip("The height above the camera that the rain will start falling from")]
		public float RainHeight = 25f;

		[Tooltip("How far the rain particle system is ahead of the player")]
		public float RainForwardOffset = -7f;

		[Tooltip("The top y value of the mist particles")]
		public float RainMistHeight = 3f;

		private void UpdateRain()
		{
			if (!(RainFallParticleSystem != null))
			{
				return;
			}
			if (FollowCamera)
			{
				ParticleSystem.ShapeModule shape = RainFallParticleSystem.shape;
				shape.shapeType = ParticleSystemShapeType.ConeVolume;
				RainFallParticleSystem.transform.position = Camera.transform.position;
				RainFallParticleSystem.transform.Translate(0f, RainHeight, RainForwardOffset);
				RainFallParticleSystem.transform.rotation = Quaternion.Euler(0f, Camera.transform.rotation.eulerAngles.y, 0f);
				if (RainMistParticleSystem != null)
				{
					ParticleSystem.ShapeModule shape2 = RainMistParticleSystem.shape;
					shape2.shapeType = ParticleSystemShapeType.HemisphereShell;
					Vector3 position = Camera.transform.position;
					position.y += RainMistHeight;
					RainMistParticleSystem.transform.position = position;
				}
			}
			else
			{
				ParticleSystem.ShapeModule shape3 = RainFallParticleSystem.shape;
				shape3.shapeType = ParticleSystemShapeType.Box;
				if (RainMistParticleSystem != null)
				{
					ParticleSystem.ShapeModule shape4 = RainMistParticleSystem.shape;
					shape4.shapeType = ParticleSystemShapeType.Box;
					Vector3 position2 = RainFallParticleSystem.transform.position;
					position2.y += RainMistHeight;
					position2.y -= RainHeight;
					RainMistParticleSystem.transform.position = position2;
				}
			}
		}

		protected override void Start()
		{
			base.Start();
		}

		protected override void Update()
		{
			base.Update();
			UpdateRain();
		}
	}
}
