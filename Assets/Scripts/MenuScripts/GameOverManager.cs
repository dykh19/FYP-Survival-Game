using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    //Declare UI elements (Eg. Score text and stuff)
    public TMP_Text WaveCleared;
    public TMP_Text EnemiesKilled;
    public TMP_Text OresObtained;
    public TMP_Text EssenceObtained;
    public TMP_Text TotalScore;

    public void Start()
    {
        UpdateTextFields();
    }

    public void UpdateTextFields()
    {
        WaveCleared.text = GameManager.Instance.PlayerStats.WavesCleared.ToString();
        EnemiesKilled.text = GameManager.Instance.PlayerStats.TotalEnemiesKilled.ToString();
        OresObtained.text = GameManager.Instance.PlayerStats.TotalOresObtained.ToString();
        EssenceObtained.text = GameManager.Instance.PlayerStats.TotalEssenceObtained.ToString();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

