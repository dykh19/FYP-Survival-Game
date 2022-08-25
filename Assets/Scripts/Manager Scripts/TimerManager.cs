using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }

    public float TimeRemaining;
    public bool IsTimerRunning;
    public TMP_Text TimerUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTimerRunning)
        {
            if (TimeRemaining > 0)
            {
                TimeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                TimeRemaining = 0;
                IsTimerRunning = false;
            }
        }
    }

    public void SetTimer(float Time)
    {
        TimeRemaining = Time;
    }

    public void StartTimer()
    {
        IsTimerRunning = true;
    }

    public void StopTimer()
    {
        IsTimerRunning = false;
    }

    void UpdateTimerUI()
    {
        float TimeToDisplay = TimeRemaining + 1;
        float minutes = Mathf.FloorToInt(TimeToDisplay / 60);
        float seconds = Mathf.FloorToInt(TimeToDisplay % 60);
        TimerUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


}
