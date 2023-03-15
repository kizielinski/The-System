using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Prison");
    }

    public void CloseGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
