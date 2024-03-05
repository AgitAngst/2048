using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public AudioClip clickSound;

    public void PlaySound()
    {
        AudioManager.Instance.PlayEffects(clickSound,false);
    }

    public void PlaySoundRandomPitch()
    {
        AudioManager.Instance.PlayEffects(clickSound, true);
    }

}
