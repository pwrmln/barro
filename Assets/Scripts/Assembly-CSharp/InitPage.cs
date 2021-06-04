using System.Collections;
using SCJogos.Database;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitPage : MonoBehaviour
{
	[Header("Duration before launching the next scene.")]
	public float WaitDuration = 2f;

	private void Awake()
	{
		StartCoroutine("F_WaitBeforeLaunchingGame");
	}

	private IEnumerator F_WaitBeforeLaunchingGame()
	{
		yield return new WaitForSeconds(WaitDuration);
		BARDataManager.Instance.BARConfig.WeAreOnTrack = 0;
		SceneManager.LoadScene(1);
	}
}
