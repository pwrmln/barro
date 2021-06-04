using UnityEngine;

public class Cam_Follow : MonoBehaviour
{
	public Transform target;

	public float distance = 10f;

	public float height = 5f;

	public float rotationDamping;

	public float heightDamping;

	public bool b_Pause;

	public bool b_Find_Target_Automatically;

	[Space]
	[Header("Screenshots")]
	[SerializeField]
	private int screenWidth = 1366;

	[SerializeField]
	private int screenHeight = 768;

	[SerializeField]
	private bool skybox;

	[SerializeField]
	private bool fog;

	public float OffsetDistance { get; set; }

	private void Start()
	{
		Camera component = GetComponent<Camera>();
		if (component != null)
		{
			component.fieldOfView = 100f;
			component.nearClipPlane = 0.01f;
		}
		distance = 1.5f;
		height = 0.75f;
		rotationDamping = 3f;
		OffsetDistance = 0f;
	}

	private void FixedUpdate()
	{
		if (!b_Pause && (bool)target)
		{
			float y = target.eulerAngles.y;
			float b = target.position.y + height;
			RaycastHit hitInfo;
			if (Physics.Raycast(base.transform.position, Vector3.down, out hitInfo, 1000f))
			{
				b = Mathf.Max(hitInfo.point.y + 0.2f, b);
			}
			float y2 = base.transform.eulerAngles.y;
			float y3 = base.transform.position.y;
			y2 = Mathf.LerpAngle(y2, y, rotationDamping * Time.fixedDeltaTime);
			y3 = Mathf.Lerp(y3, b, heightDamping * Time.fixedDeltaTime);
			Quaternion quaternion = Quaternion.Euler(0f, y2, 0f);
			base.transform.position = target.position;
			base.transform.position -= quaternion * Vector3.forward * (distance + OffsetDistance);
			base.transform.position = new Vector3(base.transform.position.x, y3, base.transform.position.z);
			base.transform.LookAt(target.position + target.forward * 0.3f);
		}
	}

	public void InitCamera(CarController car, bool b_Splitscreen)
	{
		target = car.camTarget.transform;
		Vector3 position = target.position + target.forward * (0f - distance);
		position.y += height;
		base.transform.position = position;
		base.transform.LookAt(target);
		Camera component = GetComponent<Camera>();
		component.nearClipPlane = 0.01f;
		if (b_Splitscreen)
		{
			component.rect = new Rect(new Vector2(0f, 0f), new Vector2(0.5f, 1f));
		}
		MirrorController componentInChildren = GetComponentInChildren<MirrorController>();
		if ((bool)componentInChildren && (car.playerNumber == 1 || (car.playerNumber == 2 && car.carAI != null && !car.carAI.enabled)))
		{
			componentInChildren.SetCamera(car);
		}
	}

	public void Pause()
	{
		b_Pause = !b_Pause;
	}
}
