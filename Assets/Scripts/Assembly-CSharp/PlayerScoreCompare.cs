using System;

public class PlayerScoreCompare : IComparable<PlayerScoreCompare>
{
	public string name;

	public string score;

	public string _loop;

	public float total;

	public PlayerScoreCompare(string newName, string newScore, string new_loop, float newtotal)
	{
		name = newName;
		score = newScore;
		_loop = new_loop;
		total = newtotal;
	}

	public int CompareTo(PlayerScoreCompare other)
	{
		if (other == null)
		{
			return 1;
		}
		return (int)(total - other.total);
	}
}
