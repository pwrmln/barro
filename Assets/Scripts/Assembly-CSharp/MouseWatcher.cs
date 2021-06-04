using UnityEngine;

public class MouseWatcher : MonoBehaviour
{
	private float mouseIdle;

	private Vector3 prevMousePos;

	private void Update()
	{
		if (Input.mousePosition != prevMousePos || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
		{
			mouseIdle = Time.time + 1f;
			prevMousePos = Input.mousePosition;
		}
		if (Time.time > mouseIdle)
		{
			if (Cursor.visible)
			{
				Cursor.visible = false;
			}
		}
		else if (!Cursor.visible)
		{
			Cursor.visible = true;
		}
	}
}
