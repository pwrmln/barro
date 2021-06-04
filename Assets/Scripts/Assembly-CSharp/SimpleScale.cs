using UnityEngine;

public class SimpleScale : MonoBehaviour
{
	public int speed = 2;

	public Vector3 dir = new Vector3(0f, 1f, 0f);

	public AnimationCurve curveX = new AnimationCurve();

	public AnimationCurve curveY = new AnimationCurve();

	public float tmpTime;

	public RectTransform rectTransform;

	public bool startAnimation;

	private void Start()
	{
	}

	private void Update()
	{
		if (startAnimation)
		{
			tmpTime = Mathf.MoveTowards(tmpTime, 1f, Time.deltaTime * (float)speed);
			rectTransform.localScale = new Vector3(curveX.Evaluate(tmpTime), curveY.Evaluate(tmpTime), 1f);
			if (tmpTime == 1f)
			{
				startAnimation = false;
			}
		}
	}
}
