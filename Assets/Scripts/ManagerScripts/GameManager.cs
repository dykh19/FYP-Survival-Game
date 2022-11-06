using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

// Game Manager is in-charge of the overall game state.

public class GameManager : MonoBehaviour
{
    // Make this class a singleton instance.
    public static GameManager Instance { get; private set; }

    public bool LoadingSavedGame;

    public GameMode CurrentGameMode;
    public Difficulty CurrentDifficulty;
    public GameState CurrentGameState;
    public int WaveCountToWin;
    public int TimeToNextWave;

    public int EasyWaveCount;
    public int NormalWaveCount;
    public int HardWaveCount;

    public WorldGenerator WorldGen;

    public GameObject playerReference;

    // Player Inventory
    public Inventory PlayerInventory;
    public GameItem[] StartingItems;

    // Player Skills
    public Skills PlayerSkills;

    // User Interfaces
    public UserInterface[] UserInterfaces;

    // Player Statistics to keep track of current game's stats, also used for saving and loading.
    public PlayerStatistics PlayerStats;

    // Unity Action that will run multiple functions when invoked.
    public UnityAction OnPlayerDie;

    public UnityAction LoadData;
    public UnityAction SaveData;

    void Awake()
    {
        // Make sure there is only one instance of GameManager and destroy itself if there is already an existing instance.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);

        PlayerStats = new PlayerStatistics();
        LoadingSavedGame = false;
        OnPlayerDie += LoseGame;
        //PlayerSkills.Initialize();
    }

    // When Scene changes to the game level, run the OnNewGameLevelLoaded function
    private void Start()
    {
        SceneManager.sceneLoaded += LoadNewScene;
    }

    private void Update()
    {
        if (PlayerSkills != null && PlayerSkills.initialized == true)
        {
            PlayerSkills.Update();
        }
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

    // Set Time Scale to 1 to resume normal game speed
    public void ResumeGame()
    {
        CurrentGameState = GameState.INGAME;
        Time.timeScale = 1f;
    }

    // Set Time Scale to 0 to pause game speed
    public void PauseGame()
    {
        CurrentGameState = GameState.PAUSED;
        Time.timeScale = 0f;
    }

    // Function to load the main menu
    public void ExitToMainMenu()
    {
        ResetPlayerStats();
        SceneManager.LoadScene(0);
    }

    public void SaveAndExitToMainMenu()
    {
        SaveLoadManager.Instance.SavePlayerDataToFile();
        ResetPlayerStats();
        SceneManager.LoadScene(0);
    }

    // Function to run when the player completes the game objective
    public void WinGame()
    {
        Debug.Log("Player Wins Game");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(3);
    }

    // Function to run when the base is destroyed.
    public void LoseGame()
    {
        Debug.Log("Player is Dead");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(2);
    }

    // Function will be called whenever a new scene is loaded.
    public void LoadNewScene(Scene scene, LoadSceneMode aMode)
    {
        // Perform Initial Loading Stuff Here
        if (SceneManager.GetActiveScene().name == "GamePlay")
        {
            playerReference = GameObject.FindGameObjectWithTag("Player");

            if (LoadingSavedGame)
            {
                LoadSavedGame();
            }
            else
            {
                CreateNewGame();
            }
        }

        // If the loaded scene is MainMenu, reset the game state and resume the paused timescale.
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            CurrentGameState = GameState.START;
            PlayerSkills.Reset();
            AudioManager.instance.PlayMainMenu();
            ResumeGame();
        }
    }

    private void CreateNewGame()
    {
        CurrentGameState = GameState.INGAME;

        LoadWaveCount();
        PlayerStats = new PlayerStatistics();
        PlayerSkills.Initialize();
        PlayerInventory = new Inventory();
        LoadUserInterfaces();

        WorldGen = GameObject.Find("World Generator").GetComponent<WorldGenerator>();
        WorldGen.CreateWorld(false);
        AudioManager.instance.PlayOpenWorld();
    }

    private void LoadSavedGame()
    {
        CurrentGameState = GameState.INGAME;
        PlayerStats = SaveLoadManager.Instance.ReadPlayerDataFromFile();
        CurrentGameMode = PlayerStats.CurrentGameMode;
        CurrentDifficulty = PlayerStats.CurrentDifficutly;

        LoadWaveCount();

        FormatInventory();
        LoadData.Invoke();
        LoadData = null;

        //Load Player Transform
        playerReference.transform.position = new Vector3(PlayerStats.playerPos[0], PlayerStats.playerPos[1], PlayerStats.playerPos[2]);

        playerReference.transform.rotation.Set(PlayerStats.playerRot[1], PlayerStats.playerRot[2], PlayerStats.playerRot[3], PlayerStats.playerRot[0]);

        PlayerInventory = PlayerStats.PlayerInventory;
        PlayerSkills = PlayerStats.PlayerSkills;
        
        // Load player skill stats when game load
        PlayerSkills.LoadUpgrades();
        
        LoadUserInterfaces();

        WorldGen = GameObject.Find("World Generator").GetComponent<WorldGenerator>();
        WorldGen.LoadWorldData(PlayerStats.WorldGenSaveData);
        WorldGen.CreateWorld(true);
        WorldGen.LoadWorldObjects(PlayerStats.WorldGenSaveData);
        AudioManager.instance.PlayOpenWorld();

        StartCoroutine(ResetLoadingSavedGame());
    }

    private void LoadWaveCount()
    {
        if (CurrentGameMode == GameMode.NORMAL)
        {
            WaveCountToWin = CurrentDifficulty switch
            {
                Difficulty.EASY => EasyWaveCount,
                Difficulty.NORMAL => NormalWaveCount,
                Difficulty.HARD => HardWaveCount,
                _ => 1,
            };
        }
        else if (CurrentGameMode == GameMode.ENDLESS)
        {
            WaveCountToWin = -1;
        }
    }

    private void FormatInventory()
    {
        for (int i = 0; i < PlayerStats.PlayerInventory.Items.Length; i++)
        {
            if (PlayerStats.PlayerInventory.Items[i].item == null)
            {
                PlayerStats.PlayerInventory.Items[i] = null;
            }
        }
    }

    private void LoadUserInterfaces()
    {
        foreach (var ui in UserInterfaces)
            ui.Load();
    }

    // Function to update the Player Statistics object with the latest data.
    public void UpdatePlayerStatistics()
    {
        PlayerStats.CurrentDifficutly = CurrentDifficulty;
        PlayerStats.CurrentGameMode = CurrentGameMode;
        PlayerStats.PlayerInventory = PlayerInventory;
        PlayerStats.PlayerSkills = PlayerSkills;

        //Save Player Transform
        PlayerStats.playerPos[0] = playerReference.transform.position.x;
        PlayerStats.playerPos[1] = playerReference.transform.position.y;
        PlayerStats.playerPos[2] = playerReference.transform.position.z;

        PlayerStats.playerRot[0] = playerReference.transform.rotation.w;
        PlayerStats.playerRot[1] = playerReference.transform.rotation.x;
        PlayerStats.playerRot[2] = playerReference.transform.rotation.y;
        PlayerStats.playerRot[3] = playerReference.transform.rotation.z;


        // Update stuff with unity action.
        SaveData.Invoke();
        SaveData = null;
        WorldGen.SaveWorldData(PlayerStats.WorldGenSaveData);
    }

    public void ResetPlayerStats()
    {
        PlayerStats = null;
        PlayerStats = new PlayerStatistics();
    }

    private IEnumerator ResetLoadingSavedGame()
    {
        yield return new WaitForSeconds(5);
        LoadingSavedGame = false;
    }
}
