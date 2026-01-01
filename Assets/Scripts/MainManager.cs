using System;
using System.IO;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    public string playerName;
    public string BestplayerName;
    public int Score;
    public int BestScore;

    [Serializable]
    public class GameData
    {
        public string playerName = "";
        public string BestplayerName = "";
        public int Score = 0;
        public int BestScore = 0;
    }
    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();
    }

    public void SaveData(string path = null)
    {
        path ??= Application.persistentDataPath + "/defaultsavedata.json";

        GameData data = new GameData();
        data.playerName = playerName;
        data.BestplayerName = BestplayerName;
        data.Score = Score;
        data.BestScore = BestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }
    public void LoadData(string path = null)
    {
        path ??= Application.persistentDataPath + "/defaultsavedata.json";

        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameData data = JsonUtility.FromJson<GameData>(json);

            BestScore = data.BestScore;
            BestplayerName = data.BestplayerName;
        }
    }
    public void ResetData(string path = null)
    {
        BestplayerName = "";
        BestScore = 0;
        SaveData();
    }
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPaused = false;
#endif
    }
}
