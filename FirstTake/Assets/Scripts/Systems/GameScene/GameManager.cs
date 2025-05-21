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

    public TextMeshProUGUI playerMoneyText;

    public int randomNum;

    public int playerMoney;

    public int blueGemCount;

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

        blueGemCount = 0;

    }

    private void Update()
    {

        playerMoneyText.text = playerMoney.ToString();

        time += Time.deltaTime; 
   
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

    public void IncreaseMoney()
    {
        playerMoney += 100;
    }

    public void DecreaseMoney()
    {
        if(playerMoney < 100)
        {
            return;
        }

        playerMoney -= 100;
    }
  
    public void IncreaseBlueGemCount()
    {
        blueGemCount++;
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
