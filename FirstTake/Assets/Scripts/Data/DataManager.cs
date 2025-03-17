using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataManager : MonoBehaviour
{
    private static GameObject container;
    private static DataManager instance;

    public string gameDataFileName = "gameData.json";
    public GameData gameData;


    static GameObject Container
    {
        get
        {
            return container;
        }
    }

    public GameData GameData
    {
        get
        {

            if (gameData == null)
            {
                LoadGameData();
                SaveGameData();
            }

            return gameData;
        }
    }

    public static DataManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            
            return instance;
        }
    }

    public void SaveGameData()
    {
        string toJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + gameDataFileName;
        File.WriteAllText(filePath, toJsonData);
        Debug.Log("데이터 저장 완료");
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + gameDataFileName;

        if (File.Exists(filePath))
        {
            Debug.Log("데이터 불러오기 완료");
            string fromJsonData = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(fromJsonData);
        }
        else
        {
            Debug.Log("해당 데이터는 존재하지 않습니다");
            gameData = new GameData();
        }
    }


    private void OnApplicationQuit()
    {
        SaveGameData();
    }

}
