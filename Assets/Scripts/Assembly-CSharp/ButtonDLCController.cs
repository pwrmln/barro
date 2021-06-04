using SCJogos.Steamworks;
using UnityEngine;

public class ButtonDLCController : MonoBehaviour
{
	[SerializeField]
	private DLCType dlcType;

	public DLCType DLCType
	{
		get
		{
			return dlcType;
		}
		set
		{
			dlcType = value;
		}
	}

	private void Start()
	{
		CheckEnable();
	}

	public void CheckEnable()
	{
		base.gameObject.SetActive(!BARSteamManager.Instance.IsDLCInstalled(dlcType));
	}

	public void OnClick()
	{
		BARSteamManager.Instance.OpenDLCSite(dlcType);
	}
}
