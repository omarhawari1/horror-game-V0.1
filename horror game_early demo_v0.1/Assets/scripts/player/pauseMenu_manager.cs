using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu_manager : MonoBehaviour
{
    [SerializeField]private GameObject pauseMenu;
    [SerializeField]private player_main player_Main;
    [SerializeField]private GameObject settings;

    public void resume()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        player_Main.paused = false;
    }
    public void settingsMenu()
    {
        settings.SetActive(true);
        pauseMenu.SetActive(false);
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
