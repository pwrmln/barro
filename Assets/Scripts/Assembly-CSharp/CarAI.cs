using System.Collections;
using SCJogos.Database;
using UnityEngine;

public class CarAI : MonoBehaviour
{
	public bool SeeInspector;

	public LayerMask myLayerMask;

	public bool b_Pause;

	public GameObject Target;

	public CarController carController;

	public int CarAIEulerRotation = 70;

	public string Obstacles = "Null";

	public float smoothAvoid;

	public bool b_DisableRotation;

	private bool CheckCarVelocity;

	public bool CarMoveForward = true;

	public bool b_endBackward;

	public float carWaitBeforeBackwardDuration = 2f;

	public float carBackwardDuration = 1f;

	public int HowManyTimeCarCantMove;

	public GameObject obj_pivotCheckCarCollisionRearRight;

	public GameObject obj_pivotCheckCarCollisionFrontRight;

	public bool IsTouchingCar_OnHisRight;

	public float WaitBeforeTakingDecision;

	public GameObject SecondCarOnRight;

	public bool allowRandomValues = true;

	public int successRate_BestTargetPos = 100;

	public float TargetPosMin = -0.15f;

	public float TargetPosMax = 0.15f;

	public int successRate_BestOffsetSpeed = 100;

	public float offsetSpeedMin = -0.15f;

	private float offsetSpeedMax;

	public int successRate_BestOffsetRandomEulerAngle = 100;

	public float offsetRandomEulerAngleMin = -10f;

	private float offsetRandomEulerAngleMax;

	public float offsetRandomEulerAngle;

	public float angle;

	public bool b_contactWithOtherCar;

	private float impact;

	private DifficultyManager difficultyManager;

	public float ObstacleDistance;

	private float raycastLength = 6f;

	public AnimationCurve curveOvaidObstacle;

	public float angleObstacle;

	private void Start()
	{
		difficultyManager = Object.FindObjectOfType<DifficultyManager>();
		carController = GetComponent<CarController>();
		Target = GameObject.Find("P" + carController.playerNumber + "_Target_Part2");
		initRandomValue();
	}

	private void FixedUpdate()
	{
		if (!carController.b_CarIsRespawning && !carController.b_CountdownActivate)
		{
			CheckObstacles();
			TurnLeftAndRight();
			Acceleration();
			CheckCarLocalVelocity();
		}
		else if (carController.b_btn_Acce || carController.b_btn_Break)
		{
			carController.Btn_AccelerationDeactivate();
		}
	}

