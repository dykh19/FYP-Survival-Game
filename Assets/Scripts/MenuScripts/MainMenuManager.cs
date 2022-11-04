using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Declare Variables
    public GameObject MainMenuUI;
    public GameObject OptionsUI;
    public GameObject CreditsUI;
    public GameObject NewGameUI;
    public GameObject ControlsUI;
    public Button EasyButton;
    public Button NormalButton;
    public Button HardButton;
    public Button NormalModeButton;
    public Button EndlessModeButton;

    public void Awake()
    {
        EasyButton.onClick.AddListener(delegate { GameManager.Instance.SetDifficulty(0); });
        NormalButton.onClick.AddListener(delegate { GameManager.Instance.SetDifficulty(1); });
        HardButton.onClick.AddListener(delegate { GameManager.Instance.SetDifficulty(2); });
        NormalModeButton.onClick.AddListener(delegate { GameManager.Instance.SetGameMode(0); });
        EndlessModeButton.onClick.AddListener(delegate { GameManager.Instance.SetGameMode(1); GameManager.Instance.SetDifficulty(1);});
    }

    //Quit game
    public void QuitGame()
    {
        Application.Quit();
    }

    //Start Game
    public void StartGame()
    {
        GameManager.Instance.LoadingSavedGame = false;
        SceneManager.LoadScene(1);
    }   
    
    public void LoadGame()
    {
        GameManager.Instance.LoadingSavedGame = true;
        SceneManager.LoadScene(1);
    }

    //Open options screen
    public void OpenOptions()
    {
        MainMenuUI.SetActive(false);
        OptionsUI.SetActive(true);
    }

    //Open credits screen
    public void OpenCredits()
    {
        MainMenuUI.SetActive(false);
        CreditsUI.SetActive(true);
    }

    //Open mode select screen
    public void OpenModeSelect()
    {
        MainMenuUI.SetActive(false);
        NewGameUI.transform.Find("ModeSelectUI").gameObject.SetActive(true);
    }

    //Open difficulty select screen
    public void OpenDifficultySelect()
    {
        NewGameUI.transform.Find("ModeSelectUI").gameObject.SetActive(false);
        NewGameUI.transform.Find("DifficultySelectUI").gameObject.SetActive(true);
    }

    public void OpenMainMenu()
    {
        MainMenuUI.SetActive(true);
        OptionsUI.SetActive(false);
        CreditsUI.SetActive(false);
        ControlsUI.SetActive(false);
        NewGameUI.transform.Find("ModeSelectUI").gameObject.SetActive(false);
        NewGameUI.transform.Find("DifficultySelectUI").gameObject.SetActive(false);
    }

    //Open controls screen
    public void OpenControls()
    {
        MainMenuUI.SetActive(false);
        ControlsUI.SetActive(true);
    }
}
