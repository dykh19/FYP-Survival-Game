using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // Declare Variables
    public GameObject mainMenuUI;
    public GameObject optionsUI;
    public GameObject creditsUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Quit game
    public void QuitGame()
    {
        Application.Quit();
    }

    //Open options screen
    public void OpenOptions()
    {
        mainMenuUI.SetActive(false);
        optionsUI.SetActive(true);
    }

    //Open credits screen
    public void OpenCredits()
    {
        mainMenuUI.SetActive(false);
        creditsUI.SetActive(true);
    }

    //Close options screen
    public void CloseOptions()
    {
        optionsUI.SetActive(false);
        mainMenuUI.SetActive(true);
        
    }

    //Close credits screen
    public void CloseCredits()
    {
        creditsUI.SetActive(false);
        mainMenuUI.SetActive(true);
        
    }
}
