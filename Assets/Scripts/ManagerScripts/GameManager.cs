using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
// Game Manager is in-charge of the overall game state
public class GameManager : MonoBehaviour
{
    // Make this class a singleton instance
    public static GameManager Instance { get; private set; }

    public GameMode CurrentGameMode;
    public Difficulty CurrentDifficulty;
    public GameState CurrentGameState;
    public int WaveCountToWin;
    public int TimeToNextWave;

    public int EasyWaveCount;
    public int NormalWaveCount;
    public int HardWaveCount;

    public WorldGenerator WorldGen;

    // Player Inventory
    public Inventory PlayerInventory;
    public GameItem[] StartingItems;

    // User Interfaces
    public UserInterface[] UserInterfaces;

    // Player Statistics to keep track of current game's stats, also used for saving and loading
    public PlayerStatistics PlayerStats;

    // Unity Action that will run multiple functions when invoked
    public UnityAction OnPlayerDie;


    void Awake()
    {
        // Make sure there is only one instance of GameManager and destroy itself if there is already an existing instance
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

    // Set Game Mode base on integer given
    public void SetGameMode(int newMode)
    {
        CurrentGameMode = (GameMode)newMode;
    }

    // Set Difficulty based on integer given
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

    // Function to load the main menu
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
    
    // Function will be called whenever a new scene is loaded
    public void LoadNewScene(Scene scene, LoadSceneMode aMode)
    {
        // Perform Initial Loading Stuff Here
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

            PlayerInventory = new Inventory();

            UserInterfaces[0].userInterface = GameObject.Find("PlayerHUD").GetComponent<Canvas>();
            UserInterfaces[1].userInterface = GameObject.Find("PauseMenuUI").GetComponent<Canvas>();
            UserInterfaces[2].userInterface = GameObject.Find("InventoryUI").GetComponent<Canvas>();
            UserInterfaces[3].userInterface = GameObject.Find("VendorUI").GetComponent<Canvas>();

            WorldGen = GameObject.Find("World Generator").GetComponent<WorldGenerator>();
            WorldGen.CreateWorld();
        }
        
        // If the loaded scene is MainMenu, reset the game state and resume the paused timescale
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            CurrentGameState = GameState.START;
            ResumeGame();
        }
    }

    // Function to update the Player Statistics object with the latest data
    public void UpdatePlayerStatistics()
    {
        PlayerStats.CurrentDifficutly = CurrentDifficulty;
        PlayerStats.CurrentGameMode = CurrentGameMode;

        PlayerStats.CurrentHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().CurrentHealth;
        PlayerStats.MaxHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().MaxHealth;
    }
}
