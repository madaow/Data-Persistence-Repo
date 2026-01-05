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

    private const string WebGLKey = "defaultsavedata_json";
    private static string DefaultPath => Application.persistentDataPath + "/defaultsavedata.json";

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();
    }

    public void SaveData(string path = null)
    {
        var data = new GameData
        {
            playerName = playerName,
            BestplayerName = BestplayerName,
            Score = Score,
            BestScore = BestScore
        };

        string json = JsonUtility.ToJson(data);

#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL: ブラウザストレージへ
        PlayerPrefs.SetString(WebGLKey, json);
        PlayerPrefs.Save(); // これ重要
#else
        // Standalone/Editor: ファイルへ
        path ??= DefaultPath;
        File.WriteAllText(path, json);
#endif
    }

    public void LoadData(string path = null)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (!PlayerPrefs.HasKey(WebGLKey)) return;

        string json = PlayerPrefs.GetString(WebGLKey, "");
        if (string.IsNullOrEmpty(json)) return;

        GameData data = JsonUtility.FromJson<GameData>(json);
        BestScore = data.BestScore;
        BestplayerName = data.BestplayerName;
#else
        path ??= DefaultPath;
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        GameData data = JsonUtility.FromJson<GameData>(json);
        BestScore = data.BestScore;
        BestplayerName = data.BestplayerName;
#endif
    }

    public void ResetData(string path = null)
    {
        BestplayerName = "";
        BestScore = 0;

#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerPrefs.DeleteKey(WebGLKey);
        PlayerPrefs.Save();
#else
        SaveData(path);
#endif
    }
}
