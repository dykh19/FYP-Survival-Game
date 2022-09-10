using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveTimerManager : MonoBehaviour
{
    public static WaveTimerManager Instance { get; private set; }

    public float TimeValue;
    public float TimeRemaining;
    public bool IsTimerRunning;
    public TMP_Text TimerUI;
    public Monster_Spawner MonsterSpawner;

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
        TimerUI = GameObject.Find("Timer Text").GetComponent<TMP_Text>();
        MonsterSpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Monster_Spawner>();
    }

    private void Start()
    {
        SetTimer(300);
        StartTimer();
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
                if (MonsterSpawner.waveNumber == 0)
                {
                    MonsterSpawner.StartWave();
                }
                else
                {
                    MonsterSpawner.NextWave();
                }
                
            }
        }
        if (Input.GetButtonDown("Tilde"))
        {
            Debug.Log("Pressed Tilde");
            if(MonsterSpawner.isWave != true)
            {
                SkipToNextWave();
                Debug.Log("Skipped to next wave");
            }
        }
    }

    public void SetTimer(float Time)
    {
        TimeValue = Time;
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

    public void ResetTimer()
    {
        TimeRemaining = TimeValue;
    }

    public void ShowTimer()
    {
        TimerUI.gameObject.SetActive(true);
    }

    public void HideTimer()
    {
        TimerUI.gameObject.SetActive(false);
    }

    void UpdateTimerUI()
    {
        float TimeToDisplay = TimeRemaining + 1;
        float minutes = Mathf.FloorToInt(TimeToDisplay / 60);
        float seconds = Mathf.FloorToInt(TimeToDisplay % 60);
        TimerUI.text = "";
        TimerUI.text += "Time until next wave: ";
        TimerUI.text += string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SkipToNextWave()
    {
        StopTimer();
        ResetTimer();
        if (MonsterSpawner.waveNumber == 0)
        {
            MonsterSpawner.StartWave();
        }
        else
        {
            MonsterSpawner.NextWave();
        }
    }

    public void StartNewWaveTimer()
    {
        ResetTimer();
        StartTimer();
    }


}
