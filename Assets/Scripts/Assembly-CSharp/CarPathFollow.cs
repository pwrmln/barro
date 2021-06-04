using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;



public class CarPathFollow : MonoBehaviour
{
	public Transform target;

	public float progressDistance;

	public WaypointCircuit Track;

	public int iLapCounter = 1;

	private bool pathExist;

	private LapCounter sLapCounter;

	private CarPathFollow carPathFollow;

	private Vector3 progressDelta;

	private int lastWaypoint;

	private float dist;

	private float oldDist;

	private float timeCheck;

	private int oldWaypoint = -1;

	private int qtdWrong;

	private int playerIndex;

	private Image imageWrongWay;

	private CarController carController;

	public WaypointCircuit.RoutePoint progressPoint { get; private set; }

	public GameObject LastWaypoint
	{
		get
		{
			return Track.waypointList.items[lastWaypoint].gameObject;
		}
	}

	private IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		carController = GetComponent<CarController>();
		GameObject tmpObj3 = GameObject.Find("P" + base.transform.GetComponent<CarController>().playerNumber + "_Target");
		if ((bool)tmpObj3)
		{
			target = tmpObj3.transform;
		}
		tmpObj3 = GameObject.Find("Track_Path");
		if ((bool)tmpObj3)
		{
			Track = tmpObj3.GetComponent<WaypointCircuit>();
		}
		if (Track != null && Track.waypointList.items.Length > 0)
		{
			pathExist = true;
		}
		tmpObj3 = GameObject.Find("StartLine_lapCounter");
		if ((bool)tmpObj3)
		{
			sLapCounter = tmpObj3.GetComponent<LapCounter>();
		}
		carPathFollow = GetComponent<CarPathFollow>();
		GameObject go = null;
		if (base.gameObject.name.Contains("P1_"))
		{
			playerIndex = 1;
			go = GameObject.Find("P1_Cam");
		}
		else if (base.gameObject.name.Contains("P2_") && !GetComponent<CarAI>().enabled)
		{
			playerIndex = 2;
			go = GameObject.Find("P2_Cam ");
			if (!go)
			{
				go = GameObject.Find("P2_Cam");
			}
		}
		else
		{
			playerIndex = 0;
		}
		if ((bool)go)
		{
			imageWrongWay = go.GetComponentInChildren<Image>();
		}
	}

	private void Update()
	{
		if (Track != null && target != null && pathExist)
		{
			target.position = Track.GetRoutePoint(progressDistance + 1.45f, out lastWaypoint).position;
			progressPoint = Track.GetRoutePoint(progressDistance, out lastWaypoint);
			target.rotation = Quaternion.LookRotation(progressPoint.direction);
			if (playerIndex > 0 && (bool)imageWrongWay && Time.time > timeCheck)
			{
				timeCheck = Time.time + 0.1f;
				StartCoroutine(CheckWrongWay());
			}
			progressDelta = progressPoint.position - base.transform.position;
			if (Vector3.Dot(progressDelta, progressPoint.direction) < 0f)
			{
				progressDistance += progressDelta.magnitude * 0.5f;
			}
		}
		if (Track != null && progressDistance / Track.Length > 1f)
		{
			iLapCounter++;
			if ((bool)sLapCounter)
			{
				sLapCounter.displayLap(carPathFollow);
			}
			progressDistance %= Track.Length;
		}
	}

	private IEnumerator CheckWrongWay()
	{
		yield return null;
		dist = Vector3.Distance(base.transform.position, target.position);
		if (lastWaypoint == oldWaypoint)
		{
			if (dist > oldDist)
			{
				qtdWrong++;
			}
			else
			{
				qtdWrong = 0;
			}
			if ((imageWrongWay.transform.eulerAngles.z < -90f && imageWrongWay.transform.eulerAngles.z > -270f) || (imageWrongWay.transform.eulerAngles.z > 90f && imageWrongWay.transform.eulerAngles.z < 270f) || qtdWrong > 15)
			{
				imageWrongWay.transform.LookAt(target.position);
				imageWrongWay.transform.localRotation = Quaternion.Euler(0f, 0f, 0f - imageWrongWay.transform.localEulerAngles.y);
				if (!imageWrongWay.enabled)
				{
					imageWrongWay.enabled = true;
				}
				if (dist > 20f)
				{
					carController.RespawnTheCar();
				}
			}
			else if (imageWrongWay.enabled)
			{
				imageWrongWay.enabled = false;
			}
		}
		oldDist = dist;
		oldWaypoint = lastWaypoint;
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying && Track != null && target != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(base.transform.position, target.position);
		}
	}
}