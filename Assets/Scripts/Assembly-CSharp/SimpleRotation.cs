using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
	public int speed = 10;

	public Vector3 dir = new Vector3(0f, 1f, 0f);

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(dir * Time.deltaTime * speed, Space.World);
	}
}
