using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public float dimScreenTimer = 0;
    public float dimScreenMaxTimer = 0;
    public float undimScreenTimer = 0;
    public string changeScene = "";
    public GameObject dimImage;
    public SaveManager saveManager;
    bool isdimming = false;
    public bool isLoading = false;
    AsyncOperation loadingOperation;
    public AudioManager audioManager;
    void Update()
    {
        if (dimScreenTimer > 0)
        {
            isdimming = true;
            dimScreenTimer -= Time.unscaledDeltaTime;
            dimImage.SetActive(true);
            audioManager.GetComponent<AudioSource>().volume = 1 - dimScreenTimer / dimScreenMaxTimer;
            dimImage.GetComponent<Image>().color = new Color(0, 0, 0, 1 - dimScreenTimer / dimScreenMaxTimer);
        }
        else if (changeScene != "")
        {
            loadingOperation = SceneManager.LoadSceneAsync(changeScene);
            changeScene = "";
            undimScreenTimer = dimScreenMaxTimer;
        }
        else if (loadingOperation != null && loadingOperation.isDone)
        {
            if (undimScreenTimer > 0)
            {
                undimScreenTimer -= Time.unscaledDeltaTime;
                audioManager.GetComponent<AudioSource>().volume = 1 - dimScreenTimer / dimScreenMaxTimer;
                dimImage.GetComponent<Image>().color = new Color(0, 0, 0, undimScreenTimer / dimScreenMaxTimer);
            }
            else if (isdimming == true)
            {
                dimImage.SetActive(false);
                isdimming = false;
                GameObject[] gameControllers = GameObject.FindGameObjectsWithTag("GameController");
                if (gameControllers.Length > 0)
                {
                    GameObject gameController = gameControllers[0];
                    if (isLoading)
                        gameController.GetComponent<GameSetup>().LoadVariables(saveManager, this);
                    else
                        gameController.GetComponent<GameSetup>().SetupVariables(GetComponent<GameSetupVariables>(), saveManager, this);
                    isLoading = false;
                }
            }
        }
    }
}
