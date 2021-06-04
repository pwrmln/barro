using System.Collections;
using System.Collections.Generic;
using Core.Audio;
using SCJogos.Database;
using SCJogos.Steamworks;
using UnityEngine;

public class CarController : MonoBehaviour
{
	[SerializeField]
	private Camera mirrorCamera;

	public bool SeeInspector;

	public LayerMask myLayerMask;

	public bool b_Pause;

	public AnimationCurve curveRotation;

	public AnimationCurve curveAcceleration;

	public int playerNumber;

	public float MaxSpeed = 3f;

	public float offsetSpeedDifficultyManager;

	public float offsetSpeedForMobile;

	public float CarRotationSpeed = 1.5f;

	public float offsetRotationForMobile;

	private float Speed;

	private Rigidbody rb;

	public float springConstant;

	public float damperConstant;

	public float SpringHeight = 0.11f;

	public float restLength;

	public float dis = 0.2f;

	public float offsetWheelFront = 1f;

	public float offsetWheelRear = 1f;

	public Vector3 eulerAngleVelocity;

	public float Force = 120f;

	public float BrakeForce = 35f;

	public Transform t_ApplyForce;

	public float Coeff = 0.15f;

	public float refCoeffZ_Min = 0.5f;

	public float refCoeffZ_Max = 1.5f;

	public float CoeffZWhenCarIsSlow = 10f;

	public float CoeffZ = 1.5f;

	public Vector3 com;

	private float DirBackward_ = 1f;

	public int NumberOfWheelThatTouchGround;

	public float LenghtWheelsDistance = 0.127f;

	public float RearWheelsDistance = 0.07f;

	public float FrontWheelsDistance = 0.07f;

	public GameObject[] RayCastWheels;

	private float[] springforce2 = new float[4];

	private float[] damperForce2 = new float[4];

	private float[] previous_Length2 = new float[4];

	private float[] current_Length2 = new float[4];

	private float[] springVelocity2 = new float[4];

	public GameObject rearCapsuleCollider;

	public GameObject frontCapsuleCollider;

	public float PicWheelSize = 0.19f;

	public float WheelSizeRear;

	public float WheelSizeFront;

	public GameObject[] Wheel_X_Rotate = new GameObject[4];

	public List<float> Wheel_X_RefLocalPosition = new List<float>();

	public GameObject[] Wheel_Z_Rotate = new GameObject[4];

	public float[] tmpAngle = new float[4];

	public AudioSource audio_;

	public float MaxAudioPitch = 0.9f;

	public AudioSource objSkid_Sound;

	public AudioSource obj_CarImpact_Sound;

	public float impactVolumeMax = 0.3f;

	private bool Once;

	public bool b_Grounded_Audio;

	public float tmpmulti;

	public float tmprotate;

	public float Input_Acceleration;

	public bool b_btn_Acce;

	public bool b_btn_Break;

	public float btn_Rotation;

	public bool b_btn_Left;

	public bool b_btn_Right;

	public bool b_RespawnMobile;

	public GameObject CarBodyCollider;

	public CarPathFollow carPathFollow;

	private float timerBeforeRespawn;

	public GameObject RespawnCloud;

	public bool b_CarIsRespawning;

	public float DurationBeforeCarRespawn = 1f;

	public Transform objRespawn;

	public GameObject Capsule_Rear;

	public GameObject Capsule_Front;

	public GameObject BodyCollider;

	public Collider ColliderCarOnBack;

	public GameObject Grp_BodyPlusBlobShadow;

	public Transform pivotCarSelection;

	public float pivotOffsetZ;

	public bool b_AutoAcceleration;

	public bool b_random;

	public bool b_allowRandomCarValue = true;

	public CarAI carAI;

	public float randomSpeedOffset;

	public bool b_CarAccelerate;

	public float tmpFakeBodyRotation;

	public float BodyRotationValue = 5f;

	public bool StopAcceleration;

	private float Slide_01 = 1f;

	private float Slide_02;

	private float Slide_03;

	public AnimationCurve curve_01;

	public float ReachMaxRotation;

	public float ReachMaxRotationAcc;

	public int turnDirection;

	public bool b_CarMoveForward = true;

	public float t;

	public bool raceIsFinished;

	private Game_Manager gameManager;

	public bool b_CountdownActivate;

	public bool b_MaxAccelerationAfterCountdown = true;

	private Vector3 tmpVelocity;

	public GameObject camTarget;

	[SerializeField]
	private GameObject fullBody;

	public bool b_InitInputWhenGameStart = true;

	private float vacumTime;

	private float dragOffset;

	private bool boost;

	private int respawns;

	private float tempForce;

	private float backRotation;

	private float aiCrashDelay;

	public Camera MirrorCamera
	{
		get
		{
			return mirrorCamera;
		}
	}

