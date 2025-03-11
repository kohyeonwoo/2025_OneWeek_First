using System.Collections;
using System.Collections.Generic;
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

}
