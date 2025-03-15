using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;

    public int killCount;

    private bool isPause;

    public Text killCountText;

    public GameObject pausePanel;

    public GameObject endGamePanel;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        StartMenuMusic();

        killCount = 0;

        isPause = false;
    }

    private void Update()
    {
        killCountText.text = killCount.ToString();
    }

    public void StartMenuMusic()
    {
        AudioManager.Instance.PlayMusic("MenuMusic");
    }
    
    public void StartOstMusic()
    {
        AudioManager.Instance.PlayMusic("MainOst1Music");
    }

    public void StartCrowdSound()
    {
        AudioManager.Instance.PlayMusic("CrowdMusic");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Pause()
    {
        if (isPause == false)
        {
            Time.timeScale = 0;
            isPause = true;
            return;
        }
    }

    public void UnPause()
    {
        if (isPause == true)
        {
            Time.timeScale = 1;
            isPause = false;
            return;
        }
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
