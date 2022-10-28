using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 17/08/2022.

public class PlayerOpenUI : MonoBehaviour
{
    private UserInterface defaultUI;
    private UserInterface? currentlyActive = null;

    void Start()
    {
        defaultUI = GameManager.Instance.UserInterfaces[0];

        ClearAllUI();
        SetUIActivity(defaultUI, true);
    }

    void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameState.INGAME)
            PollForOpenUI();
        else
            PollForCloseUI();
    }

    private void PollForOpenUI()
    {
        // Open the UI if the player pressed its respective assigned UI key.

        foreach (var ui in GameManager.Instance.UserInterfaces)
            if ((ui.keyCode != KeyCode.None) && Input.GetKeyDown(ui.keyCode))
                OpenUI(ui);
    }

    private void PollForCloseUI()
    {
        // Close the current UI if the player presses either the Escape key
        // or the assigned keycode if specified.

        if (Input.GetKeyDown(KeyCode.Escape) || ((currentlyActive is not null) && Input.GetKeyDown(((UserInterface)currentlyActive).keyCode)))
            CloseUI();
    }

    public void OpenUI(UserInterface ui)
    {
        GameManager.Instance.PauseGame();
        currentlyActive = ui;

        SetUIActivity(defaultUI, false);
        SetUIActivity(ui, true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseUI()
    {
        GameManager.Instance.ResumeGame();
        currentlyActive = null;

        ClearAllUI();
        SetUIActivity(defaultUI, true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private static void ClearAllUI()
    {
        foreach (var ui in GameManager.Instance.UserInterfaces)
            SetUIActivity(ui, false);
    }

    private static void SetUIActivity(UserInterface ui, bool state)
    {
        ui.userInterface.gameObject.SetActive(state);
    }
}

[System.Serializable]
public class UserInterface
{
    public string name;
    public KeyCode keyCode;

    [HideInInspector]
    public Canvas userInterface;

    public void Load()
    {
        var instance = GameObject.Find(name);
        userInterface = instance.GetComponent<Canvas>();
    }
}
