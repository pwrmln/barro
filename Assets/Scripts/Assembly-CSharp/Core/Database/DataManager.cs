using UnityEngine;

namespace Core.Database
{
	public abstract class DataManager : MonoBehaviour
	{
		[SerializeField]
		private ConfigData configData;

		public ConfigData ConfigData
		{
			get
			{
				return configData;
			}
		}

		public void SaveConfig()
		{
			configData.Save();
		}
	}
}
