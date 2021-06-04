using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/ListInputsValues", order = 1)]
public class DefaultInputsValues : ScriptableObject
{
	[Serializable]
	public class _ListOfInputs
	{
		public List<string> Desktop = new List<string>();

		public List<string> Gamepad = new List<string>();
	}

	public List<_ListOfInputs> ListOfInputs = new List<_ListOfInputs>();
}
