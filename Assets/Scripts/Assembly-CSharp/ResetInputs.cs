using UnityEngine;

public class ResetInputs : MonoBehaviour
{
	public void ResetData()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}
}
