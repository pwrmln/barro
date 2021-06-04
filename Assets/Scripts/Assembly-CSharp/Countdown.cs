using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
	public bool SeeInspector;

	public bool b_Pause;

	public bool b_ActivateCountdown;

	private float Timer;

	private string LastTimer = string.Empty;

	public Text txt_Countdown;

	public List<string> list_Txt = new List<string>();

	public List<AudioClip> list_Audio = new List<AudioClip>();

	private AudioSource _audio;

	private Game_Manager gameManager;

	private void Start()
	{
		Timer = list_Txt.Count;
		_audio = GetComponent<AudioSource>();
		if (gameManager == null)
		{
			GameObject gameObject = GameObject.Find("Game_Manager");
			if ((bool)gameObject)
			{
				gameManager = gameObject.GetComponent<Game_Manager>();
			}
		}
	}

	private void Update()
	{
		if (!b_Pause && b_ActivateCountdown && Timer > 0f)
		{
			F_Countdown();
		}
	}

	private void F_Countdown()
	{
		Timer -= Time.deltaTime;
		string text = Mathf.Floor(Timer % 60f).ToString("0");
		if ((bool)txt_Countdown && LastTimer != text && Timer >= 0f)
		{
			int num = int.Parse(text);
			txt_Countdown.text = list_Txt[list_Txt.Count - 1 - num];
			if ((bool)_audio && list_Audio[list_Txt.Count - 1 - num] != null)
			{
				_audio.clip = list_Audio[list_Txt.Count - 1 - num];
				_audio.Play();
			}
			if ((bool)gameManager && text == "0")
			{
				gameManager.RaceStart();
			}
		}
		else if ((bool)txt_Countdown && LastTimer != text)
		{
			txt_Countdown.text = string.Empty;
		}
		LastTimer = text;
	}

	public void Pause()
	{
		if (b_Pause)
		{
			b_Pause = false;
			_audio.UnPause();
		}
		else
		{
			_audio.Pause();
			b_Pause = true;
		}
	}
}
