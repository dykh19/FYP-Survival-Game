using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Difficulty
    {
        EASY,
        NORMAL,
        HARD
    }

    public enum GameMode
    {
        NORMAL,
        ENDLESS
    }

    public static GameManager Instance { get; private set; }
    public GameMode CurrentMode;
    public Difficulty CurrentDifficulty;

    void Awake()
    {
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameMode(int newMode)
    {
        CurrentMode = (GameMode)newMode;
    }

    public void SetDifficulty(int newDifficulty)
    {
        CurrentDifficulty = (Difficulty)newDifficulty;
    }
}
