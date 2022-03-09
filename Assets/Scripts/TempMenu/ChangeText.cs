using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    private Slider slider;
    private bool antiexcept = false;
    private void Start()
    {
        slider = GameObject.Find("Slider").GetComponent<Slider>();
        float value = PlayerPrefs.GetFloat("audioDelay", 0);
        slider.value = value/0.01f;
        GetComponent<Text>().text = value.ToString();
        antiexcept = true;
    }

    public void setText()
    {
        if(antiexcept) GetComponent<Text>().text = (slider.value*0.01f).ToString();
    }
}
