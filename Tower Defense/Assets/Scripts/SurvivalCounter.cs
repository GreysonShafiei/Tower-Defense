using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SurvivalCounter : MonoBehaviour
{
    public static SurvivalCounter Instance { get; private set; }
    public TextMeshProUGUI timer;
    public float timerStartTime; // Tracks the time when the survival timer started
    public float survivalTime = 0; // Stores the total survival time
    private void Update()
    {
        timer.text = survivalTime.ToString("F2");
    }

    public void startSurvivalTimer()
    {
        // Record the time when gameplay starts
        timerStartTime = Time.time;
    }

    public void endSurvivalTimer()
    {
        // Calculate the survival time as the difference between now and when the timer started
        survivalTime = Time.time - timerStartTime;
    }
}
