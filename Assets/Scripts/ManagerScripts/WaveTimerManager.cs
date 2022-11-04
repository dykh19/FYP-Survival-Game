using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// WaveTimerManager is the timer for spawning the next wave
public class WaveTimerManager : MonoBehaviour
{
    // Make this class a singleton instance
    public static WaveTimerManager Instance { get; private set; }

    public float TimeValue;
    public float TimeRemaining;
    public bool IsTimerRunning;
    public TMP_Text TimerUI;
    public TMP_Text WaveCountUI;
    public Monster_Spawner MonsterSpawner;

    private void Awake()
    {
        // Make sure there is only one instance of WaveTimerManager and destroy itself if there is already an existing instance
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        // Set References to the UI and Spawner
        TimerUI = GameObject.Find("TimerText").GetComponent<TMP_Text>();
        WaveCountUI = GameObject.Find("WaveCountText").GetComponent<TMP_Text>();
        MonsterSpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Monster_Spawner>();
        GameManager.Instance.LoadData += LoadTimerData;
        GameManager.Instance.SaveData += SaveTimerData;
    }

    // Set the timer based on the GameManager Value and start the timer
    private void Start()
    {
        if (!GameManager.Instance.LoadingSavedGame)
        {
            SetTimer(GameManager.Instance.TimeToNextWave);
            StartTimer();
            UpdateWaveCountText();
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
                if (MonsterSpawner.waveNumber == 0)
                {
                    MonsterSpawner.StartWave();
                }
                else
                {
                    MonsterSpawner.NextWave();
                }
                UpdateWaveCountText();
            }
        }
        if (Input.GetButtonDown("Tilde"))
        {
            if(MonsterSpawner.isWave != true)
            {
                SkipToNextWave();
                UpdateWaveCountText();
            }
        }
    }

    // Function to set the timer with a new time value
    public void SetTimer(float Time)
    {
        TimeValue = Time;
        TimeRemaining = Time;
    }

    // Function to start the timer
    public void StartTimer()
    {
        IsTimerRunning = true;
    }

    // Function to stop the timer
    public void StopTimer()
    {
        IsTimerRunning = false;
    }

    // Function to reset the timer
    public void ResetTimer()
    {
        TimeRemaining = TimeValue;
    }

    // Function to set time value
    public void SetTimeValue(float Time)
    {
        TimeValue = Time;
    }

    // Function to show the timer
    public void ShowTimer()
    {
        TimerUI.gameObject.SetActive(true);
    }

    // Function to hide the timer
    public void HideTimer()
    {
        TimerUI.gameObject.SetActive(false);
    }

    // Function to set the text to show the wave is here
    public void IncomingWave()
    {
        TimerUI.text = "Enemy Wave is here!";
    }    

    // Function to update the text to show the time left
    void UpdateTimerUI()
    {
        float TimeToDisplay = TimeRemaining + 1;
        float minutes = Mathf.FloorToInt(TimeToDisplay / 60);
        float seconds = Mathf.FloorToInt(TimeToDisplay % 60);
        TimerUI.text = "";
        TimerUI.text += "Time until next wave: ";
        TimerUI.text += string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Function to skip the timer and spawn the next wave
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

    // Function to reset and start the timer
    public void StartNewWaveTimer()
    {
        ResetTimer();
        StartTimer();
    }

    // Function to update the wave progress text
    public void UpdateWaveCountText()
    {
        if (GameManager.Instance.CurrentGameMode == GameMode.NORMAL)
        {
            WaveCountUI.text = "Wave Progress: " + MonsterSpawner.waveNumber + "/" + GameManager.Instance.WaveCountToWin;
        }
        else if (GameManager.Instance.CurrentGameMode == GameMode.ENDLESS)
        {
            WaveCountUI.text = "Wave Progress: " + MonsterSpawner.waveNumber + "/ Endless";
        }
        
    }

    public void LoadTimerData()
    {
        TimeRemaining = GameManager.Instance.PlayerStats.TimeLeftToNextWave;
        StartTimer();
        UpdateWaveCountText();
    }

    public void SaveTimerData()
    {
        GameManager.Instance.PlayerStats.TimeLeftToNextWave = TimeRemaining;
    }
}
