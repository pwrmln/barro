using UnityEngine;

public class MobileInitAfterPause : MonoBehaviour
{
	public GameObject MobileInputCanvas;

	private bool b_MobileInputActivated;

	private void Start()
	{
		if ((bool)MobileInputCanvas && MobileInputCanvas.activeSelf)
		{
			b_MobileInputActivated = true;
		}
	}

	public void ActivateMobileInput()
	{
		if ((bool)MobileInputCanvas && b_MobileInputActivated)
		{
			MobileInputCanvas.SetActive(true);
		}
	}
}
