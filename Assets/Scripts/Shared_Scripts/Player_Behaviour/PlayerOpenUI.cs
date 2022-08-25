using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 17/08/2022.

// TODO: Bug - The pointer doesn't get locked when closing the pause menu.

public class PlayerOpenUI : MonoBehaviour
{
    private UserInterface defaultUI;
    private UserInterface? currentlyActive = null;

    void Awake()
    {
        defaultUI = GameManagerJoseph.Main.userInterfaces[0];
    }

    void Start()
    {
        ClearAllUI();
        SetUIActivity(defaultUI, true);
    }

    void Update()
    {
        if (GameManagerJoseph.Main.isPlaying)
            PollForOpenUI();
        else
            PollForCloseUI();
    }

    private void PollForOpenUI()
    {
        // Open the UI if the player pressed its respective assigned UI key.

        foreach (var ui in GameManagerJoseph.Main.userInterfaces)
            if ((ui.keyCode != KeyCode.None) && Input.GetKeyDown(ui.keyCode))
                OpenUI(ui);
    }

    private void PollForCloseUI()
    {
        // Close the current UI if the player presses either the Escape key
        // or the assigned keycode if specified.

        if (Input.GetKeyDown(KeyCode.Escape) || (currentlyActive is not null) && Input.GetKeyDown(((UserInterface)currentlyActive).keyCode))
            CloseUI();
    }

    public void OpenUI(UserInterface ui)
    {
        GameManagerJoseph.Main.isPlaying = false;
        currentlyActive = ui;

        SetUIActivity(defaultUI, false);
        SetUIActivity(ui, true);

        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseUI()
    {
        GameManagerJoseph.Main.isPlaying = true;
        currentlyActive = null;

        ClearAllUI();
        SetUIActivity(defaultUI, true);

        Cursor.lockState = CursorLockMode.Locked;
    }

    private static void ClearAllUI()
    {
        foreach (var ui in GameManagerJoseph.Main.userInterfaces)
            SetUIActivity(ui, false);
    }

    private static void SetUIActivity(UserInterface ui, bool state)
    {
        ui.userInterface.gameObject.SetActive(state);
    }
}

[System.Serializable]
public struct UserInterface
{
    public string name;
    public KeyCode keyCode;
    public Canvas userInterface;
}
