using UnityEngine;
using UnityEngine.UI;

public class SelectASprite : MonoBehaviour
{
	public Sprite SpriteOFF;

	public Sprite SpriteON;

	private Image sr;

	private void Start()
	{
		sr = base.gameObject.GetComponent<Image>();
		if (SpriteOFF == null)
		{
			SpriteOFF = sr.sprite;
		}
	}

	public void F_Sprite_OFF()
	{
		sr.sprite = SpriteOFF;
	}

	public void F_Sprite_ON()
	{
		if ((bool)SpriteON)
		{
			sr.sprite = SpriteON;
		}
	}
}
