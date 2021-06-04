using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemGlobalPrefs
{
	public bool b_TestMode;

	public List<GameObject> list_Cars = new List<GameObject>();

	public List<bool> list_playerType = new List<bool>();

	public GameObject Canvas_TestMode;
}
