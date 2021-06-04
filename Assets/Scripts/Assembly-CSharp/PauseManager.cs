using System.Collections;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
	private Countdown countdown;

	private Cam_Follow[] cam_Follow;

	private ParticleSystem[] particles;

	private LapCounter lapCounter;

	private CarController[] carController;

	private CarAI[] carAI;

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);
		countdown = Object.FindObjectOfType<Countdown>();
		lapCounter = Object.FindObjectOfType<LapCounter>();
		cam_Follow = Object.FindObjectsOfType<Cam_Follow>();
		particles = Object.FindObjectsOfType<ParticleSystem>();
		GameObject[] arrCars = GameObject.FindGameObjectsWithTag("Car");
		carController = new CarController[arrCars.Length];
		carAI = new CarAI[arrCars.Length];
		for (int i = 0; i < arrCars.Length; i++)
		{
			carController[i] = arrCars[i].GetComponent<CarController>();
			carAI[i] = arrCars[i].GetComponent<CarAI>();
		}
	}

	public void PauseGame()
	{
		countdown.Pause();
		lapCounter.Pause();
		for (int i = 0; i < carController.Length; i++)
		{
			if ((bool)carController[i])
			{
				carController[i].Pause();
			}
			if ((bool)carAI[i])
			{
				carAI[i].Pause();
			}
		}
		for (int j = 0; j < cam_Follow.Length; j++)
		{
			cam_Follow[j].Pause();
		}
		for (int k = 0; k < particles.Length; k++)
		{
			if (particles[k].isPlaying)
			{
				particles[k].Pause();
			}
			else
			{
				particles[k].Play();
			}
		}
	}
}
