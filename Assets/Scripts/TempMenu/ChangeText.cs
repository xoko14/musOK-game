using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    public Slider slider;
    public string pref;

    public float defaultVal;
    private bool antiexcept = false;
    private void Start()
    {
        float value = PlayerPrefs.GetFloat(pref, default);
        slider.value = value/0.01f;
        GetComponent<Text>().text = value.ToString();
        antiexcept = true;
    }

    public void setText()
    {
        if(antiexcept) GetComponent<Text>().text = (slider.value*0.01f).ToString();
    }
}
