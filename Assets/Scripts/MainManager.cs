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

    public Text ScoreText;
    Text BestScoreText;

    public GameObject GameOverTextObject;
    public GameObject BestScoreGameObject;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        BestScoreText = BestScoreGameObject.GetComponent<Text>();

        SetBestScoreGameText();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
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

    private void SetBestScoreGameText()
    {
        if (GameManager.Instance.SaveFileLoaded())
        {
            int HighScore;
            if (GameManager.Instance.HighScore >= m_Points)
            {
                HighScore = GameManager.Instance.HighScore;
            }
            else
            {
                HighScore = m_Points;
                GameManager.Instance.HighScoreName = GameManager.Instance.CurrentSessionName;
                GameManager.Instance.HighScore = HighScore;
                GameManager.Instance.SaveScore();
            }
            BestScoreText.text = $"Best Score {GameManager.Instance.HighScoreName} : {HighScore}";
        }
        else
        {
            GameManager.Instance.HighScoreName = GameManager.Instance.CurrentSessionName;
            GameManager.Instance.HighScore = m_Points;
            GameManager.Instance.SaveScore();

            BestScoreText.text = $"Best Score {GameManager.Instance.CurrentSessionName} : {m_Points}";
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            SetBestScoreGameText();

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
            SetBestScoreGameText();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        GameOverTextObject.SetActive(true);
    }
}
