using System;

namespace Core
{
	[Serializable]
	public class SaveData
	{
		public string Key { get; set; }

		public object Data { get; set; }
	}
}
