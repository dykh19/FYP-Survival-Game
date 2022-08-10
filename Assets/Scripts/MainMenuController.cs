using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Declare Variables
    public GameObject MainMenuUI;
    public GameObject OptionsUI;
    public GameObject CreditsUI;
    //public GameObject ModeSelectUI;
    //public GameObject DifficultySelectUI;
    public GameObject NewGameUI;

    //Quit game
    public void QuitGame()
    {
        Application.Quit();
    }

    //Start Game
    public void StartGame()
    {
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
        //ModeSelectUI.SetActive(false);
        //DifficultySelectUI.SetActive(true);
    }

    //Close options screen
    public void CloseOptions()
    {
        OptionsUI.SetActive(false);
        MainMenuUI.SetActive(true);
        
    }

    //Close credits screen
    public void CloseCredits()
    {
        CreditsUI.SetActive(false);
        MainMenuUI.SetActive(true);
    }
}
