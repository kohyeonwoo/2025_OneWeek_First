using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        StartMenuMusic();
    }

    public void StartMenuMusic()
    {
        AudioManager.Instance.PlayMusic("MenuMusic");
    }
    
    public void StartOstMusic()
    {
        AudioManager.Instance.PlayMusic("MainOst1Music");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("GameScene");
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
