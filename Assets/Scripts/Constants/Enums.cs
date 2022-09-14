using System.Collections;
using System.Collections.Generic;

//Difficulty of the game
public enum Difficulty
{
    EASY,
    NORMAL,
    HARD,
    ENDLESS
}

//Game Mode of the game
public enum GameMode
{
    NORMAL,
    ENDLESS
}

//Game States
public enum GameState
{
    START,
    INGAME,
    PAUSED,
    END
}
