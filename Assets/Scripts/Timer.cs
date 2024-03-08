using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public float timeRemaining = 10f;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeText;
    public static Timer instance = null;
    [SerializeField] public SaveSytem saveSytem;

    void Awake()
    {
        
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                print("Auto-save!");
                saveSytem.SaveSettings();
                timeRemaining = 10f;
            }
        }
    }
    
    //Ui timer countodwn
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
