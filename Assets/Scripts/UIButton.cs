using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioManager _audioManager;
    public void PlaySound()
    {
        _audioManager.PlayEffects(clickSound,false);
    }

    public void PlaySoundRandomPitch()
    {
        _audioManager.PlayEffects(clickSound, true);
    }

}
