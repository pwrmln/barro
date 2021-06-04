using System.Collections;
using UnityEngine;

public class CheckCarOnHisBack : MonoBehaviour
{
	public CarController carController;

	public bool b_Pause;

	public BoxCollider boxCollider;

	private void Start()
	{
		boxCollider = base.gameObject.GetComponent<BoxCollider>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (base.gameObject.activeSelf && other.tag != "TriggerStart" && boxCollider.enabled && other.tag != "TriggerAI")
		{
			StartCoroutine("CarOnBackTimer");
		}
	}

	private void OnTriggerExit(Collider other)
	{
		StopCoroutine("CarOnBackTimer");
	}

	private IEnumerator CarOnBackTimer()
	{
		float tmpTimer = 0f;
		while (tmpTimer < 2f)
		{
			if (!b_Pause)
			{
				tmpTimer = Mathf.MoveTowards(tmpTimer, 2f, Time.deltaTime);
			}
			yield return null;
		}
		carController.RespawnTheCar();
	}

	private void Pause()
	{
		b_Pause = true;
	}
}
