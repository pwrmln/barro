using UnityEngine;

namespace Core.Database
{
	public class ConfigData : Data
	{
		[SerializeField]
		private float mainVolume;

		[SerializeField]
		private float musicVolume;

		[SerializeField]
		private float sfxVolume;

		[SerializeField]
		private bool achievements;

		[SerializeField]
		private bool initialAchievementMessage;

		[SerializeField]
		private bool useLeaderboard;

		[SerializeField]
		private bool publicLeaderboard;

		protected override string FileName
		{
			get
			{
				return "HydrochoerusHydrochaeris";
			}
		}

		protected override string Prefix
		{
			get
			{
				return "DefaultConfig_";
			}
		}

		public float MainVolume
		{
			get
			{
				return mainVolume;
			}
			set
			{
				mainVolume = value;
			}
		}

		public float MusicVolume
		{
			get
			{
				return musicVolume;
			}
			set
			{
				musicVolume = value;
			}
		}

		public float SFXVolume
		{
			get
			{
				return sfxVolume;
			}
			set
			{
				sfxVolume = value;
			}
		}

		public bool Achievements
		{
			get
			{
				return achievements;
			}
			set
			{
				achievements = value;
			}
		}

		public bool InitialAchievementMessage
		{
			get
			{
				return initialAchievementMessage;
			}
			set
			{
				initialAchievementMessage = value;
			}
		}

		public bool UseLeaderboard
		{
			get
			{
				return useLeaderboard;
			}
			set
			{
				useLeaderboard = value;
			}
		}

		public bool PublicLeaderboard
		{
			get
			{
				return publicLeaderboard;
			}
			set
			{
				publicLeaderboard = value;
			}
		}

		public override void LoadDefault()
		{
			LoadDefaultAchievement();
			LoadDefaultAudio();
			LoadDefaultLeaderboard();
		}

		protected override void ValidateBeforeSave()
		{
		}

		protected void LoadDefaultAudio()
		{
			mainVolume = 1f;
			musicVolume = 1f;
			sfxVolume = 1f;
		}

		protected void LoadDefaultAchievement()
		{
			achievements = false;
			initialAchievementMessage = false;
		}

		private void LoadDefaultLeaderboard()
		{
			useLeaderboard = false;
			publicLeaderboard = false;
		}
	}
}
