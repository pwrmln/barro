using UnityEngine;

namespace Core.Audio
{
	[CreateAssetMenu(menuName = "SC Jogos/Core/Audio/SFXData", fileName = "SFXData")]
	public class SFXData : ScriptableObject
	{
		[SerializeField]
		private AudioClip audioClip;

		public AudioClip AudioClip
		{
			get
			{
				return audioClip;
			}
		}
	}
}
