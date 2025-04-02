using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthController : MonoBehaviour
{
    public GameObject rightHealthBar;
    public GameObject leftHealthBar;

    private Slider rightSlider;
    private Slider leftSlider;

    void Start()
    {
        rightSlider = rightHealthBar.GetComponent<Slider>();
        leftSlider = leftHealthBar.GetComponent<Slider>();
    }

    public void UpdateHealth(float value = 0)
    {
        rightSlider.value = value;
        leftSlider.value = value;
    }
    public void ShowBar()
    {
        rightHealthBar.SetActive(true);
        leftHealthBar.SetActive(true);
    }
    public void HideBar()
    {
        rightHealthBar.SetActive(false);
        leftHealthBar.SetActive(false);
    }
}
