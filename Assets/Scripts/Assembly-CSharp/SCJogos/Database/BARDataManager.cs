using Core.Database;
using UnityEngine;

namespace SCJogos.Database
{
	public class BARDataManager : DataManager
	{
		[SerializeField]
		private BARConfig barConfig;

		public BARConfig BARConfig
		{
			get
			{
				return barConfig;
			}
		}

		public static BARDataManager Instance { get; private set; }

		private void Start()
		{
			Instance = this;
		}

		public void SaveBARConfig()
		{
			barConfig.Save();
		}
	}
}
