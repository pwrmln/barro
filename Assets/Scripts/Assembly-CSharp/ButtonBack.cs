using UnityEngine;
using UnityEngine.UI;

public class ButtonBack : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetButtonDown("Back"))
		{
			GetComponent<Button>().onClick.Invoke();
		}
	}
}
