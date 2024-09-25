using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{

    public GameObject player;
    public GameObject pauseText;
    public GameObject pauseMenu;

    public bool unPausable = true;
    public bool paused = true;

    private PlayerController playerController;
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {

    }
    public void Pause()
    {
        if (!paused)
        {
            pauseText.SetActive(true);
            Time.timeScale = 0;
            paused = true;
        }
    }
    public void Unpause()
    {
        if (!unPausable)
        {
            pauseText.SetActive(false);
            Time.timeScale = 1;
            paused = false;
        }
    }
    public void OpenPauseMenu()
    {
        playerController.DisableJoysticks();
        Pause();
        pauseMenu.SetActive(true);
    }
    public void ClosePauseMenu()
    {
        playerController.EnableJoysticks();
        Unpause();
        pauseMenu.SetActive(false);
    }
    public void OpenShopMenu()
    {
        playerController.DisableJoysticks();
        unPausable = true;
        pauseText.SetActive(false);
        Pause();
    }
    public void CloseShopMenu()
    {
        playerController.EnableJoysticks();
        unPausable = false;
        Unpause();
    }
}
