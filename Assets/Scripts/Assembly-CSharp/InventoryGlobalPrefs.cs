using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/List TestCar", order = 1)]
public class InventoryGlobalPrefs : ScriptableObject
{
	public List<ItemGlobalPrefs> inventoryItem = new List<ItemGlobalPrefs>();
}
