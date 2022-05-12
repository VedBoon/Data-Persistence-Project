using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text BestScoreText;
    public Text ScoreText;
    public GameObject GameOverText;

    public GameObject NameText;
    //public string currentSessionName;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    public static MainManager Instance;

    public int Score;
    public string Name;



    [System.Serializable]
    class SaveData
    {
        public int Score;
        public string Name;
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.Score = Score;
        data.Name = Name;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
    }
    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savedata.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            Name = data.Name;
            Score = data.Score;
        }
    }

    private void Awake()
    {
       if (Instance != null)
        {
            Destroy(gameObject);
                return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load Best Score (String:Int)
        LoadScore();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameOverText)
        GameOverText.SetActive(false);
    }

    public void Setup()
    {
        
        GameObject BallGameObject = GameObject.FindGameObjectWithTag("Ball");
        Ball = BallGameObject.GetComponent<Rigidbody>();

        GameObject[] TextObjects = GameObject.FindGameObjectsWithTag("Text");

        foreach (GameObject t in TextObjects)
        {
            //Debug.Log("Text Name: " + t.GetComponent<Text>().name);
            Debug.Log("Text Name: " + t.name);

            if (t.name.Equals("ScoreText"))
            {
                ScoreText = t.GetComponent<Text>();
            }
            else if (t.name.Equals("BestScoreText"))
            {
                BestScoreText = t.GetComponent<Text>();
            }
            else if (t.name.Equals("GameoverText"))
            {
                GameOverText = t;
                GameOverText.SetActive(false);
            }
            else
            {
                Debug.Log("Unknown Text");
            }
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex,LoadSceneMode.Single);
                m_Started = false;
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
