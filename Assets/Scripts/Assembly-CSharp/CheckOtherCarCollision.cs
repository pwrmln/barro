using UnityEngine;

public class CheckOtherCarCollision : MonoBehaviour
{
	public CarAI car;

	public GameObject CarTarget;

	public void ChangeCarTargetPosition(float NewPosX)
	{
		if ((bool)CarTarget)
		{
			CarTarget.transform.localPosition = new Vector3(NewPosX, 0f, 0f);
		}
	}
}
