using UnityEngine;
using UnityEngine.UI;

namespace Core.Versions
{
	public class VersionManager : MonoBehaviour
	{
		private static string VERSION = "V.2021.1.0";

		[SerializeField]
		private Text textVersion;

		private void OnEnable()
		{
			if (textVersion != null)
			{
				textVersion.text = VERSION;
			}
		}
	}
}
