using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/ListCar", order = 1)]
public class InventoryCar : ScriptableObject
{
	public List<ItemCar> inventoryItem = new List<ItemCar>();

	public bool b_mobile;

	public float mobileMaxSpeedOffset;

	public float mobileWheelStearingOffsetReactivity;

	public bool b_Countdown = true;

	public bool b_LapCounter = true;
}
