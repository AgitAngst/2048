using UnityEngine;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour
{
	public bool SFX;

	public Sprite musicOnSprite;

	public Sprite musicOffSprite;

	public Sprite sfxOnSprite;

	public Sprite sfxOffSprite;

	public Image spriteButton;

	private void Start()
	{
		SetButton();
	}

	public void MusicButtonClicked()
	{
		AudioManager.Instance.MuteMusic();
		AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick,false);
		SetButton();
	}

	public void SfxButtonClicked()
	{
		AudioManager.Instance.MuteSfx();
		AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick, false);
		SetButton();
	}

	private void SetButton()
	{
		if ((!AudioManager.Instance.IsMusicMute() && !SFX) || (!AudioManager.Instance.IsSfxMute() && SFX))
		{
			if (SFX)
			{
				spriteButton.sprite = sfxOnSprite;
			}
			else
			{
				spriteButton.sprite = musicOnSprite;
			}
		}
		else if (SFX)
		{
			spriteButton.sprite = sfxOffSprite;
		}
		else
		{
			spriteButton.sprite = musicOffSprite;
		}
	}
}
