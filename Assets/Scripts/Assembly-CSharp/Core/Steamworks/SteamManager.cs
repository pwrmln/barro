using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Core.Database;
using Steamworks;
using UnityEngine;

namespace Core.Steamworks
{
	public abstract class SteamManager : MonoBehaviour
	{
		[SerializeField]
		protected ConfigData configData;

		private string secretKey = "PumaConcolor";

		private string addScoreURL = "https://scjogos.com/access/addscore.php";

		private string getScoreURL = "https://scjogos.com/access/getscore.php";

		private string deleteScoreURL = "https://scjogos.com/access/deletescore.php";

		private string connectURL = "https://scjogos.com/access/connect.php";

		private Dictionary<string, List<LeaderboardResult>> leaderboardsTopScores;

		public bool SteamVersion { get; private set; }

		public string PlayerID { get; private set; }

		public string PlayerName { get; set; }

		protected abstract uint GameAppID { get; }

		protected abstract uint[] DLCsAppID { get; }

		public Dictionary<string, List<LeaderboardResult>> TopScores
		{
			get
			{
				return leaderboardsTopScores;
			}
		}

		protected abstract string[] LeaderboardNames { get; }

		private void Start()
		{
			InitInstance();
			if (SteamAPI.RestartAppIfNecessary(new AppId_t(GameAppID)))
			{
				Application.Quit();
				return;
			}
			SteamVersion = SteamAPI.Init();
			if (SteamVersion)
			{
				PlayerID = SteamUser.GetSteamID().m_SteamID.ToString();
				PlayerName = SteamFriends.GetPersonaName();
				CheckDLCs();
			}
			else
			{
				PlayerID = string.Empty;
				PlayerName = string.Empty;
			}
			InitiateLeaderboard();
			OnStart();
		}

		private void OnApplicationQuit()
		{
			if (SteamVersion)
			{
				SteamAPI.Shutdown();
			}
		}

		public void DeleteAllData()
		{
			if (SteamVersion)
			{
				SteamUserStats.RequestCurrentStats();
				SteamUserStats.ResetAllStats(true);
				SteamUserStats.StoreStats();
				StartCoroutine(DeleteScore());
			}
		}

		protected abstract void OnStart();

		protected abstract void InitInstance();

		public bool SetAchievement(string achievement)
		{
			try
			{
				if (SteamVersion && configData.Achievements)
				{
					SteamUserStats.RequestCurrentStats();
					bool pbAchieved;
					if (SteamUserStats.GetAchievement(achievement, out pbAchieved) && !pbAchieved)
					{
						SteamUserStats.SetAchievement(achievement);
						SteamUserStats.StoreStats();
						return true;
					}
				}
				return false;
			}
			catch
			{
				return false;
			}
			finally
			{
				SteamAPI.RunCallbacks();
			}
		}

		protected bool ValidateDLC(uint appID)
		{
			if (SteamVersion)
			{
				return SteamApps.BIsDlcInstalled(new AppId_t(appID));
			}
			return false;
		}

		protected abstract void CheckDLCs();

		private void InitiateLeaderboard()
		{
			leaderboardsTopScores = new Dictionary<string, List<LeaderboardResult>>();
			if (configData.UseLeaderboard)
			{
				StartCoroutine(Connect());
			}
		}

		private void GetAllScores()
		{
			string[] leaderboardNames = LeaderboardNames;
			foreach (string leaderboard in leaderboardNames)
			{
				GetScores(leaderboard);
			}
		}

		public void GetScores(string leaderboard)
		{
			if (!leaderboardsTopScores.ContainsKey(leaderboard))
			{
				leaderboardsTopScores.Add(leaderboard, new List<LeaderboardResult>());
			}
			else
			{
				leaderboardsTopScores[leaderboard].Clear();
			}
			if (!SteamVersion || !configData.UseLeaderboard)
			{
				return;
			}
			WWW wWW = new WWW(getScoreURL, GetForm(leaderboard));
			float num = Time.time + 0.5f;
			while (!wWW.isDone && Time.time < num)
			{
			}
			if (wWW.error != null)
			{
				Debug.LogError("There was an error getting the high score: " + wWW.error);
			}
			else
			{
				string[] array = wWW.text.Split('|');
				if (array.Length > 2)
				{
					for (int j = 0; j < array.Length; j += 3)
					{
						float result;
						if (float.TryParse(array[j + 2], out result))
						{
							LeaderboardResult leaderboardResult = new LeaderboardResult();
							leaderboardResult.PlayerID = array[j];
							leaderboardResult.PlayerName = array[j + 1];
							leaderboardResult.PlayerScore = result;
							leaderboardsTopScores[leaderboard].Add(leaderboardResult);
						}
					}
				}
			}
			leaderboardsTopScores[leaderboard] = leaderboardsTopScores[leaderboard].OrderBy((LeaderboardResult i) => i.PlayerScore).ToList();
		}

		public void PostScore(string leaderboardName, float score)
		{
			StartCoroutine(Connect());
			StartCoroutine(AddScore(leaderboardName, score));
		}

		private IEnumerator AddScore(string leaderboardName, float score)
		{
			if (SteamVersion && configData.UseLeaderboard)
			{
				WWW hs_post = new WWW(addScoreURL, GetForm(leaderboardName, score));
				yield return hs_post;
				if (hs_post.error != null)
				{
					Debug.LogError("There was an error getting the high score: " + hs_post.error);
				}
			}
		}

		private IEnumerator DeleteScore()
		{
			WWW hs_post = new WWW(deleteScoreURL, GetForm(string.Empty));
			yield return hs_post;
			if (hs_post.error != null)
			{
				Debug.LogError("There was an error getting the high score: " + hs_post.error);
			}
		}

		private IEnumerator Connect()
		{
			if (SteamVersion && configData.UseLeaderboard)
			{
				WWW hs_post = new WWW(connectURL, GetForm(string.Empty));
				yield return hs_post;
				if (hs_post.error != null)
				{
					Debug.LogError("There was an error connecting: " + hs_post.error);
				}
			}
		}

		private WWWForm GetForm(string leaderboardName, float score = 0f)
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("steamid", PlayerID);
			wWWForm.AddField("leaderboard", leaderboardName);
			wWWForm.AddField("name", PlayerName.Replace('|', '!'));
			wWWForm.AddField("appid", GameAppID.ToString());
			wWWForm.AddField("score", score.ToString());
			wWWForm.AddField("hash", Md5Sum(PlayerID + secretKey));
			return wWWForm;
		}

		private string Md5Sum(string strToEncrypt)
		{
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			byte[] bytes = uTF8Encoding.GetBytes(strToEncrypt);
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
			string text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				text += Convert.ToString(array[i], 16).PadLeft(2, '0');
			}
			return text.PadLeft(32, '0');
		}
	}
}
