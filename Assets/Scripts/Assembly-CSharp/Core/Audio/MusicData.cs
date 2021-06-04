using UnityEngine;

namespace Core.Audio
{
	[CreateAssetMenu(menuName = "SC Jogos/Core/Audio/MusicData", fileName = "MusicData")]
	public class MusicData : ScriptableObject
	{
		[SerializeField]
		private AudioClip audioClip;

		[SerializeField]
		private int repeatAmount;

		public AudioClip AudioClip
		{
			get
			{
				return audioClip;
			}
		}

		public int RepeatAmount
		{
			get
			{
				return repeatAmount;
			}
		}
	}
}
