using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }

    string SaveFilePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SaveFilePath = Application.persistentDataPath + "/gamedata.json";
        PrintDebug();
    }

    string ReadFile()
    {
        if (File.Exists(SaveFilePath))
        {
            string FileContents = File.ReadAllText(SaveFilePath);
            return FileContents;
        }
        else
        {
            return null;
        }
    }

    void WriteFile(string DataToSave)
    {
        SaveFilePath = Application.persistentDataPath + "/gamedata.json";
        File.WriteAllText(SaveFilePath, DataToSave);
    }

    public void SavePlayerDataToFile()
    {
        GameManager.Instance.UpdatePlayerStatistics();
        PlayerStatistics DataToSave = GameManager.Instance.PlayerStats;
        string DataToSaveJson = JsonUtility.ToJson(DataToSave);
        WriteFile(DataToSaveJson);
    }

    public PlayerStatistics ReadPlayerDataFromFile()
    {
        string DataToLoadJson = ReadFile();
        if (DataToLoadJson != null)
        {
            PlayerStatistics LoadedData = JsonUtility.FromJson<PlayerStatistics>(DataToLoadJson);
            return LoadedData;
        }
        else
        {
            return null;
        }
    }

    public void PrintDebug()
    {
        Debug.Log(SaveFilePath);
        Debug.Log(Application.persistentDataPath);
    }
}
