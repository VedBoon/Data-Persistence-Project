using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text BestScoreMenuText;

    public Text NameText;
    public static GameManager Instance;
    private bool m_isSaveFileAvailable;

    public int HighScore;
    public string HighScoreName;

    public string CurrentSessionName;

    public bool SaveFileLoaded()
    {
        return m_isSaveFileAvailable;
    }

    [System.Serializable]
    class SaveData
    {
        public string Name;
        public int Score;
    }

    public void SaveScore()
    {
        SaveData data = new SaveData
        {
            Score = HighScore,
            Name = HighScoreName
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
        m_isSaveFileAvailable = true;
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if (File.Exists(path))
        {
            m_isSaveFileAvailable = true;

            string json = File.ReadAllText(path);
            SaveData Data = JsonUtility.FromJson<SaveData>(json);

            HighScoreName = Data.Name;
            HighScore = Data.Score;
        }

    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);

        LoadScore();
    }

    // Start is called before the first frame update
    void Start()
    {
        string Path = Application.persistentDataPath + "/savedata.json";
        
        if (File.Exists(Path))
        {
            BestScoreMenuText.text = "Best Score: " + HighScoreName + " : " + HighScore;
        }
    }
}
