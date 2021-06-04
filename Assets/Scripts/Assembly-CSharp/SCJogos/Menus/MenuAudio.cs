using Core.Audio;
using SCJogos.Database;
using UnityEngine;
using UnityEngine.UI;

namespace SCJogos.Menus
{
	public class MenuAudio : MonoBehaviour
	{
		[SerializeField]
		private Slider sliderMainMenu;

		[SerializeField]
		private Slider sliderMusicMenu;

		[SerializeField]
		private Slider sliderSFXMenu;

		[SerializeField]
		private Button buttonBack;

		[SerializeField]
		private SFXData sfxOnSelect;

		private void Start()
		{
			sliderMainMenu.value = BARDataManager.Instance.ConfigData.MainVolume;
			sliderMusicMenu.value = BARDataManager.Instance.ConfigData.MusicVolume;
			sliderSFXMenu.value = BARDataManager.Instance.ConfigData.SFXVolume;
			sliderMainMenu.onValueChanged.AddListener(OnChangeVolume);
			sliderMusicMenu.onValueChanged.AddListener(OnChangeVolume);
			sliderSFXMenu.onValueChanged.AddListener(OnChangeVolume);
			buttonBack.onClick.AddListener(Save);
		}

		private void OnChangeVolume(float value)
		{
			BARDataManager.Instance.ConfigData.MainVolume = sliderMainMenu.value;
			BARDataManager.Instance.ConfigData.MusicVolume = sliderMusicMenu.value;
			BARDataManager.Instance.ConfigData.SFXVolume = sliderSFXMenu.value;
			AudioManager.Instance.PlaySFXUseSameClip(sfxOnSelect, value);
			AudioManager.Instance.RefreshVolume();
		}

		private void Save()
		{
			BARDataManager.Instance.SaveConfig();
		}
	}
}
