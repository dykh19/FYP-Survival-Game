using System;
using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 20/08/2022.

// TODO: Play sound on button click.

public class PauseUI : UIController
{
    public Sprite buttonSprite;
    public Color buttonColour;
    public Color buttonTextColour;
    public Vector2 buttonSize;
    public float buttonFontSize;
    public float buttonDistance;

    void Start()
    {
        var button1Position = Vector2.up * (buttonDistance / 2);
        var button2Position = Vector2.down * (buttonDistance / 2);

        Action BackToGame = GameManagerJoseph.Main
            .playerStatus
            .GetComponent<PlayerOpenUI>()
            .CloseUI;

        CreateMenuButton("Back to Game", button1Position, BackToGame);
        CreateMenuButton("Exit to Main Menu", button2Position, ReturnToMainMenu);
    }

    private void CreateMenuButton(string name, Vector2 position, Action Callback)
    {
        CreateButton(name, position, buttonSize, buttonSprite,
            buttonColour, buttonFontSize, buttonTextColour, Callback);
    }

    private void ReturnToMainMenu()
    {
        // TODO
    }
}
