using SCJogos.Steamworks;
using UnityEngine;

[CreateAssetMenu(fileName = "Track_", menuName = "TrackData", order = 1)]
public class TrackData : ScriptableObject
{
	[SerializeField]
	private string circuitName = "[Clear]Track #";

	[SerializeField]
	private string sceneName = "Track_";

	[SerializeField]
	private Sprite background;

	[SerializeField]
	private Vector2 backgroundScale = new Vector2(0.5f, 0.5f);

	[SerializeField]
	private int numberOfLaps;

	[SerializeField]
	private DLCType[] dlc;

	public string CircuitName
	{
		get
		{
			return circuitName;
		}
		set
		{
			circuitName = value;
		}
	}

	public string SceneName
	{
		get
		{
			return sceneName;
		}
		set
		{
			sceneName = value;
		}
	}

	public Sprite Background
	{
		get
		{
			return background;
		}
		set
		{
			background = value;
		}
	}

	public Vector2 BackgroundScale
	{
		get
		{
			return backgroundScale;
		}
		set
		{
			backgroundScale = value;
		}
	}

	public int NumberOfLaps
	{
		get
		{
			return numberOfLaps;
		}
		set
		{
			numberOfLaps = value;
		}
	}

	public DLCType[] DLC
	{
		get
		{
			return dlc;
		}
		set
		{
			dlc = value;
		}
	}
}
