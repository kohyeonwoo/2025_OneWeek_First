using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;

    public int killCount;

    public int result_KillCount;

    [SerializeField]
    private float hitDuration = 0.1f;

    private bool isPause;

    public GameObject pausePanel;

    public GameObject endGamePanel;

    public GameObject hitEffect;

    public Camera mainCamera;

    public List<GameObject> characters = new List<GameObject>();

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

        isPause = false;

        DataManager.Instance.LoadGameData();
    }

    private void Update()
    {

        DataManager.Instance.LoadGameData();
     
        DataManager.Instance.SaveGameData();
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
