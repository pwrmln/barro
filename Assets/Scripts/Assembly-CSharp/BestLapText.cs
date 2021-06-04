using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BestLapText : MonoBehaviour
{
	[SerializeField]
	private Text text;

	[SerializeField]
	private Text smallText;

	public bool P1 { get; private set; }

	private void Awake()
	{
		text.text = string.Empty;
		smallText.text = string.Empty;
		P1 = GetComponentInParent<Camera>().gameObject.name.Contains("P1_");
	}

	private IEnumerator FadeText()
	{
		yield return new WaitForSeconds(2f);
		Color color = text.color;
		WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
		while (color.a > 0f)
		{
			color = text.color;
			color.a -= Time.timeScale * 0.3f;
			text.color = color;
			color = smallText.color;
			color.a -= Time.timeScale * 0.3f;
			smallText.color = color;
			yield return waitForEndOfFrame;
		}
		text.text = string.Empty;
		smallText.text = string.Empty;
	}

	public void ShowTime(float lapTime, float bestTime)
	{
		StopAllCoroutines();
		text.text = default(DateTime).AddSeconds(lapTime).ToString("mm:ss:fff");
		if (lapTime == bestTime || bestTime == float.MaxValue)
		{
			smallText.text = string.Empty;
		}
		else if (lapTime > bestTime)
		{
			smallText.color = Color.red;
			smallText.text = "+" + default(DateTime).AddSeconds(lapTime - bestTime).ToString("mm:ss:fff");
		}
		else
		{
			smallText.color = Color.green;
			smallText.text = "-" + default(DateTime).AddSeconds(bestTime - lapTime).ToString("mm:ss:fff");
		}
		Color color = text.color;
		color.a = 1f;
		text.color = color;
		StartCoroutine(FadeText());
	}
}
