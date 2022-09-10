using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    //Declare UI elements (Eg. Score text and stuff)
    TextMeshPro WaveCleared;
    TextMeshPro EnemiesKilled;
    TextMeshPro ResourcesObtained;
    TextMeshPro TotalScore;


    public void UpdateTextFields()
    {

    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

