using UnityEngine;

namespace Core.Versions
{
	[CreateAssetMenu(menuName = "SC Jogos/Core/Versions/VersionData", fileName = "VersionData")]
	public class VersionData : ScriptableObject
	{
		[SerializeField]
		private string version;

		[TextArea]
		[SerializeField]
		private string description;

		public string Version
		{
			get
			{
				return version;
			}
			set
			{
				version = value;
			}
		}

		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}
	}
}
