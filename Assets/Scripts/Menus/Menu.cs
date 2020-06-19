using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class Menu : MonoBehaviour
{
    #region Variables
    [Header("Audio")]
    private float masterVol;
    public AudioMixer masterAudio;
    public bool muted;
    [Header("Visual")]
    public bool isFullScreen;
    public Resolution[] resolutions;
    public Dropdown resolution;
    #endregion
    #region Audio
    //function that controls master volume
    public void ChangeMaster(float volume)
    {
        masterVol = volume;
        //only changes volume if not muted
        if (!muted)
        {
            masterAudio.SetFloat("mastervol", volume);
        }
    }
    //function that controls music volume
    public void ChangeMusic(float volume)
    {
        if (!muted)
        {
            masterAudio.SetFloat("musicvol", volume);
        }
    }
    //function that controls sound effect volume
    public void ChangeSounds(float volume)
    {
        if (!muted)
        {
            masterAudio.SetFloat("soundvol", volume);
        }
    }
    //allows for volume to be toggled on/off
    public void ToggleMute(bool isMuted)
    {
        muted = isMuted;
        if (isMuted)
        {
            masterAudio.GetFloat("mastervol", out masterVol);
            masterAudio.SetFloat("mastervol", -80);
        }
        else
        {
            masterAudio.SetFloat("mastervol", masterVol);
        }
    }
    #endregion
    #region Visual
    //function that allows the user to change the graphics quality
    public void Quality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    //function that allows for fullscreen to be toggled on/off
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    private void Start()
    {
        //sets up the resolution options
        resolutions = Screen.resolutions;
        resolution.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolution.AddOptions(options);
        resolution.value = currentResolutionIndex;
        resolution.RefreshShownValue();
    }
    //function that allows for game resolution to be changed
    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
    #endregion
    public void SwapToController(bool controller)
    {
        PlayerHandler.controllerMovement = controller;
    }
}