	public void CheckObstacles()
	{
		Obstacles = "Null";
		Vector3 position = carController.frontCapsuleCollider.gameObject.transform.position;
		Vector3 forward = carController.frontCapsuleCollider.gameObject.transform.forward;
		Vector3 vector = -carController.frontCapsuleCollider.gameObject.transform.forward + carController.frontCapsuleCollider.gameObject.transform.right * 0.4f;
		Vector3 vector2 = -carController.frontCapsuleCollider.gameObject.transform.forward - carController.frontCapsuleCollider.gameObject.transform.right * 0.4f;
		Vector3 position2 = carController.RayCastWheels[0].gameObject.transform.position;
		Vector3 position3 = carController.RayCastWheels[1].gameObject.transform.position;
		RaycastHit hitInfo;
		if (Physics.Raycast(position, forward, out hitInfo, raycastLength, myLayerMask))
		{
			Obstacles = "Right";
			ObstacleDistance = hitInfo.distance;
		}
		else if (Physics.Raycast(position3, forward, out hitInfo, raycastLength * 0.5f, myLayerMask))
		{
			Obstacles = "Left";
			ObstacleDistance = hitInfo.distance;
		}
		else if (Physics.Raycast(position2, forward, out hitInfo, raycastLength * 0.5f, myLayerMask))
		{
			Obstacles = "Right";
			ObstacleDistance = hitInfo.distance;
		}
		if (Physics.Raycast(position2, -vector2, out hitInfo, raycastLength * 0.7f, myLayerMask))
		{
			Obstacles = "Right";
			ObstacleDistance = hitInfo.distance;
		}
		if (Physics.Raycast(position3, -vector, out hitInfo, raycastLength * 0.7f, myLayerMask))
		{
			Obstacles = "Left";
			ObstacleDistance = hitInfo.distance;
		}
		Debug.DrawLine(position, position + forward * raycastLength, Color.green);
		Debug.DrawLine(position3, position3 + forward * raycastLength * 0.5f, Color.green);
		Debug.DrawLine(position2, position2 + forward * raycastLength * 0.5f, Color.green);
		Debug.DrawLine(position3, position3 - vector * raycastLength * 0.7f, Color.green);
		Debug.DrawLine(position2, position2 - vector2 * raycastLength * 0.7f, Color.green);
		Vector3 vector3 = -obj_pivotCheckCarCollisionFrontRight.transform.forward + obj_pivotCheckCarCollisionFrontRight.gameObject.transform.right * 0.2f;
		Vector3 vector4 = -obj_pivotCheckCarCollisionRearRight.transform.forward - obj_pivotCheckCarCollisionRearRight.transform.right * 0.2f;
		if (Physics.Raycast(obj_pivotCheckCarCollisionFrontRight.transform.position, vector3, out hitInfo, 0.450000018f) && hitInfo.collider.tag == "Car" && Physics.Raycast(obj_pivotCheckCarCollisionRearRight.transform.position, -vector4, out hitInfo, 0.450000018f) && hitInfo.collider.tag == "Car")
		{
			IsTouchingCar_OnHisRight = true;
			if (WaitBeforeTakingDecision == 0f)
			{
				SecondCarOnRight = hitInfo.collider.gameObject;
				StartCoroutine("Take_Decision_Because_Car_Touch_Other_Car_On_His_Right");
			}
		}
		else if (IsTouchingCar_OnHisRight)
		{
			IsTouchingCar_OnHisRight = false;
		}
		Debug.DrawLine(obj_pivotCheckCarCollisionFrontRight.transform.position, obj_pivotCheckCarCollisionFrontRight.transform.position + vector3 * raycastLength * 0.15f, Color.yellow);
		Debug.DrawLine(obj_pivotCheckCarCollisionRearRight.transform.position, obj_pivotCheckCarCollisionRearRight.transform.position - vector4 * raycastLength * 0.15f, Color.yellow);
	}

	public void TurnLeftAndRight()
	{
		Vector3 position = carController.frontCapsuleCollider.gameObject.transform.position;
		Vector3 targetDir = Target.transform.position - position;
		angle = Vector2.Angle(new Vector2(targetDir.x, targetDir.z), new Vector2(base.transform.forward.x, base.transform.forward.z));
		float num = AngleDir(base.transform.forward, targetDir, base.transform.up);
		float num2 = angle / 15f;
		if (b_contactWithOtherCar)
		{
			return;
		}
		if (!b_DisableRotation)
		{
			if (Obstacles == "Null")
			{
				if (num == 1f && Mathf.Abs(angle) > 2f)
				{
					carController.Btn_RightActivate();
					carController.Btn_LeftDeactivate();
				}
				else if (num == -1f && Mathf.Abs(angle) > 2f)
				{
					carController.Btn_RightDeactivate();
					carController.Btn_LeftActivate();
				}
				else
				{
					carController.Btn_RightDeactivate();
					carController.Btn_LeftDeactivate();
				}
			}
			else if (Obstacles == "Left")
			{
				carController.Btn_RightActivate();
				carController.Btn_LeftDeactivate();
			}
			else if (Obstacles == "Right")
			{
				carController.Btn_LeftActivate();
				carController.Btn_RightDeactivate();
			}
		}
		else
		{
			if (Obstacles == "Right")
			{
				carController.Btn_RightActivate();
				carController.Btn_LeftDeactivate();
			}
			else
			{
				carController.Btn_RightDeactivate();
				carController.Btn_LeftActivate();
			}
			num2 = 2f;
		}
		if (Obstacles == "Null")
		{
			smoothAvoid = Mathf.MoveTowards(smoothAvoid, 1f, Time.fixedDeltaTime * 0.5f);
			carController.eulerAngleVelocity.y = ((float)CarAIEulerRotation + offsetRandomEulerAngle) * num2 * smoothAvoid;
		}
		else
		{
			smoothAvoid = 0f;
			carController.eulerAngleVelocity.y = ((float)CarAIEulerRotation + offsetRandomEulerAngle) * 9f * curveOvaidObstacle.Evaluate(1f - ObstacleDistance * 1f / raycastLength);
		}
	}

