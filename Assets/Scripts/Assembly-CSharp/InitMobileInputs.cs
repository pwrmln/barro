using UnityEngine;

public class InitMobileInputs : MonoBehaviour
{
	public CarController carController;

	public void F_InitMobileButtons(GameObject car)
	{
		carController = car.GetComponent<CarController>();
	}
}
