using System;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
	[Serializable]
	public class _difficulties
	{
		public List<float> addGlobalSpeedOffset = new List<float>();

		public List<int> waypointSuccesRate = new List<int>();

		public List<float> waypointMinTarget = new List<float>();

		public List<float> waypointMaxTarget = new List<float>();

		public List<int> speedSuccesRate = new List<int>();

		public List<float> speedOffset = new List<float>();

		public List<int> rotationSuccesRate = new List<int>();

		public List<float> rotationOffset = new List<float>();
	}

	public bool SeeInspector;

	public int selectedDifficulties;

	public List<_difficulties> difficulties = new List<_difficulties>();
}
