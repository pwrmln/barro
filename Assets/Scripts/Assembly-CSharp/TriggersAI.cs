using System.Collections.Generic;
using UnityEngine;

public class TriggersAI : MonoBehaviour
{
	public bool SeeInspector;

	public List<CarAI> l_Cars = new List<CarAI>();

	public List<bool> l_allowRandomValue = new List<bool>();

	public List<float> l_targetsValues = new List<float>();

	public List<int> successRate_BestTargetPos = new List<int>();

	public void InitTriggersAI(List<CarController> carAI)
	{
		for (int i = 0; i < carAI.Count; i++)
		{
			if (carAI[i] != null)
			{
				l_Cars.Add(carAI[i].gameObject.GetComponent<CarAI>());
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!(other.tag == "Car"))
		{
			return;
		}
		float num = Random.Range(0, 101);
		for (int i = 0; i < l_Cars.Count; i++)
		{
			if (l_Cars[i].gameObject == other.gameObject && l_Cars[i].enabled && (float)successRate_BestTargetPos[i] >= num)
			{
				l_Cars[i].allowRandomValues = l_allowRandomValue[i];
				l_Cars[i].Target.transform.localPosition = new Vector3(l_targetsValues[i], 0f, 0f);
			}
		}
	}
}
