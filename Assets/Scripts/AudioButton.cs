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
    public AudioManager _audioManager;

    private void Start()
	{
		SetButton();
	}

	public void MusicButtonClicked()
	{
        _audioManager.MuteMusic();
        _audioManager.PlayEffects(_audioManager.buttonClick,false);
		SetButton();
	}

	public void SfxButtonClicked()
	{
		_audioManager.MuteSfx();
        _audioManager.PlayEffects(_audioManager.buttonClick, false);
		SetButton();
	}

	private void SetButton()
	{
		if ((!_audioManager.IsMusicMute() && !SFX) || (!_audioManager.IsSfxMute() && SFX))
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
