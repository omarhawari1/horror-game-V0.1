using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class settings : MonoBehaviour
{
    [SerializeField]private player_main player_Main;
    [SerializeField]private TMP_InputField mouseSenseY;
    [SerializeField]private TMP_InputField mouseSenseX;
    [SerializeField]private float mouseSensX_startingValue;
    [SerializeField]private float mouseSensY_startingValue;
    [SerializeField]private GameObject pauseMenu;

    private void Awake()
    {
        mouseSenseX.text = mouseSensX_startingValue.ToString();
        mouseSenseY.text = mouseSensY_startingValue.ToString();
    }
    private void Update() 
    {
        player_Main.mouseXSens = float.Parse(mouseSenseX.text);
        player_Main.mouseYSens = float.Parse(mouseSenseY.text);
    }
    public void back()
    {
        gameObject.SetActive(false);
        pauseMenu.SetActive(true);
    }
}
