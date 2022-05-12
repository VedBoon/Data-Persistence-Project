using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIHandler : MonoBehaviour
{

    public void StartNew()
    {
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }

    public void Exit()
    {
        MainManager.Instance.SaveScore();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
    }


    // Start is called before the first frame update
    void Start()
    {
        
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            MainManager.Instance.Setup();
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            //MainManager.Instance.currentSessionName = 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
