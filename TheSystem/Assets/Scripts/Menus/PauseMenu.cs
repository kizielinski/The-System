using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //public static 
    [SerializeField] string scenePath;

    public PlayerController player;
    // Update is called once per frame
    void Update()
    {
    }

    public void Quit()
    {
        //loads the main menu
        SceneManager.LoadScene(scenePath);
    }

    public void Resume()
    {
        //disables the pause menu
        player.paused = false;
    }
}
