using System.Collections;
using UnityEngine;

namespace SCJogos.Steamworks
{
	public class AchievementUnlock : MonoBehaviour
	{
		[SerializeField]
		private string achievement;

		private IEnumerator Start()
		{
			yield return new WaitForFixedUpdate();
			yield return new WaitForSecondsRealtime(5f);
			BARSteamManager.Instance.SetAchievement(achievement);
			Object.Destroy(base.gameObject);
		}
	}
}
