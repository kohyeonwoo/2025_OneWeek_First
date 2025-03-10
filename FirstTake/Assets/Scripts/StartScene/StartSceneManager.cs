using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    public void SceneChange()
    {
        SceneManager.LoadScene("GameScene");
    }
}
