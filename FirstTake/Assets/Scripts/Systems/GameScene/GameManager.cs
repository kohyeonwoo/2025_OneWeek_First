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

    public GameObject changeParticle;

    public GameObject changeParticleLocation;

    public List<GameObject> spirits = new List<GameObject>();

    public List<GameObject> Monsters_Spike = new List<GameObject>();

    public int randomNum;

    public float time;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }        
    }

    private void Start()
    {   
        isPause = false;

        randomNum = Random.Range(0, 4);

        time = 0.0f;

        spirits[randomNum].SetActive(true);
    }

    private void Update()
    {
        time += Time.deltaTime; 
        
        if(time > 100.0f)
        {
            Evolve();
        }

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

    public void Evolve()
    {
        for(int i =0; i < spirits.Count; i++)
        {
            spirits[i].SetActive(false);
        }

        Monsters_Spike[0].SetActive(true);

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
