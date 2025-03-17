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

    int rand;

    [SerializeField]
    private float hitDuration = 0.1f;

    private bool isPause;

    public bool bSpawn;

    public Text killCountText;

    public TextMeshProUGUI result_killCountText;
    
    public TextMeshProUGUI result_Stage;

    public GameObject pausePanel;

    public GameObject endGamePanel;

    public GameObject hitEffect;

    public List<GameObject> characters = new List<GameObject>();

    public List<GameObject> charactersHealth = new List<GameObject>();

    public List<GameObject> checkImages = new List<GameObject>();

    public List<GameObject> lockImages = new List<GameObject>();

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

        result_KillCount = 0;

        isPause = false;

        bSpawn = false;

        DataManager.Instance.LoadGameData();
    }

    private void Update()
    {

        DataManager.Instance.LoadGameData();

        rand = Random.Range(0, 4);

        lockImages[rand].SetActive(DataManager.Instance.gameData.lockImageOff[rand]);

        killCountText.text = killCount.ToString();

        result_KillCount = killCount;

        result_killCountText.text = result_KillCount.ToString();

        DataManager.Instance.SaveGameData();
 
    }

    public void OffImage()
    {
        
        DataManager.Instance.gameData.lockImageOff[rand] = false;

        lockImages[rand].SetActive(DataManager.Instance.gameData.lockImageOff[rand]);

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

    public void PlayHitEffect()
    {
        StartCoroutine(ActiveHitEffect());
    }

    private IEnumerator ActiveHitEffect()
    {

        hitEffect.SetActive(true);

        float elapsed = 0.0f;

        while(elapsed < hitDuration)
        {
            elapsed += Time.deltaTime;

            yield return null;
        }

        hitEffect.SetActive(false);

    }

    public void StartSpawnEnemy()
    {
        bSpawn = true;
    }

    public void SetActivePausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void SetActiveEndPanel()
    {
        endGamePanel.SetActive(true);   
    }

    public void CharacterActiveCase(int index)
    {
        switch(index)
        {
            case 0:
                characters[0].SetActive(true);
                charactersHealth[0].SetActive(true);
                checkImages[0].SetActive(true);

                characters[1].SetActive(false);
                charactersHealth[1].SetActive(false);
                checkImages[1].SetActive(false);

                characters[2].SetActive(false);
                charactersHealth[2].SetActive(false);
                checkImages[2].SetActive(false);

                characters[3].SetActive(false);
                charactersHealth[3].SetActive(false);
                checkImages[3].SetActive(false);

                characters[4].SetActive(false);
                charactersHealth[4].SetActive(false);
                checkImages[4].SetActive(false);

                characters[5].SetActive(false);
                charactersHealth[5].SetActive(false);
                checkImages[5].SetActive(false);

                break;

            case 1:
                characters[0].SetActive(false);
                charactersHealth[0].SetActive(false);
                checkImages[0].SetActive(false);

                characters[1].SetActive(true);
                charactersHealth[1].SetActive(true);
                checkImages[1].SetActive(true);

                characters[2].SetActive(false);
                charactersHealth[2].SetActive(false);
                checkImages[2].SetActive(false);

                characters[3].SetActive(false);
                charactersHealth[3].SetActive(false);
                checkImages[3].SetActive(false);

                characters[4].SetActive(false);
                charactersHealth[4].SetActive(false);
                checkImages[4].SetActive(false);

                characters[5].SetActive(false);
                charactersHealth[5].SetActive(false);
                checkImages[5].SetActive(false);

                break;

            case 2:
                characters[0].SetActive(false);
                charactersHealth[0].SetActive(false);
                checkImages[0].SetActive(false);

                characters[1].SetActive(false);
                charactersHealth[1].SetActive(false);
                checkImages[1].SetActive(false);

                characters[2].SetActive(true);
                charactersHealth[2].SetActive(true);
                checkImages[2].SetActive(true);

                characters[3].SetActive(false);
                charactersHealth[3].SetActive(false);
                checkImages[3].SetActive(false);

                characters[4].SetActive(false);
                charactersHealth[4].SetActive(false);
                checkImages[4].SetActive(false);

                characters[5].SetActive(false);
                charactersHealth[5].SetActive(false);
                checkImages[5].SetActive(false);

                break;

            case 3:
                characters[0].SetActive(false);
                charactersHealth[0].SetActive(false);
                checkImages[0].SetActive(false);

                characters[1].SetActive(false);
                charactersHealth[1].SetActive(false);
                checkImages[1].SetActive(false);

                characters[2].SetActive(false);
                charactersHealth[2].SetActive(false);
                checkImages[2].SetActive(false);

                characters[3].SetActive(true);
                charactersHealth[3].SetActive(true);
                checkImages[3].SetActive(true);

                characters[4].SetActive(false);
                charactersHealth[4].SetActive(false);
                checkImages[4].SetActive(false);

                characters[5].SetActive(false);
                charactersHealth[5].SetActive(false);
                checkImages[5].SetActive(false);

                break;

            case 4:
                characters[0].SetActive(false);
                charactersHealth[0].SetActive(false);
                checkImages[0].SetActive(false);

                characters[1].SetActive(false);
                charactersHealth[1].SetActive(false);
                checkImages[1].SetActive(false);

                characters[2].SetActive(false);
                charactersHealth[2].SetActive(false);
                checkImages[2].SetActive(false);

                characters[3].SetActive(false);
                charactersHealth[3].SetActive(false);
                checkImages[3].SetActive(false);

                characters[4].SetActive(true);
                charactersHealth[4].SetActive(true);
                checkImages[4].SetActive(true);

                characters[5].SetActive(false);
                charactersHealth[5].SetActive(false);
                checkImages[5].SetActive(false);

                break;

            case 5:
                characters[0].SetActive(false);
                charactersHealth[0].SetActive(false);
                checkImages[0].SetActive(false);

                characters[1].SetActive(false);
                charactersHealth[1].SetActive(false);
                checkImages[1].SetActive(false);

                characters[2].SetActive(false);
                charactersHealth[2].SetActive(false);
                checkImages[2].SetActive(false);

                characters[3].SetActive(false);
                charactersHealth[3].SetActive(false);
                checkImages[3].SetActive(false);

                characters[4].SetActive(false);
                charactersHealth[4].SetActive(false);
                checkImages[4].SetActive(false);

                characters[5].SetActive(true);
                charactersHealth[5].SetActive(true);
                checkImages[5].SetActive(true);

                break;

        }
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
