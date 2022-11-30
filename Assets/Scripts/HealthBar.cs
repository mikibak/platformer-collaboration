using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(int healt)
    {
        slider.value = healt;
    }

    public void SetMaxHealt(int max)
    {
        slider.maxValue = max;
        slider.value = max;
    }
}
