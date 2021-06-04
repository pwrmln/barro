using Core.Database;
using SCJogos.Steamworks;
using UnityEngine;

namespace SCJogos.Database
{
	public class BARConfig : Data
	{
		[SerializeField]
		private DifficultyType difficulty;

		[SerializeField]
		private bool p1InvertHorizontal;

		[SerializeField]
		private bool p1InvertVertical;

		[SerializeField]
		private bool p2InvertHorizontal;

		[SerializeField]
		private bool p2InvertVertical;

		[SerializeField]
		private GameModeType gameMode;

		[SerializeField]
		private int laps;

		[SerializeField]
		private string track;

		[SerializeField]
		private int playersNumber;

		[SerializeField]
		private int weAreOnTrack;

		[SerializeField]
		private int[] playerCarSelected;

		[SerializeField]
		private int currentTrackSelected;

		[SerializeField]
		private bool p1Mirror;

		[SerializeField]
		private bool p2Mirror;

		protected override string Prefix
		{
			get
			{
				return "NSConfig_" + BARSteamManager.Instance.PlayerID + "_";
			}
		}

		protected override string FileName
		{
			get
			{
				return "PantheraOnca";
			}
		}

		public DifficultyType Difficulty
		{
			get
			{
				return difficulty;
			}
			set
			{
				difficulty = value;
			}
		}

		public bool P1InvertHorizontal
		{
			get
			{
				return p1InvertHorizontal;
			}
			set
			{
				p1InvertHorizontal = value;
			}
		}

		public bool P1InvertVertical
		{
			get
			{
				return p1InvertVertical;
			}
			set
			{
				p1InvertVertical = value;
			}
		}

		public bool P2InvertHorizontal
		{
			get
			{
				return p2InvertHorizontal;
			}
			set
			{
				p2InvertHorizontal = value;
			}
		}

		public bool P2InvertVertical
		{
			get
			{
				return p2InvertVertical;
			}
			set
			{
				p2InvertVertical = value;
			}
		}

		public GameModeType GameMode
		{
			get
			{
				return gameMode;
			}
			set
			{
				gameMode = value;
			}
		}

		public int Laps
		{
			get
			{
				return laps;
			}
			set
			{
				laps = value;
			}
		}

		public string Track
		{
			get
			{
				return track;
			}
			set
			{
				track = value;
			}
		}

		public int PlayersNumber
		{
			get
			{
				return playersNumber;
			}
			set
			{
				playersNumber = value;
			}
		}

		public int WeAreOnTrack
		{
			get
			{
				return weAreOnTrack;
			}
			set
			{
				weAreOnTrack = value;
			}
		}

		public int[] PlayerCarSelected
		{
			get
			{
				return playerCarSelected;
			}
			set
			{
				playerCarSelected = value;
			}
		}

		public int CurrentTrackSelected
		{
			get
			{
				return currentTrackSelected;
			}
			set
			{
				currentTrackSelected = value;
			}
		}

		public bool P1Mirror
		{
			get
			{
				return p1Mirror;
			}
			set
			{
				p1Mirror = value;
			}
		}

		public bool P2Mirror
		{
			get
			{
				return p2Mirror;
			}
			set
			{
				p2Mirror = value;
			}
		}

		protected override void ValidateBeforeSave()
		{
		}

		public override void LoadDefault()
		{
			difficulty = DifficultyType.Easy;
			p1InvertHorizontal = false;
			p1InvertVertical = false;
			p2InvertHorizontal = false;
			p2InvertVertical = false;
			gameMode = GameModeType.Arcade;
			weAreOnTrack = 0;
			playerCarSelected = new int[10] { 3, 0, 1, 2, 0, 0, 0, 0, 0, 0 };
			currentTrackSelected = 0;
			laps = 1;
			p1Mirror = true;
			p2Mirror = true;
		}
	}
}
