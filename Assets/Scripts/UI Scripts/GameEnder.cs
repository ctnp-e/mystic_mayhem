using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnder : MonoBehaviour
{
    public static GameEnder ender;

    public void Awake()
    {
        ender = this;
    }

    public void SwitchToEnd()
    {
        SceneManager.LoadSceneAsync("END");
    }        
}
