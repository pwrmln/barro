using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAScene : MonoBehaviour
{
	public void LoadASceneWithThisNumber(int value)
	{
		StartCoroutine(E_Load_LCD_With_Int(value));
	}

	public void ReloadAScene()
	{
		StartCoroutine(E_Load_LCD(SceneManager.GetActiveScene().name));
	}

	private IEnumerator E_Load_LCD(string value)
	{
		AsyncOperation a = SceneManager.LoadSceneAsync(value, LoadSceneMode.Single);
		a.allowSceneActivation = false;
		while (a.isDone)
		{
			Debug.Log("loading " + value + " : " + a.progress);
			yield return null;
		}
		a.allowSceneActivation = true;
	}

	private IEnumerator E_Load_LCD_With_Int(int value)
	{
		AsyncOperation a = SceneManager.LoadSceneAsync(value, LoadSceneMode.Single);
		a.allowSceneActivation = false;
		while (a.isDone)
		{
			Debug.Log("loading " + value + " : " + a.progress);
			yield return null;
		}
		a.allowSceneActivation = true;
	}
}
