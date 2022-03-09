using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnGo : MonoBehaviour
{
    private Text text;
    private Slider slider;

    void Start()
    {
        text = GameObject.Find("TextSong").GetComponent<Text>();
        slider = GameObject.Find("Slider").GetComponent<Slider>();
    }
    public void Pressed()
    {
        SongSaver.SongID = text.text;
        SongSaver.Delay = slider.value;
        SceneManager.LoadScene(1);
    }
}
