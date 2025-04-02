using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class Initialization : MonoBehaviour
{
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        //Go to Main menu
        GetComponent<SceneChanger>().changeScene = "MainMenu";
    }
}
