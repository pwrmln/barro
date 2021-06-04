using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QualitySelector : MonoBehaviour
{
	public int currentResolution;

	public Text txt_Resolution;

	public List<Button> l_QualityButtons = new List<Button>();

	public List<GameObject> l_QualityCheckmarks = new List<GameObject>();

	public int LastHeightResolution;

	public int LastWidthResolution;

	private Resolution[] resolutions;

	private void Start()
	{
		if ((bool)txt_Resolution)
		{
			txt_Resolution.text = Screen.currentResolution.width + "x" + Screen.currentResolution.height;
		}
		resolutions = Screen.resolutions.Distinct().ToArray();
		for (int i = 0; i < resolutions.Length; i++)
		{
			if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
			{
				currentResolution = i;
			}
		}
		InitQualitySettingsOnScreen();
	}

	public void ChooseQualitySettings(int quality)
	{
		QualitySettings.SetQualityLevel(quality, true);
		InitQualitySettingsOnScreen();
	}

	public void InitQualitySettingsOnScreen()
	{
		int qualityLevel = QualitySettings.GetQualityLevel();
		for (int i = 0; i < l_QualityCheckmarks.Count; i++)
		{
			if (i == qualityLevel)
			{
				l_QualityCheckmarks[i].SetActive(true);
			}
			else
			{
				l_QualityCheckmarks[i].SetActive(false);
			}
		}
	}

	public void ChooseResolution(int value)
	{
		LastHeightResolution = resolutions[currentResolution].height;
		LastWidthResolution = resolutions[currentResolution].width;
		currentResolution += value;
		if (currentResolution < 0)
		{
			currentResolution = resolutions.Length - 1;
		}
		else
		{
			currentResolution %= resolutions.Length;
		}
		if ((bool)txt_Resolution)
		{
			txt_Resolution.text = resolutions[currentResolution].width + "x" + resolutions[currentResolution].height;
		}
	}

	public void ValidateResolution()
	{
		Screen.SetResolution(resolutions[currentResolution].width, resolutions[currentResolution].height, true);
	}
}
