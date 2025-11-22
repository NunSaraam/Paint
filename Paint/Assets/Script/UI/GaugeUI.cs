using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeUI : MonoBehaviour
{
    public GameObject background;
    public Image gaugeImage;

    private void Start()
    {
        background.SetActive(false);
    }

    public void GaugeValue(float value)
    {
        gaugeImage.fillAmount = Mathf.Clamp01(value);
    }

    public void ShowGauge(bool isVisible)
    {
        background.SetActive(isVisible);
    }
}
