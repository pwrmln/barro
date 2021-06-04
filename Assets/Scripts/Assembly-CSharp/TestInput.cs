using SCJogos.Database;
using UnityEngine;
using UnityEngine.UI;

public class TestInput : MonoBehaviour
{
	[SerializeField]
	private Image imageLeft;

	[SerializeField]
	private Image imageRight;

	[SerializeField]
	private string input;

	[Space]
	[SerializeField]
	private Toggle toggleP1Horizontal;

	[SerializeField]
	private Toggle toggleP1Vertical;

	[SerializeField]
	private Toggle toggleP2Horizontal;

	[SerializeField]
	private Toggle toggleP2Vertical;

	private void Start()
	{
		if ((bool)toggleP1Horizontal)
		{
			toggleP1Horizontal.isOn = BARDataManager.Instance.BARConfig.P1InvertHorizontal;
			toggleP1Vertical.isOn = BARDataManager.Instance.BARConfig.P1InvertVertical;
			toggleP2Horizontal.isOn = BARDataManager.Instance.BARConfig.P2InvertHorizontal;
			toggleP2Vertical.isOn = BARDataManager.Instance.BARConfig.P2InvertVertical;
		}
	}

	private void Update()
	{
		float num = Input.GetAxis(input);
		if (BARDataManager.Instance.BARConfig.P1InvertHorizontal && input == "P1_Horizontal")
		{
			num *= -1f;
		}
		else if (BARDataManager.Instance.BARConfig.P1InvertVertical && input == "P1_Vertical")
		{
			num *= -1f;
		}
		else if (BARDataManager.Instance.BARConfig.P2InvertHorizontal && input == "P2_Horizontal")
		{
			num *= -1f;
		}
		else if (BARDataManager.Instance.BARConfig.P2InvertVertical && input == "P2_Vertical")
		{
			num *= -1f;
		}
		if (num > 0f)
		{
			imageLeft.fillAmount = 0f;
			imageRight.fillAmount = num;
		}
		else if (num < 0f)
		{
			imageLeft.fillAmount = num * -1f;
			imageRight.fillAmount = 0f;
		}
		else
		{
			imageLeft.fillAmount = 0f;
			imageRight.fillAmount = 0f;
		}
	}
}
