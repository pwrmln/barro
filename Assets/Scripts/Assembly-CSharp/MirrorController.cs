using SCJogos.Database;
using UnityEngine;
using UnityEngine.UI;

public class MirrorController : MonoBehaviour
{
	[SerializeField]
	private RenderTexture textureP1;

	[SerializeField]
	private RenderTexture textureP2;

	[SerializeField]
	private RawImage rawImage;

	private CarController car;

	private void Update()
	{
		if (Input.GetButtonDown("P1_Mirror") && (bool)car && car.playerNumber == 1)
		{
			BARDataManager.Instance.BARConfig.P1Mirror = !BARDataManager.Instance.BARConfig.P1Mirror;
			BARDataManager.Instance.BARConfig.Save();
			if ((bool)car)
			{
				SetCameraP1();
			}
		}
		if (Input.GetButtonDown("P2_Mirror") && (bool)car && car.playerNumber == 2 && !car.carAI.enabled)
		{
			BARDataManager.Instance.BARConfig.P2Mirror = !BARDataManager.Instance.BARConfig.P2Mirror;
			BARDataManager.Instance.BARConfig.Save();
			if ((bool)car)
			{
				SetCameraP2();
			}
		}
	}

	public void SetCamera(CarController car)
	{
		this.car = car;
		if (this.car.playerNumber == 1)
		{
			SetCameraP1();
		}
		else if (this.car.playerNumber == 2)
		{
			SetCameraP2();
		}
	}

	private void SetCameraP1()
	{
		car.MirrorCamera.gameObject.SetActive(BARDataManager.Instance.BARConfig.P1Mirror);
		rawImage.enabled = BARDataManager.Instance.BARConfig.P1Mirror;
		if (BARDataManager.Instance.BARConfig.P1Mirror)
		{
			car.MirrorCamera.targetTexture = textureP1;
			rawImage.texture = textureP1;
		}
	}

	private void SetCameraP2()
	{
		car.MirrorCamera.gameObject.SetActive(BARDataManager.Instance.BARConfig.P2Mirror);
		rawImage.enabled = BARDataManager.Instance.BARConfig.P2Mirror;
		if (BARDataManager.Instance.BARConfig.P2Mirror)
		{
			car.MirrorCamera.targetTexture = textureP2;
			rawImage.texture = textureP2;
		}
	}
}
