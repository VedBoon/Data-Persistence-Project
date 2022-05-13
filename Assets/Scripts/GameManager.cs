using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject BestScoreMenuObject;
    public GameObject NameText;

    public static GameManager Instance;

    public int HighScore;
    public string HighScoreName;

    public string CurrentSessionName;

    public readonly string path = Application.persistentDataPath + "/savedata.json";

    [System.Serializable]
    class SaveData
    {
        public string Name;
        public int Score;
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.Score = HighScore;
        data.Name = HighScoreName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if (File.Exists(path))
        {
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
        
        if (File.Exists(path))
        {
            BestScoreMenuObject.GetComponent<Text>().text = "Best Score: " + HighScoreName + " : " + HighScore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
}
