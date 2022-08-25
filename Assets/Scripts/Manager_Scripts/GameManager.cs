using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// Game Manager is in-charge of the overall game state
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameMode CurrentGameMode;
    public Difficulty CurrentDifficulty;
    public GameState CurrentGameState;
    public int NumberOfWaves;
    public int TimeToNextWave;

    void Awake()
    {
        // Make sure there is only one instance of GameManager and prevent it from being destroyed
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnNewGameLevelLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            if(CurrentGameState == GameState.INGAME)
            {
                CurrentGameState = GameState.PAUSED;
                PauseGame();
            }
            else if(CurrentGameState == GameState.PAUSED)
            {
                CurrentGameState = GameState.INGAME;
                ResumeGame();
            }    

        }
    }

    public void SetGameMode(int newMode)
    {
        CurrentGameMode = (GameMode)newMode;
    }

    public void SetDifficulty(int newDifficulty)
    {
        CurrentDifficulty = (Difficulty)newDifficulty;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void WinGame()
    {

    }

    public void LoseGame()
    {

    }

    public void OnNewGameLevelLoaded(Scene scene, LoadSceneMode aMode)
    {
        //Perform Initial Loading Stuff Here
        //Eg. Generate Map, Choose Base Location
        CurrentGameState = GameState.INGAME;
    }
}
