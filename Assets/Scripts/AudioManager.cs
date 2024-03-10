using UnityEngine;
using System;
using YG;
public class AudioManager : MonoBehaviour
{
	//public static AudioManager Instance;
	
	[Header("Audio Sources")]
	public AudioSource sfxSource;

	public AudioSource musicSource;

	[Header("Background Music")]
	public AudioClip menuMusic;

	public AudioClip[] _gameMusic;

	[Header("Sound Effects")]
	public AudioClip buttonClick;

	public AudioClip gameOver;

	public AudioClip blockMovement;

	public AudioClip blockMerge;
    public AudioClip PlayerClick;


    private bool muteMusic;

	private bool muteSFX;
	public bool isRandomPitch;
	private bool isFocused;
	private bool isMusicPlaying;
    public static event Action MusicToggleEvent;


    private void OnEnable()
    {
		MusicToggleEvent += PlayRandomMusic;
    }

    private void OnDisable()
    {
        MusicToggleEvent -= PlayRandomMusic;
    }
    private void Awake()
	{
		//if (Instance == null)
		//{
		//	Instance = this;
		//}
		//else if (Instance != this)
		//{
		//	UnityEngine.Object.Destroy(base.gameObject);
		//}
        //UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		muteMusic = ((PlayerPrefs.GetInt("MuteMusic") == 1) ? true : false);
		muteSFX = ((PlayerPrefs.GetInt("MuteEfx") == 1) ? true : false);
        MusicToggleEvent?.Invoke();
        //PlayMusic(menuMusic);
        PlayRandomMusic();
	}

    private void Update()
    {
		
    }
    void OnApplicationFocus(bool b)
    {
        print("application focus: " + b);
        isFocused = b;
    }
    public void PlayMusic(AudioClip clip)
	{
		if (!muteMusic)
		{
			musicSource.clip = clip;
			if (!musicSource.isPlaying)
			{
				musicSource.Play();
			}
		}
	}

	public void PlayRandomMusic()
	{
        var m = UnityEngine.Random.Range(0,_gameMusic.Length);
        if (!muteMusic)
        {
            musicSource.clip = _gameMusic[m];
            if (!musicSource.isPlaying)
            {
                musicSource.Play();
				isMusicPlaying = true;
            }
        }
    }


	public void StopMusic()
	{
		musicSource.Stop();
        isMusicPlaying = false;
    }

    public void PlayEffects(AudioClip clip, bool isRandomPitch)
	{
		if (!muteSFX)
		{
			if (!isRandomPitch)
			{
                sfxSource.PlayOneShot(clip);
            }
			else
			{
				sfxSource.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.75f,1f);
				sfxSource.PlayOneShot(clip);

            }
        }
	}

	public void MuteMusic()
	{
		MusicToggleEvent?.Invoke();

        if (muteMusic)
		{
			muteMusic = false;
			//PlayMusic(menuMusic);
			PlayRandomMusic();

            PlayerPrefs.SetInt("MuteMusic", 0);
		}
		else
		{
			muteMusic = true;
			StopMusic();
			PlayerPrefs.SetInt("MuteMusic", 1);
		}
	}

	public void MuteSfx()
	{
		if (muteSFX)
		{
			PlayerPrefs.SetInt("MuteEfx", 0);
		}
		else
		{
			PlayerPrefs.SetInt("MuteEfx", 1);
		}
		muteSFX = !muteSFX;
	}

	public bool IsMusicMute()
	{
		return muteMusic;
	}

	public bool IsSfxMute()
	{
		return muteSFX;
	}
}