	private float AngleDir(Vector3 _forward, Vector3 _targetDir, Vector3 _up)
	{
		Vector3 lhs = Vector3.Cross(_forward, _targetDir);
		float num = Vector3.Dot(lhs, _up);
		if (num > 0f)
		{
			return 1f;
		}
		if (num < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	public void Acceleration()
	{
		if (!b_Pause && !b_contactWithOtherCar)
		{
			if (CarMoveForward)
			{
				b_DisableRotation = false;
				carController.b_btn_Acce = true;
				carController.b_btn_Break = false;
			}
			else
			{
				b_DisableRotation = true;
				carController.b_btn_Acce = false;
				carController.b_btn_Break = true;
			}
		}
	}

	public void CheckCarLocalVelocity()
	{
		if (!b_Pause && !carController.raceIsFinished)
		{
			if ((double)carController._localVelovity() < 0.2 && !CheckCarVelocity && !b_endBackward && !carController.b_CarIsRespawning)
			{
				StartCoroutine("WaitBeforeGoBackward");
			}
			else if (carController._localVelovity() >= Mathf.Abs(0.2f) && CheckCarVelocity)
			{
				StopCoroutine("WaitBeforeGoBackward");
				CarMoveForward = true;
				b_DisableRotation = false;
				b_endBackward = false;
			}
		}
	}

	public void initRandomValue()
	{
		if (difficultyManager != null && carController != null && carController.playerNumber != 1)
		{
			int index = 0;
			if (BARDataManager.Instance.BARConfig.Difficulty == DifficultyType.Medium)
			{
				index = 1;
			}
			else if (BARDataManager.Instance.BARConfig.Difficulty == DifficultyType.Expert)
			{
				index = 2;
			}
			carController.offsetSpeedDifficultyManager = difficultyManager.difficulties[index].addGlobalSpeedOffset[carController.playerNumber - 2];
			successRate_BestTargetPos = difficultyManager.difficulties[index].waypointSuccesRate[carController.playerNumber - 2];
			TargetPosMin = difficultyManager.difficulties[index].waypointMinTarget[carController.playerNumber - 2];
			TargetPosMax = difficultyManager.difficulties[index].waypointMaxTarget[carController.playerNumber - 2];
			successRate_BestOffsetSpeed = difficultyManager.difficulties[index].speedSuccesRate[carController.playerNumber - 2];
			offsetSpeedMin = difficultyManager.difficulties[index].speedOffset[carController.playerNumber - 2];
			successRate_BestOffsetRandomEulerAngle = difficultyManager.difficulties[index].rotationSuccesRate[carController.playerNumber - 2];
			offsetRandomEulerAngleMin = difficultyManager.difficulties[index].rotationOffset[carController.playerNumber - 2];
			F_RandomCarValues();
		}
	}

	public void F_RandomCarValues()
	{
		if (allowRandomValues)
		{
			float num = Random.Range(0, 101);
			if ((float)successRate_BestTargetPos < num)
			{
				num = Random.Range(TargetPosMin, TargetPosMax);
				Target.transform.localPosition = new Vector3(num, 0f, 0f);
			}
			else
			{
				Target.transform.localPosition = new Vector3(0f, 0f, 0f);
			}
			num = Random.Range(0, 101);
			if ((float)successRate_BestOffsetSpeed < num)
			{
				num = Random.Range(offsetSpeedMin, offsetSpeedMax);
				carController.randomSpeedOffset = num;
			}
			else
			{
				carController.randomSpeedOffset = 0f;
			}
			num = Random.Range(0, 101);
			if ((float)successRate_BestOffsetRandomEulerAngle < num)
			{
				num = (offsetRandomEulerAngle = Random.Range(offsetRandomEulerAngleMin, offsetRandomEulerAngleMax));
			}
			else
			{
				offsetRandomEulerAngle = 0f;
			}
		}
	}

	private IEnumerator Take_Decision_Because_Car_Touch_Other_Car_On_His_Right()
	{
		WaitBeforeTakingDecision = 0f;
		while (WaitBeforeTakingDecision < 1f)
		{
			if (!b_Pause)
			{
				WaitBeforeTakingDecision = Mathf.MoveTowards(WaitBeforeTakingDecision, 1f, Time.fixedDeltaTime);
			}
			yield return null;
		}
		if (allowRandomValues)
		{
			Target.transform.localPosition = new Vector3(-0.15f, 0f, 0f);
			SecondCarOnRight.GetComponent<CheckOtherCarCollision>().ChangeCarTargetPosition(0.15f);
		}
		float refForce = carController.Force;
		while (WaitBeforeTakingDecision < 4f)
		{
			if (!b_Pause)
			{
				WaitBeforeTakingDecision = Mathf.MoveTowards(WaitBeforeTakingDecision, 4f, Time.fixedDeltaTime);
				carController.Force = Mathf.MoveTowards(carController.Force, refForce - 80f, Time.fixedDeltaTime * 80f);
			}
			yield return null;
		}
		carController.Force = refForce;
		if (allowRandomValues)
		{
			Target.transform.localPosition = new Vector3(0f, 0f, 0f);
			SecondCarOnRight.GetComponent<CheckOtherCarCollision>().ChangeCarTargetPosition(0f);
		}
		WaitBeforeTakingDecision = 0f;
	}

	public void StopCo()
	{
		StopAllCoroutines();
	}

	private IEnumerator WaitBeforeGoBackward()
	{
		float tmpTimer2 = 0f;
		b_endBackward = true;
		while (tmpTimer2 < carWaitBeforeBackwardDuration)
		{
			if (!b_Pause)
			{
				tmpTimer2 = Mathf.MoveTowards(tmpTimer2, carWaitBeforeBackwardDuration, Time.fixedDeltaTime);
			}
			yield return null;
		}
		if (HowManyTimeCarCantMove == 3)
		{
			carController.RespawnTheCar();
			HowManyTimeCarCantMove = 0;
			CarMoveForward = true;
			b_DisableRotation = false;
			b_endBackward = false;
			yield break;
		}
		if (carController._localVelovity() >= Mathf.Abs(0.2f))
		{
			HowManyTimeCarCantMove = 0;
		}
		else
		{
			HowManyTimeCarCantMove++;
			tmpTimer2 = 0f;
			CarMoveForward = false;
			while (tmpTimer2 < carBackwardDuration)
			{
				if (!b_Pause)
				{
					tmpTimer2 = Mathf.MoveTowards(tmpTimer2, carBackwardDuration, Time.fixedDeltaTime);
				}
				yield return null;
			}
		}
		CarMoveForward = true;
		b_DisableRotation = false;
		b_endBackward = false;
	}

	public void Pause()
	{
		b_Pause = !b_Pause;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.relativeVelocity.magnitude > 1.5f && collision.gameObject.CompareTag("Car") && !b_contactWithOtherCar && !collision.gameObject.GetComponent<CarAI>().isActiveAndEnabled)
		{
			b_contactWithOtherCar = true;
			carController.b_btn_Acce = false;
			carController.b_btn_Break = false;
			impact = collision.relativeVelocity.magnitude;
			StartCoroutine("WaitAfterContact");
		}
	}

	private IEnumerator WaitAfterContact()
	{
		float tmpTimer = 0f;
		if (impact > 5f)
		{
			impact = 1f;
		}
		else
		{
			impact = 0.5f;
		}
		while (tmpTimer < impact)
		{
			if (!b_Pause)
			{
				tmpTimer = Mathf.MoveTowards(tmpTimer, 1f, Time.fixedDeltaTime);
			}
			yield return null;
		}
		b_contactWithOtherCar = false;
	}
}
