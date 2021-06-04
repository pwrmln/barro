using System.Collections;
using UnityEngine;

public class Boost : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer[] meshRenderers;

	private void Start()
	{
		StartCoroutine(ChangeImage());
	}

	private IEnumerator ChangeImage()
	{
		WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);
		while (true)
		{
			meshRenderers[0].enabled = !meshRenderers[0].enabled;
			meshRenderers[1].enabled = !meshRenderers[0].enabled;
			yield return waitForSeconds;
		}
	}
}
