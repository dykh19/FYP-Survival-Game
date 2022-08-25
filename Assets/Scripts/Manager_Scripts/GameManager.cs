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

    //When Scene changes to the game level, run the OnNewGameLevelLoaded function
    private void Start()
    {
        SceneManager.sceneLoaded += OnNewGameLevelLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        //Temporary Pause Function, toggle pause with escape
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

    //Set Time Scale to 1 to resume normal game speed
    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    //Set Time Scale to 0 to pause game speed
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    //Function to run when the player completes the game objective
    public void WinGame()
    {

    }

    //Function to run when the base is destroyed
    public void LoseGame()
    {

    }

    //This function will be called by the game manager when the game level is loaded
    public void OnNewGameLevelLoaded(Scene scene, LoadSceneMode aMode)
    {
        //Perform Initial Loading Stuff Here
        //Eg. Generate Map, Choose Base Location
        CurrentGameState = GameState.INGAME;
    }
}
