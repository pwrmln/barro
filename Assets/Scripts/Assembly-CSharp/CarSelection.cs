using System;
using SCJogos.Database;
using UnityEngine;

public class CarSelection : MonoBehaviour
{
	[Serializable]
	public class _ListOfCars
	{
		public int currentCarSelection;

		public GameObject pivotToSpawnCar;
	}

	public bool SeeInspector;

	public _ListOfCars[] ListOfCars = new _ListOfCars[10];

	public GameObject parentMenu;

	public InventoryCar inventoryItemCar;

	public void lastCar(int playerNumber)
	{
		ListOfCars[playerNumber].currentCarSelection--;
		if (ListOfCars[playerNumber].currentCarSelection < 0)
		{
			ListOfCars[playerNumber].currentCarSelection = inventoryItemCar.inventoryItem[playerNumber].Cars.Count - 1;
		}
		LoadNewCar(playerNumber);
	}

	public void NextCar(int playerNumber)
	{
		ListOfCars[playerNumber].currentCarSelection = (ListOfCars[playerNumber].currentCarSelection + 1) % inventoryItemCar.inventoryItem[playerNumber].Cars.Count;
		LoadNewCar(playerNumber);
	}

	public void initCarSelection(int NumberOfPlayer)
	{
		for (int i = 0; i < ListOfCars.Length; i++)
		{
			ListOfCars[i].currentCarSelection = BARDataManager.Instance.BARConfig.PlayerCarSelected[i];
			LoadNewCar(i);
		}
	}

	public void LoadNewCar(int playerNumber)
	{
		GameObject pivotToSpawnCar = ListOfCars[playerNumber].pivotToSpawnCar;
		Transform[] componentsInChildren = pivotToSpawnCar.GetComponentsInChildren<Transform>();
		Transform[] array = componentsInChildren;
		foreach (Transform transform in array)
		{
			if (transform != pivotToSpawnCar.transform)
			{
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
		if (inventoryItemCar.inventoryItem[playerNumber].Cars[ListOfCars[playerNumber].currentCarSelection] != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(inventoryItemCar.inventoryItem[playerNumber].Cars[ListOfCars[playerNumber].currentCarSelection], ListOfCars[playerNumber].pivotToSpawnCar.transform);
			gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
			gameObject.GetComponent<Rigidbody>().isKinematic = true;
			CarController component = gameObject.GetComponent<CarController>();
			component.enabled = false;
			component.audio_.mute = true;
			component.objSkid_Sound.mute = true;
			component.obj_CarImpact_Sound.mute = true;
			gameObject.GetComponent<CarAI>().enabled = false;
			Transform transform2 = gameObject.GetComponent<CarController>().pivotCarSelection.transform;
			transform2.parent = ListOfCars[playerNumber].pivotToSpawnCar.transform;
			gameObject.transform.parent = transform2;
			transform2.transform.localPosition = Vector3.zero;
			transform2.transform.eulerAngles = ListOfCars[playerNumber].pivotToSpawnCar.transform.eulerAngles;
			BARDataManager.Instance.BARConfig.PlayerCarSelected[playerNumber] = ListOfCars[playerNumber].currentCarSelection;
			BARDataManager.Instance.SaveBARConfig();
		}
	}
}
