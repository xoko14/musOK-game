using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISliderStep : MonoBehaviour
{
    float stepAmount = 0.01f;
    Slider mySlider = null;

    float numberOfSteps = 0;

    private bool antiex = false;

    // Start is called before the first frame update
    void Start()
    {
        mySlider = GetComponent<Slider>();
        numberOfSteps = (mySlider.maxValue - mySlider.minValue) / stepAmount;
        antiex = true;
    }

    public void UpdateStep()
    {
        if (antiex)
        {
            /*float range = ((mySlider.value + mySlider.maxValue) / (mySlider.maxValue - mySlider.minValue)) * numberOfSteps;
            int ceil = Mathf.RoundToInt(range);
            mySlider.value = ceil * stepAmount-mySlider.maxValue;
            */
            mySlider.value = mySlider.value - mySlider.value % stepAmount;
        }
    }
}
