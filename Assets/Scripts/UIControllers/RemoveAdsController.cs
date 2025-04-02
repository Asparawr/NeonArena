using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAdsController : MonoBehaviour
{
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("SaveManager")[0].GetComponent<SaveManager>().state.adBlock == true)
        {
            gameObject.SetActive(false);
        }
    }
    void OnEnable()
    {
        if (GameObject.FindGameObjectsWithTag("SaveManager")[0].GetComponent<SaveManager>().state.adBlock == true)
        {
            gameObject.SetActive(false);
        }
    }
}
