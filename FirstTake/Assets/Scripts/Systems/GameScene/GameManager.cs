using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;

    private bool isPause;

    public GameObject pausePanel;

    public GameObject endGamePanel;

    public List<GameObject> spirits = new List<GameObject>();

    public int randomNum;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }        
    }

    void Start()
    {   
        isPause = false;

        randomNum = Random.Range(0, 2);

        spirits[randomNum].SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Pause()
    {
        //if (isPause == false)
        //{
        //    Time.timeScale = 0;
        //    isPause = true;
        //    return;
        //}
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        //if (isPause == true)
        //{
        //    Time.timeScale = 1;
        //    isPause = false;
        //    return;
        //}
        Time.timeScale = 1;
    }

    public void SetActivePausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void SetActiveEndPanel()
    {
        endGamePanel.SetActive(true);   
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
