using UnityEngine;
using UnityEngine.EventSystems;

public class MobileButtons : MonoBehaviour
{
	public InitMobileInputs grpInputs;

	private CarController carController;

	public bool btn_Left;

	public bool btn_Right;

	public bool btn_Accelerate;

	public bool btn_Break;

	public bool btn_Respawn;

	private void Start()
	{
		EventTrigger component = GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener(delegate(BaseEventData data)
		{
			OnPointerDownDelegate((PointerEventData)data);
		});
		component.triggers.Add(entry);
		EventTrigger.Entry entry2 = new EventTrigger.Entry();
		entry2.eventID = EventTriggerType.PointerUp;
		entry2.callback.AddListener(delegate(BaseEventData data)
		{
			OnPointerUpDelegate((PointerEventData)data);
		});
		component.triggers.Add(entry2);
	}

	public void OnPointerDownDelegate(PointerEventData data)
	{
		if (carController == null)
		{
			carController = grpInputs.carController;
		}
		if (btn_Left)
		{
			carController.Btn_LeftActivate();
		}
		if (btn_Right)
		{
			carController.Btn_RightActivate();
		}
		if (btn_Accelerate)
		{
			carController.Btn_AccelerationActivate();
		}
		if (btn_Break)
		{
			carController.Btn_BreakActivate();
		}
		if (btn_Respawn)
		{
			carController.Btn_Respawn();
		}
	}

	public void OnPointerUpDelegate(PointerEventData data)
	{
		if (carController == null)
		{
			carController = grpInputs.carController;
		}
		if (btn_Left)
		{
			carController.Btn_LeftDeactivate();
		}
		if (btn_Right)
		{
			carController.Btn_RightDeactivate();
		}
		if (btn_Accelerate)
		{
			carController.Btn_AccelerationDeactivate();
		}
		if (btn_Break)
		{
			carController.Btn_BreakDeactivate();
		}
	}
}
