using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScheneAndLevel : MonoBehaviour
{
    public GameObject gameManager;
    public void LoadToScene(string sceneName)
    {
        //Di mana pun Anda memuat scene baru, pastikan "MainMenu" dimuat juga
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene(sceneName);
    }
    public void Exit()
    {
        Application.Quit();
        Debug.Log("Keluar app");
    }
}
