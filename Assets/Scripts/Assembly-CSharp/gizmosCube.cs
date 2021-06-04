using UnityEngine;

public class gizmosCube : MonoBehaviour
{
	public Color GizmoColor = new Color(0f, 0.9f, 1f, 0.5f);

	private void OnDrawGizmos()
	{
		Gizmos.color = GizmoColor;
		Matrix4x4 matrix4x2 = (Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, base.transform.localScale));
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}
}
