using System;
using Core.Steamworks;
using Steamworks;
using UnityEngine;

namespace SCJogos.Steamworks
{
	public class BARSteamManager : SteamManager
	{
		private bool canUnlock;

		private float achievementUpdate;

		private string achievementName;

		private int maxAchievementCount;

		private int currentAchievement;

		private bool achieved;

		private bool AllAchievements;

		public static BARSteamManager Instance { get; private set; }

		public bool DLC_MapPack1 { get; private set; }

		public bool DLC_MapPack2 { get; private set; }

		public bool DLC_Supporters { get; private set; }

		public bool DLC_2020Expansion { get; private set; }

		protected override uint GameAppID
		{
			get
			{
				return 618140u;
			}
		}

		protected override uint[] DLCsAppID
		{
			get
			{
				return new uint[4] { 834130u, 834131u, 1264040u, 1374380u };
			}
		}

		protected override string[] LeaderboardNames
		{
			get
			{
				return new string[23]
				{
					"Track_01", "Track_02", "Track_03", "Track_04", "Track_05", "Track_06", "Track_07", "Track_08", "Track_09", "Track_10",
					"Track_11", "Track_12", "Track_13", "Track_14", "Track_15", "Track_16", "Track_17", "Track_18", "Track_19", "Track_20",
					"Track_21", "Track_22", "Track_23"
				};
			}
		}

		private void Awake()
		{
			AllAchievements = false;
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			foreach (string text in commandLineArgs)
			{
				if (text.Contains("-AllAchi"))
				{
					AllAchievements = true;
					break;
				}
			}
		}

		private void Update()
		{
			if (base.SteamVersion && configData.Achievements && canUnlock && Time.time > achievementUpdate)
			{
				achievementUpdate = Time.time + 5f;
				UpdateAchievement();
			}
		}

		private void UpdateAchievement()
		{
			SteamUserStats.RequestCurrentStats();
			achievementName = string.Empty;
			int num = 5100;
			while (string.IsNullOrEmpty(achievementName) && num > 0 && currentAchievement <= maxAchievementCount)
			{
				num--;
				achievementName = SteamUserStats.GetAchievementName((uint)currentAchievement);
				currentAchievement++;
				if (!string.IsNullOrEmpty(achievementName) && ((!AllAchievements && !achievementName.Contains("_ACHIEVEMENT_")) || (SteamUserStats.GetAchievement(achievementName, out achieved) && achieved)))
				{
					achievementName = string.Empty;
				}
			}
			if (currentAchievement > maxAchievementCount)
			{
				num = 5100;
				currentAchievement = 0;
				while (string.IsNullOrEmpty(achievementName) && num > 0 && currentAchievement <= maxAchievementCount)
				{
					num--;
					achievementName = SteamUserStats.GetAchievementName((uint)currentAchievement);
					currentAchievement++;
					if (!string.IsNullOrEmpty(achievementName) && ((!AllAchievements && !achievementName.Contains("_ACHIEVEMENT_")) || (SteamUserStats.GetAchievement(achievementName, out achieved) && achieved)))
					{
						achievementName = string.Empty;
					}
				}
				if (currentAchievement > maxAchievementCount)
				{
					canUnlock = false;
				}
			}
			if (!string.IsNullOrEmpty(achievementName))
			{
				SteamUserStats.SetAchievement(achievementName);
				SteamUserStats.StoreStats();
			}
		}

		protected override void CheckDLCs()
		{
			DLC_MapPack1 = ValidateDLC(DLCsAppID[0]);
			DLC_MapPack2 = ValidateDLC(DLCsAppID[1]);
			DLC_Supporters = ValidateDLC(DLCsAppID[2]);
			DLC_2020Expansion = ValidateDLC(DLCsAppID[3]);
		}

		public bool IsDLCInstalled(DLCType dlc)
		{
			switch (dlc)
			{
			case DLCType.None:
				return true;
			case DLCType.MapPack1:
				return DLC_MapPack1;
			case DLCType.MapPack2:
				return DLC_MapPack2;
			case DLCType.Supporters:
				return DLC_Supporters;
			case DLCType.Expansion2020:
				return DLC_2020Expansion;
			default:
				return false;
			}
		}

		public void OpenDLCSite(DLCType dlc)
		{
			switch (dlc)
			{
			case DLCType.MapPack1:
				SteamFriends.ActivateGameOverlayToStore(new AppId_t(DLCsAppID[0]), EOverlayToStoreFlag.k_EOverlayToStoreFlag_AddToCartAndShow);
				break;
			case DLCType.MapPack2:
				SteamFriends.ActivateGameOverlayToStore(new AppId_t(DLCsAppID[1]), EOverlayToStoreFlag.k_EOverlayToStoreFlag_AddToCartAndShow);
				break;
			case DLCType.Supporters:
				SteamFriends.ActivateGameOverlayToStore(new AppId_t(DLCsAppID[2]), EOverlayToStoreFlag.k_EOverlayToStoreFlag_AddToCartAndShow);
				break;
			case DLCType.Expansion2020:
				SteamFriends.ActivateGameOverlayToStore(new AppId_t(DLCsAppID[3]), EOverlayToStoreFlag.k_EOverlayToStoreFlag_AddToCartAndShow);
				break;
			}
		}

		protected override void InitInstance()
		{
			Instance = this;
		}

		protected override void OnStart()
		{
			if (base.SteamVersion)
			{
				maxAchievementCount = (int)SteamUserStats.GetNumAchievements();
				currentAchievement = 0;
				canUnlock = true;
			}
			else
			{
				canUnlock = false;
			}
		}
	}
}
