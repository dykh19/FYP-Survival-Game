using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
// Game Manager is in-charge of the overall game state
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameMode CurrentGameMode;
    public Difficulty CurrentDifficulty;
    public GameState CurrentGameState;
    public int NumberOfWaves;
    public int TimeToNextWave;

    public int EasyWaveCount;
    public int NormalWaveCount;
    public int HardWaveCount;

    public int WaveCountToWin;

    public WorldGenerator worldgen;

    //Player Inventory
    public Inventory playerInventory;
    public GameItem[] startingItems;

    //User Interfaces
    public UserInterface[] userInterfaces;

    public PlayerStatistics PlayerStats;

    public UnityAction OnPlayerDie;


    void Awake()
    {
        // Make sure there is only one instance of GameManager and prevent it from being destroyed
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        
        DontDestroyOnLoad(this);
        PlayerStats = new PlayerStatistics();
        OnPlayerDie += LoseGame;

        
    }

    //When Scene changes to the game level, run the OnNewGameLevelLoaded function
    private void Start()
    {
        SceneManager.sceneLoaded += LoadNewScene;
    }

    // Update is called once per frame
    void Update()
    {

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

    public void ExitToMainMenu()
    {
        
        SceneManager.LoadScene(0);
    }

    //Function to run when the player completes the game objective
    public void WinGame()
    {
        Debug.Log("Player Wins Game");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(3);
    }

    //Function to run when the base is destroyed
    public void LoseGame()
    {
        Debug.Log("Player is Dead");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(2);
    }

    public void LoadNewScene(Scene scene, LoadSceneMode aMode)
    {
        //Perform Initial Loading Stuff Here
        //Eg. Generate Map, Choose Base Location
        if (SceneManager.GetActiveScene().name == "GamePlay")
        {
            CurrentGameState = GameState.INGAME;

            switch (CurrentDifficulty)
            {
                case Difficulty.EASY:
                    WaveCountToWin = EasyWaveCount;
                    break;
                case Difficulty.NORMAL:
                    WaveCountToWin = NormalWaveCount;
                    break;
                case Difficulty.HARD:
                    WaveCountToWin = HardWaveCount;
                    break;
                default:
                    WaveCountToWin = 1;
                    break;

            }

            playerInventory = new Inventory();

            userInterfaces[0].userInterface = GameObject.Find("PlayerHUD").GetComponent<Canvas>();
            userInterfaces[1].userInterface = GameObject.Find("PauseMenuUI").GetComponent<Canvas>();
            userInterfaces[2].userInterface = GameObject.Find("InventoryUI").GetComponent<Canvas>();

            worldgen = GameObject.Find("World Generator").GetComponent<WorldGenerator>();
            worldgen.CreateWorld();
        }
        
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            CurrentGameState = GameState.START;
            ResumeGame();
        }
    }

    public void UpdatePlayerStatistics()
    {
        PlayerStats.currentDifficutly = CurrentDifficulty;
        PlayerStats.currentGameMode = CurrentGameMode;

        PlayerStats.CurrentHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().CurrentHealth;
        PlayerStats.MaxHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().MaxHealth;

    }
}