	public void SetLightsOn(bool on)
	{
		GetComponentInChildren<Light>().enabled = on;
	}

	public void StartRace()
	{
		b_CountdownActivate = false;
		rb.isKinematic = false;
		audio_.volume = 0.7f;
	}

	private void Awake()
	{
		for (int i = 0; i < Wheel_X_Rotate.Length; i++)
		{
			Wheel_X_RefLocalPosition.Add(Wheel_X_Rotate[i].transform.localPosition.y);
		}
		GameObject gameObject = GameObject.Find("Game_Manager");
		if ((bool)gameObject)
		{
			gameManager = gameObject.GetComponent<Game_Manager>();
			b_CountdownActivate = gameManager.b_UseCountdown;
		}
		carAI = GetComponent<CarAI>();
		audio_.outputAudioMixerGroup = AudioManager.Instance.AudioMixerSFX;
		audio_.volume = 0.7f;
		objSkid_Sound.outputAudioMixerGroup = AudioManager.Instance.AudioMixerSFX;
		objSkid_Sound.volume = 1f;
		obj_CarImpact_Sound.outputAudioMixerGroup = AudioManager.Instance.AudioMixerSFX;
		obj_CarImpact_Sound.volume = 1f;
	}

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		audio_ = audio_.gameObject.GetComponent<AudioSource>();
		objSkid_Sound = objSkid_Sound.gameObject.GetComponent<AudioSource>();
		obj_CarImpact_Sound = obj_CarImpact_Sound.gameObject.GetComponent<AudioSource>();
		if (carAI.enabled)
		{
			b_AutoAcceleration = true;
		}
		InitCarAudioSource();
		SetLightsOn(gameManager.Lights);
		vacumTime = Time.time + 10f;
		tempForce = Force;
	}

	private IEnumerator StartRigidbody()
	{
		yield return new WaitForSeconds(1f);
		if (b_CountdownActivate)
		{
			rb.isKinematic = true;
		}
	}

	public void InitCarAudioSource()
	{
		if (GameObject.FindGameObjectWithTag("CameraMainMenu") == null)
		{
			audio_.mute = false;
		}
		if (!carAI.enabled)
		{
			if (BARDataManager.Instance.BARConfig.PlayersNumber == 2)
			{
				audio_.spatialBlend = 0f;
				objSkid_Sound.spatialBlend = 0f;
				obj_CarImpact_Sound.spatialBlend = 0f;
			}
			else
			{
				audio_.spatialBlend = 0.25f;
				objSkid_Sound.spatialBlend = 0f;
				obj_CarImpact_Sound.spatialBlend = 0.5f;
			}
		}
		else
		{
			audio_.spatialBlend = 1f;
			objSkid_Sound.spatialBlend = 1f;
			obj_CarImpact_Sound.spatialBlend = 1f;
			audio_.volume = 0f;
		}
	}

	public void Btn_AccelerationActivate()
	{
		b_btn_Acce = true;
		b_btn_Break = false;
	}

	public void Btn_AccelerationDeactivate()
	{
		b_btn_Acce = false;
		b_btn_Break = false;
	}

	public void Btn_BreakActivate()
	{
		b_btn_Break = true;
	}

	public void Btn_BreakDeactivate()
	{
		b_btn_Acce = false;
		b_btn_Break = false;
	}

	public void Btn_LeftActivate()
	{
		b_btn_Left = true;
		b_btn_Right = false;
	}

	public void Btn_LeftDeactivate()
	{
		b_btn_Left = false;
	}

	public void Btn_RightActivate()
	{
		b_btn_Right = true;
		b_btn_Left = false;
	}

	public void Btn_RightDeactivate()
	{
		b_btn_Right = false;
	}

	public void Btn_Respawn()
	{
		if (!raceIsFinished && !b_CountdownActivate && !b_CarIsRespawning)
		{
			b_RespawnMobile = true;
		}
	}

	private void Update()
	{
		if (!b_AutoAcceleration)
		{
			UpdatePlayerInput();
		}
	}

	private void FixedUpdate()
	{
		if (!b_Pause)
		{
			if (b_AutoAcceleration)
			{
				UpdateAIInput();
			}
			Vector3 localVelocity = rb.transform.InverseTransformDirection(rb.velocity);
			RaycastHit hit = default(RaycastHit);
			UpdateRotation();
			UpdateWheels(localVelocity, hit);
			UpdateRandomReactions(localVelocity, hit);
			if (NumberOfWheelThatTouchGround >= 2)
			{
				UpdateMovement();
				UpdateSkidAudio(localVelocity);
				UpdateWheelRotation();
			}
			UpdateSkidAudioPitch();
			UpdateCarPosition(localVelocity);
			UpdateSpeed();
			UpdateFriction();
			UpdateRespawn();
		}
	}

	private void UpdateRespawn()
	{
		if (b_Pause || raceIsFinished || b_CountdownActivate || b_CarIsRespawning)
		{
			return;
		}
		if (rb.velocity.magnitude < 0.1f && NumberOfWheelThatTouchGround == 0 && !b_AutoAcceleration && timerBeforeRespawn < 3f)
		{
			timerBeforeRespawn = Mathf.MoveTowards(timerBeforeRespawn, 3f, Time.fixedDeltaTime);
			if (timerBeforeRespawn == 3f)
			{
				RespawnTheCar();
			}
		}
		else if (rb.velocity.magnitude < 0.1f && b_AutoAcceleration && timerBeforeRespawn < 3f)
		{
			timerBeforeRespawn = Mathf.MoveTowards(timerBeforeRespawn, 3f, Time.fixedDeltaTime);
			if (timerBeforeRespawn == 3f)
			{
				RespawnTheCar();
			}
		}
		else
		{
			timerBeforeRespawn = 0f;
		}
	}

	private void UpdateFriction()
	{
		if (!b_CarAccelerate && rb.velocity.magnitude < 0.5f)
		{
			CoeffZ = Mathf.MoveTowards(CoeffZ, CoeffZWhenCarIsSlow, Time.fixedDeltaTime * 40f);
		}
	}

	private void UpdateSpeed()
	{
		RaycastHit hitInfo;
		if (boost)
		{
			vacumTime = 1f;
			dragOffset = Mathf.Lerp(dragOffset, 4f, 0.1f);
		}
		else if (Physics.Raycast(base.transform.position, base.transform.forward, out hitInfo, 20f) && hitInfo.collider.CompareTag("Car") && hitInfo.distance > 0.2f)
		{
			if (vacumTime == 0f)
			{
				vacumTime = Time.time + 0.3f;
			}
			else if (Time.time >= vacumTime)
			{
				dragOffset = Mathf.Lerp(dragOffset, 2f * (1f - hitInfo.distance / 20f), 0.01f);
			}
		}
		else
		{
			vacumTime = 0f;
		}
		if (vacumTime == 0f || Time.time < vacumTime)
		{
			if (dragOffset <= 0.01f)
			{
				dragOffset = 0f;
			}
			else
			{
				dragOffset = Mathf.Lerp(dragOffset, 0f, 0.01f);
			}
		}
		if (playerNumber == 1)
		{
			gameManager.cam_P1.OffsetDistance = dragOffset * 0.2f;
		}
		else if (playerNumber == 2)
		{
			gameManager.cam_P2.OffsetDistance = dragOffset * 0.2f;
		}
		if (rb.velocity.magnitude > MaxSpeed + randomSpeedOffset + offsetSpeedDifficultyManager + offsetSpeedForMobile + dragOffset)
		{
			rb.velocity = rb.velocity.normalized * (MaxSpeed + randomSpeedOffset + offsetSpeedDifficultyManager + offsetSpeedForMobile + dragOffset);
		}
	}

	private void UpdateCarPosition(Vector3 localVelocity)
	{
		float num = 1f;
		float num2 = Vector3.Angle(rb.velocity, rb.transform.forward);
		if (Input_Acceleration > 0f)
		{
			DirBackward_ = Mathf.MoveTowards(DirBackward_, 1f, 7f * Time.fixedDeltaTime);
			num = Mathf.Lerp(num, 1f, Time.fixedDeltaTime * 4f);
		}
		else if (Input_Acceleration < 0f)
		{
			DirBackward_ = Mathf.MoveTowards(DirBackward_, -1f, 3f * Time.fixedDeltaTime);
			num = Mathf.Lerp(num, 7f, Time.fixedDeltaTime * 4f);
		}
		else if (num2 >= 90f)
		{
			DirBackward_ = Mathf.MoveTowards(DirBackward_, -1f, 3f * Time.fixedDeltaTime);
			num = Mathf.Lerp(num, 7f, Time.fixedDeltaTime * 4f);
		}
		else if (num2 >= 0f)
		{
			DirBackward_ = Mathf.MoveTowards(DirBackward_, 1f, 7f * Time.fixedDeltaTime);
			num = Mathf.Lerp(num, 1f, Time.fixedDeltaTime * 4f);
		}
		if (Mathf.Abs(localVelocity.z) < 0.4f)
		{
			rb.centerOfMass = new Vector3(com.x, com.y, 0f);
			tmprotate = Mathf.MoveTowards(tmprotate, 0f, Time.fixedDeltaTime * 4f);
		}
		else
		{
			rb.centerOfMass = com;
			tmprotate = Mathf.MoveTowards(tmprotate, 1f, Time.fixedDeltaTime * 4f);
			Wheel_Z_Rotate[0].transform.Rotate(Vector3.right * Time.fixedDeltaTime * 1000f * localVelocity.z);
			Wheel_Z_Rotate[1].transform.Rotate(Vector3.right * Time.fixedDeltaTime * -1000f * localVelocity.z);
			Wheel_Z_Rotate[2].transform.Rotate(Vector3.right * Time.fixedDeltaTime * 1000f * localVelocity.z);
			Wheel_Z_Rotate[3].transform.Rotate(Vector3.right * Time.fixedDeltaTime * -1000f * localVelocity.z);
		}
		float num3 = 1f;
		if (NumberOfWheelThatTouchGround < 1 && b_AutoAcceleration)
		{
			num3 = 0.7f;
		}
		Quaternion quaternion = Quaternion.Euler(eulerAngleVelocity * num * tmprotate * Time.fixedDeltaTime * btn_Rotation * DirBackward_ * num3);
		if (Mathf.Abs(localVelocity.z) > 0.1f && !b_CountdownActivate)
		{
			rb.MoveRotation(rb.rotation * quaternion);
		}
	}

	private void UpdateSkidAudioPitch()
	{
		Speed = rb.velocity.magnitude;
		float target = 0f;
		if (!b_CountdownActivate)
		{
			if (NumberOfWheelThatTouchGround >= 2)
			{
				target = 0.5f + MaxAudioPitch * Speed / (MaxSpeed + randomSpeedOffset + offsetSpeedDifficultyManager + offsetSpeedForMobile);
			}
			else
			{
				target = 0.5f + MaxAudioPitch * Input_Acceleration / (MaxSpeed + randomSpeedOffset + offsetSpeedDifficultyManager + offsetSpeedForMobile);
				if (Once && (bool)objSkid_Sound)
				{
					objSkid_Sound.mute = true;
					Once = false;
				}
			}
		}
		else if (b_CountdownActivate && Input_Acceleration >= 0f)
		{
			target = 0.5f + MaxAudioPitch * MaxSpeed * Input_Acceleration / (MaxSpeed + randomSpeedOffset + offsetSpeedDifficultyManager + offsetSpeedForMobile);
		}
		else if (b_CountdownActivate && Input_Acceleration < 0f)
		{
			target = 0.5f;
		}
		audio_.pitch = Mathf.MoveTowards(audio_.pitch, target, Time.fixedDeltaTime);
	}

	private void UpdateWheelRotation()
	{
		if (btn_Rotation > 0f)
		{
			tmpAngle[1] = Mathf.MoveTowards(tmpAngle[1], 20f, Time.fixedDeltaTime * 100f);
		}
		else if (btn_Rotation < 0f)
		{
			tmpAngle[1] = Mathf.MoveTowards(tmpAngle[1], -20f, Time.fixedDeltaTime * 100f);
		}
		else
		{
			tmpAngle[1] = Mathf.MoveTowards(tmpAngle[1], 0f, Time.fixedDeltaTime * 100f);
		}
		for (int i = 0; i < 2; i++)
		{
			Wheel_X_Rotate[i].transform.localEulerAngles = new Vector3(0f, tmpAngle[1], 0f);
		}
	}

	private void UpdateSkidAudio(Vector3 localVelocity)
	{
		if (btn_Rotation == 1f)
		{
			tmpAngle[0] = Mathf.MoveTowards(tmpAngle[0], 20f, Time.fixedDeltaTime * 100f);
			if (tmpAngle[0] != 20f)
			{
				return;
			}
			CoeffZ = Mathf.MoveTowards(CoeffZ, refCoeffZ_Min, Time.fixedDeltaTime * 40f);
			if (!b_AutoAcceleration)
			{
				if (!Once && (bool)objSkid_Sound && Mathf.Abs(localVelocity.z) > 0.4f)
				{
					objSkid_Sound.mute = false;
					Once = true;
				}
				else if (Once && (bool)objSkid_Sound && Mathf.Abs(localVelocity.z) <= 0.4f)
				{
					objSkid_Sound.mute = true;
					Once = false;
				}
			}
		}
		else if (btn_Rotation == -1f)
		{
			tmpAngle[0] = Mathf.MoveTowards(tmpAngle[0], -20f, Time.fixedDeltaTime * 70f);
			if (tmpAngle[0] != -20f)
			{
				return;
			}
			CoeffZ = Mathf.MoveTowards(CoeffZ, refCoeffZ_Min, Time.fixedDeltaTime * 40f);
			if (!b_AutoAcceleration)
			{
				if (!Once && (bool)objSkid_Sound && Mathf.Abs(localVelocity.z) > 0.4f)
				{
					objSkid_Sound.mute = false;
					Once = true;
				}
				else if (Once && (bool)objSkid_Sound && Mathf.Abs(localVelocity.z) <= 0.4f)
				{
					objSkid_Sound.mute = true;
					Once = false;
				}
			}
		}
		else
		{
			tmpAngle[0] = Mathf.MoveTowards(tmpAngle[0], 0f, Time.fixedDeltaTime * 100f);
			CoeffZ = Mathf.MoveTowards(CoeffZ, refCoeffZ_Max, Time.fixedDeltaTime * 10f);
			if (Once && (bool)objSkid_Sound && Mathf.Abs(localVelocity.z) > 0.4f)
			{
				objSkid_Sound.mute = true;
				Once = false;
			}
		}
	}

	private void UpdateMovement()
	{
		Vector3 vector = rb.transform.InverseTransformDirection(-rb.velocity);
		vector.z *= Coeff;
		vector.x *= CoeffZ;
		rb.AddRelativeForce(vector * BrakeForce, ForceMode.Force);
		Vector3 vector2 = new Vector3(0f, 0f, 0f);
		vector2 = t_ApplyForce.position;
		if (Input_Acceleration < 0f)
		{
			tmpmulti = Mathf.Lerp(tmpmulti, 0.25f, Time.fixedDeltaTime * 4f);
		}
		else if (Input_Acceleration > 0f)
		{
			tmpmulti = Mathf.Lerp(tmpmulti, 1f, Time.fixedDeltaTime * 4f);
		}
		if (!StopAcceleration)
		{
			ReachMaxRotationAcc = Mathf.MoveTowards(ReachMaxRotationAcc, 1f, Time.fixedDeltaTime * 2f);
			if (!raceIsFinished && !b_CountdownActivate)
			{
				if (b_AutoAcceleration && b_MaxAccelerationAfterCountdown)
				{
					b_MaxAccelerationAfterCountdown = false;
					Input_Acceleration = 1f;
					tmpmulti = 1f;
					CoeffZ = refCoeffZ_Max;
					ReachMaxRotationAcc = 1f;
				}
				rb.AddForceAtPosition(rb.transform.forward * tmpmulti * Force * curveAcceleration.Evaluate(ReachMaxRotationAcc) * Input_Acceleration, vector2, ForceMode.Force);
			}
		}
		Vector3 vector3 = -rb.angularVelocity;
		rb.AddTorque(vector3 * BrakeForce * 0.01f);
	}

	private void UpdateRandomReactions(Vector3 localVelocity, RaycastHit hit)
	{
		if (Physics.Raycast(rb.transform.position, new Vector3(0f, -1f, 0f), out hit, 0.3f, myLayerMask) && localVelocity.y < -0.2f && NumberOfWheelThatTouchGround > 2 && b_allowRandomCarValue)
		{
			StartCoroutine(I_AllowRandomCarValue());
		}
	}

	private void UpdateWheels(Vector3 localVelocity, RaycastHit hit)
	{
		NumberOfWheelThatTouchGround = 0;
		Vector3 vector = default(Vector3);
		StopAcceleration = false;
		for (int i = 0; i < RayCastWheels.Length; i++)
		{
			Vector3 direction = RayCastWheels[i].transform.TransformDirection(-Vector3.up);
			if (Physics.Raycast(RayCastWheels[i].transform.position, direction, out hit, dis, myLayerMask))
			{
				previous_Length2[i] = current_Length2[i];
				current_Length2[i] = restLength - hit.distance;
				springVelocity2[i] = (current_Length2[i] - previous_Length2[i]) / Time.fixedDeltaTime;
				springforce2[i] = springConstant * current_Length2[i];
				damperForce2[i] = damperConstant * springVelocity2[i];
				float num = 0f;
				RaycastHit hitInfo;
				if (Physics.Raycast(rb.transform.position, -rb.transform.up, out hitInfo, 10f, myLayerMask))
				{
					hitInfo.normal.Normalize();
					float num2 = 0f - Vector3.Dot(hitInfo.normal, Vector3.up);
					vector = (Vector3.up + hitInfo.normal * num2).normalized;
					num = Vector3.Angle(vector, -Vector3.up);
				}
				if (rb.velocity.magnitude < 2f && Input_Acceleration == 0f)
				{
					if (rb.velocity.magnitude < 0.5f)
					{
						Slide_03 = Mathf.MoveTowards(Slide_03, 1f, Time.fixedDeltaTime);
						Slide_01 = Mathf.MoveTowards(Slide_01, 0f, Time.fixedDeltaTime * 0.1f * curve_01.Evaluate(Slide_03));
						Slide_02 = Mathf.MoveTowards(Slide_02, 1f, Time.fixedDeltaTime * 0.1f * curve_01.Evaluate(Slide_03));
					}
					else
					{
						Slide_01 = Mathf.MoveTowards(Slide_01, 1f - (num - 90f) / 90f, Time.fixedDeltaTime * 0.1f);
						Slide_02 = Mathf.MoveTowards(Slide_02, (num - 90f) / 90f, Time.fixedDeltaTime * 0.1f);
						Slide_03 = 0f;
					}
				}
				else
				{
					Slide_03 = Mathf.MoveTowards(Slide_03, 1f, Time.deltaTime);
					Slide_01 = Mathf.MoveTowards(Slide_01, 1f, Time.deltaTime * 0.5f * curve_01.Evaluate(Slide_03));
					Slide_02 = Mathf.MoveTowards(Slide_02, 0f, Time.deltaTime * 0.5f * curve_01.Evaluate(Slide_03));
				}
				rb.AddForceAtPosition((RayCastWheels[i].transform.up * Slide_01 + Vector3.up * Slide_02) * (springforce2[i] + damperForce2[i]), RayCastWheels[i].transform.position, ForceMode.Force);
				if (hit.transform.CompareTag("Wall"))
				{
					StopAcceleration = true;
					break;
				}
				if ((double)hit.distance > 0.1)
				{
					if (i == 0 || i == 1)
					{
						Wheel_X_Rotate[i].transform.position = new Vector3(Wheel_X_Rotate[i].transform.position.x, hit.point.y - 0.03f + offsetWheelFront, Wheel_X_Rotate[i].transform.position.z);
						Wheel_X_Rotate[i].transform.localPosition = new Vector3(0f, Wheel_X_Rotate[i].transform.localPosition.y, 0f);
					}
					else
					{
						Wheel_X_Rotate[i].transform.position = new Vector3(Wheel_X_Rotate[i].transform.position.x, hit.point.y - 0.03f + offsetWheelRear, Wheel_X_Rotate[i].transform.position.z);
						Wheel_X_Rotate[i].transform.localPosition = new Vector3(0f, Wheel_X_Rotate[i].transform.localPosition.y, 0f);
					}
					if (Wheel_X_Rotate[i].transform.localPosition.y > 0f)
					{
						Wheel_X_Rotate[i].transform.localPosition = new Vector3(0f, 0f, 0f);
					}
					if (Wheel_X_Rotate[i].transform.localPosition.y <= Wheel_X_RefLocalPosition[i])
					{
						float y = Wheel_X_Rotate[i].transform.localPosition.y;
						y = Mathf.MoveTowards(y, Wheel_X_RefLocalPosition[i], Time.fixedDeltaTime);
						Wheel_X_Rotate[i].transform.localPosition = new Vector3(0f, y, 0f);
					}
				}
				else
				{
					Wheel_X_Rotate[i].transform.localPosition = new Vector3(0f, 0f, 0f);
				}
				NumberOfWheelThatTouchGround++;
			}
			else
			{
				float y2 = Wheel_X_Rotate[i].transform.localPosition.y;
				y2 = Mathf.MoveTowards(y2, Wheel_X_RefLocalPosition[i], Time.fixedDeltaTime);
				Wheel_X_Rotate[i].transform.localPosition = new Vector3(0f, y2, 0f);
			}
		}
	}

	private void UpdateRotation()
	{
		if (rb.velocity.magnitude > 0.2f && ((turnDirection == 1 && !b_AutoAcceleration) || (turnDirection == 1 && b_AutoAcceleration && Mathf.Abs(carAI.angle) > 10f)))
		{
			if (b_AutoAcceleration)
			{
				tmpFakeBodyRotation = Mathf.MoveTowards(tmpFakeBodyRotation, -1f * (BodyRotationValue + 2f), 10f * Time.fixedDeltaTime);
			}
			else
			{
				tmpFakeBodyRotation = Mathf.MoveTowards(tmpFakeBodyRotation, -1f * BodyRotationValue, 100f * Time.fixedDeltaTime);
			}
		}
		else if (rb.velocity.magnitude > 0.2f && ((turnDirection == -1 && !b_AutoAcceleration) || (turnDirection == -1 && b_AutoAcceleration && Mathf.Abs(carAI.angle) > 10f)))
		{
			if (b_AutoAcceleration)
			{
				tmpFakeBodyRotation = Mathf.MoveTowards(tmpFakeBodyRotation, BodyRotationValue + 2f, 10f * Time.fixedDeltaTime);
			}
			else
			{
				tmpFakeBodyRotation = Mathf.MoveTowards(tmpFakeBodyRotation, BodyRotationValue, 100f * Time.fixedDeltaTime);
			}
		}
		else if (b_AutoAcceleration)
		{
			tmpFakeBodyRotation = Mathf.MoveTowards(tmpFakeBodyRotation, 0f, 10f * Time.fixedDeltaTime);
		}
		else
		{
			tmpFakeBodyRotation = Mathf.MoveTowards(tmpFakeBodyRotation, 0f, 100f * Time.fixedDeltaTime);
		}
		Grp_BodyPlusBlobShadow.transform.localEulerAngles = new Vector3(Grp_BodyPlusBlobShadow.transform.localEulerAngles.x, Grp_BodyPlusBlobShadow.transform.localEulerAngles.y, tmpFakeBodyRotation);
		if (!b_AutoAcceleration)
		{
			fullBody.transform.Rotate(new Vector3(0f - backRotation, 0f, 0f));
		}
	}

	private void UpdateAIInput()
	{
		if (b_RespawnMobile)
		{
			RespawnTheCar();
			return;
		}
		if (b_CountdownActivate)
		{
			b_CarAccelerate = false;
			return;
		}
		if (b_btn_Acce)
		{
			Input_Acceleration = Mathf.MoveTowards(Input_Acceleration, 1f, Time.fixedDeltaTime * 8f);
			b_CarMoveForward = true;
			b_CarAccelerate = true;
		}
		else if (b_btn_Break)
		{
			Input_Acceleration = Mathf.MoveTowards(Input_Acceleration, -1f, Time.fixedDeltaTime * 4f);
			b_CarMoveForward = false;
			b_CarAccelerate = true;
		}
		else
		{
			Input_Acceleration = Mathf.MoveTowards(Input_Acceleration, 0f, Time.fixedDeltaTime * 4f);
			b_CarAccelerate = false;
		}
		if (Time.time < aiCrashDelay)
		{
			btn_Rotation = Mathf.MoveTowards(btn_Rotation, 0f, Time.fixedDeltaTime * 5f);
			ReachMaxRotation = Mathf.MoveTowards(ReachMaxRotation, 0f, Time.fixedDeltaTime * 5f);
			turnDirection = 0;
		}
		else if (b_btn_Right)
		{
			ReachMaxRotation = Mathf.MoveTowards(ReachMaxRotation, 1f, Time.fixedDeltaTime * (CarRotationSpeed + offsetRotationForMobile));
			btn_Rotation = Mathf.MoveTowards(btn_Rotation, 1f, Time.fixedDeltaTime * 6f * curveRotation.Evaluate(ReachMaxRotation));
			turnDirection = 1;
		}
		else if (b_btn_Left)
		{
			ReachMaxRotation = Mathf.MoveTowards(ReachMaxRotation, 1f, Time.fixedDeltaTime * (CarRotationSpeed + offsetRotationForMobile));
			btn_Rotation = Mathf.MoveTowards(btn_Rotation, -1f, Time.fixedDeltaTime * 6f * curveRotation.Evaluate(ReachMaxRotation));
			turnDirection = -1;
		}
		else
		{
			btn_Rotation = Mathf.MoveTowards(btn_Rotation, 0f, Time.fixedDeltaTime * 5f);
			ReachMaxRotation = Mathf.MoveTowards(ReachMaxRotation, 0f, Time.fixedDeltaTime * 5f);
			turnDirection = 0;
		}
	}

	private void UpdatePlayerInput()
	{
		if (Input.GetButtonDown("P" + playerNumber + "_Respawn") && !raceIsFinished && !b_CountdownActivate)
		{
			RespawnTheCar();
			return;
		}
		float num = Input.GetAxis("P" + playerNumber + "_Vertical");
		if ((playerNumber == 1 && BARDataManager.Instance.BARConfig.P1InvertVertical) || (playerNumber == 2 && BARDataManager.Instance.BARConfig.P2InvertVertical))
		{
			num *= -1f;
		}
		if (NumberOfWheelThatTouchGround == 0 && num < 0f)
		{
			backRotation = Time.deltaTime * 50f;
		}
		else
		{
			backRotation = 0f;
		}
		Input_Acceleration = Mathf.MoveTowards(Input_Acceleration, num, (!(Time.deltaTime * num > 0f)) ? 4 : 8);
		b_CarMoveForward = num > 0f;
		b_CarAccelerate = num != 0f;
		num = Input.GetAxis("P" + playerNumber + "_Horizontal");
		if ((playerNumber == 1 && BARDataManager.Instance.BARConfig.P1InvertHorizontal) || (playerNumber == 2 && BARDataManager.Instance.BARConfig.P2InvertHorizontal))
		{
			num *= -1f;
		}
		if (num != 0f)
		{
			ReachMaxRotation = Mathf.MoveTowards(ReachMaxRotation, 1f, Time.deltaTime * (CarRotationSpeed + offsetRotationForMobile));
			btn_Rotation = Mathf.MoveTowards(btn_Rotation, num, Time.deltaTime * 6f * curveRotation.Evaluate(ReachMaxRotation));
			turnDirection = ((!(num < 0f)) ? 1 : (-1));
		}
		else
		{
			btn_Rotation = Mathf.MoveTowards(btn_Rotation, 0f, Time.deltaTime * 5f);
			ReachMaxRotation = Mathf.MoveTowards(ReachMaxRotation, 0f, Time.deltaTime * 5f);
			turnDirection = 0;
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

	public void RespawnTheCar()
	{
		if (b_CarIsRespawning)
		{
			return;
		}
		b_CarIsRespawning = true;
		StopAllCoroutines();
		StartCoroutine(RepawnInitialization());
		if (!carAI.enabled)
		{
			BARSteamManager.Instance.SetAchievement("BAR_PUFF");
		}
		respawns++;
		if (respawns >= 2)
		{
			respawns = 0;
			offsetSpeedDifficultyManager = Mathf.Min(0f, offsetSpeedDifficultyManager + 0.1f);
			if (offsetSpeedDifficultyManager > 0f)
			{
				offsetSpeedDifficultyManager = 0f;
			}
		}
	}

	private IEnumerator RepawnInitialization()
	{
		ActivateCar(false);
		if (b_AutoAcceleration)
		{
			carAI.StopCo();
			carAI.b_endBackward = false;
			carAI.CarMoveForward = true;
		}
		GameObject cloud = Object.Instantiate(RespawnCloud, base.transform.position, Quaternion.Euler(90f, 0f, 0f));
		if (carAI.enabled)
		{
			AudioSource component = cloud.GetComponent<AudioSource>();
			if (component != null)
			{
				component.spatialBlend = 1f;
			}
		}
		Object.Destroy(cloud, 2f);
		GameObject closestCheckpoint = FindClosestCheckpoint();
		Vector3 targetPos = closestCheckpoint.transform.position;
		base.transform.rotation = closestCheckpoint.transform.rotation;
		targetPos.y += 0.5f;
		WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
		WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
		yield return waitForSeconds;
		while ((base.transform.position - targetPos).sqrMagnitude > 0.1f)
		{
			if (!b_Pause)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, targetPos, 0.2f);
			}
			yield return waitForEndOfFrame;
		}
		Force = tempForce;
		ActivateCar(true);
		if (b_AutoAcceleration)
		{
			b_CarIsRespawning = false;
			yield break;
		}
		yield return waitForSeconds;
		b_CarIsRespawning = false;
	}

	public void ActivateCar(bool activate)
	{
		rb.isKinematic = !activate;
		for (int i = 0; i < RayCastWheels.Length; i++)
		{
			RayCastWheels[i].gameObject.SetActive(activate);
		}
		Capsule_Rear.SetActive(activate);
		Capsule_Front.SetActive(activate);
		BodyCollider.SetActive(activate);
		ColliderCarOnBack.GetComponent<BoxCollider>().enabled = activate;
		Grp_BodyPlusBlobShadow.SetActive(activate);
	}

	private GameObject FindClosestCheckpoint()
	{
		return carPathFollow.LastWaypoint;
	}

	public void Pause()
	{
		if (b_Pause)
		{
			b_Pause = false;
			audio_.UnPause();
			objSkid_Sound.UnPause();
			obj_CarImpact_Sound.UnPause();
			if (!b_CarIsRespawning)
			{
				rb.isKinematic = false;
				rb.velocity = tmpVelocity;
			}
		}
		else
		{
			tmpVelocity = rb.velocity;
			rb.isKinematic = true;
			audio_.Pause();
			objSkid_Sound.Pause();
			obj_CarImpact_Sound.Pause();
			b_Pause = true;
		}
	}

	public float _localVelovity()
	{
		return rb.transform.InverseTransformDirection(rb.velocity).z;
	}

	private IEnumerator I_AllowRandomCarValue()
	{
		b_allowRandomCarValue = false;
		if (carAI.enabled)
		{
			carAI.F_RandomCarValues();
		}
		t = 0f;
		float timeToMove = 1f;
		while (t < 1f)
		{
			if (!b_Pause)
			{
				t = Mathf.MoveTowards(t, 1f, Time.fixedDeltaTime / timeToMove);
			}
			yield return null;
		}
		b_allowRandomCarValue = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("RespawnZone"))
		{
			RespawnTheCar();
		}
		else if (other.CompareTag("Boost"))
		{
			boost = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Boost"))
		{
			boost = false;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		float num = 0f;
		if (collision.gameObject.CompareTag("Car"))
		{
			if (collision.relativeVelocity.magnitude < 5f)
			{
				num = impactVolumeMax * collision.relativeVelocity.magnitude * 0.5f;
			}
			else
			{
				num = impactVolumeMax;
				if (aiCrashDelay < Time.time)
				{
					aiCrashDelay = Time.time + 0.5f;
				}
			}
			obj_CarImpact_Sound.volume = num;
			if (!obj_CarImpact_Sound.isPlaying)
			{
				obj_CarImpact_Sound.Play();
			}
		}
		else if (collision.relativeVelocity.magnitude < 5f)
		{
			num = impactVolumeMax * collision.relativeVelocity.magnitude * 0.4f;
			obj_CarImpact_Sound.volume = num;
			if (!obj_CarImpact_Sound.isPlaying)
			{
				obj_CarImpact_Sound.Play();
			}
		}
		if (collision.collider.CompareTag("RespawnZone"))
		{
			RespawnTheCar();
		}
	}
}
