using UnityEngine;

public class gizmosName : MonoBehaviour
{
	public float _Size = 0.25f;

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 0.092f, 0.016f, 0.5f);
		Gizmos.DrawSphere(base.transform.position, _Size);
	}
}
