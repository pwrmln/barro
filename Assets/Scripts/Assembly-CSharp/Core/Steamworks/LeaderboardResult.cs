using System;

namespace Core.Steamworks
{
	[Serializable]
	public class LeaderboardResult
	{
		public string PlayerName { get; set; }

		public string PlayerID { get; set; }

		public float PlayerScore { get; set; }
	}
}
