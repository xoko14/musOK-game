using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Slider delaySlider;
    public Slider chartSpeedSlider;
    public void SaveAndExit()
    {
        PlayerPrefs.SetFloat("audioDelay", delaySlider.value * 0.01f);
        PlayerPrefs.SetFloat("chartSpeed", chartSpeedSlider.value * 0.01f);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
}
