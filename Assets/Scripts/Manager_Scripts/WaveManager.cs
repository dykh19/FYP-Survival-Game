using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int NumberOfWaves;
    public int TimeToNextWave;
    public int CurrentWave;

    // Start is called before the first frame update
    void Start()
    {
        //Start Timer for First Wave
        TimerManager.Instance.SetTimer(TimeToNextWave);
        TimerManager.Instance.StartTimer();
    }

    public void SpawnNextWave()
    {
        if (GameManager.Instance.CurrentGameMode == GameMode.ENDLESS)
        {
            //Call Endless Wave Spawning Functions Here
            TimerManager.Instance.SetTimer(TimeToNextWave);
            TimerManager.Instance.StartTimer();
        }
        else
        {
            if (CurrentWave <= NumberOfWaves)
            {
                //Call Normal Wave Spawning Functions Here
                TimerManager.Instance.SetTimer(TimeToNextWave);
                TimerManager.Instance.StartTimer();
                CurrentWave++;

                //TODO: Figure out how to know end of wave to start next wave and end game
            }
        }
        
    }

    
}
