using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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
    [SerializeField]private AudioMixer audioMixer;
    [SerializeField]private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    
    private void Start()
    {

        mouseSenseX.text = mouseSensX_startingValue.ToString();
        mouseSenseY.text = mouseSensY_startingValue.ToString();

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
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
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void setResolution(int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
