using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public List<GameObject> menuScenes;
    public GameObject backButton;
    public GameObject playButton;
    public GameObject eventSystem;
    // 0 - main
    // 1 - game mode
    // 2 - upgrades
    // 3 - story mode
    // 4 - infinity mode
    // 5 - shop
    public Stack<int> sceneHistory = new Stack<int>();
    int activeScene;

    public float sideChangeSpeed;
    public LeanTweenType sideChangeAnimationType;

    void Start()
    {
        sceneHistory.Push(0);
    }

    void Update()
    {

    }
    public void ResetMenu()
    {
        menuScenes[0].SetActive(true);
        sceneHistory = new Stack<int>();
        sceneHistory.Push(0);
    }

    public void SwapRightScene(int sceneID)
    {
        sceneHistory.Push(activeScene);
        SwapScene(sceneID, true);
        StartCoroutine(SwapSceneCleanup(sceneID, true));
    }
    public void SwapLeftScene(int sceneID)
    {
        sceneHistory.Push(activeScene);
        SwapScene(sceneID, false);
        StartCoroutine(SwapSceneCleanup(sceneID, true));
    }

    void SwapScene(int sceneID, bool swapRight)
    {
        // disable user input
        eventSystem.SetActive(false);
        // activate new scene
        menuScenes[sceneID].SetActive(true);
        // reposition new scene
        if (swapRight)
            menuScenes[sceneID].GetComponent<RectTransform>().anchoredPosition = new Vector3(800, 0, 0);
        else
            menuScenes[sceneID].GetComponent<RectTransform>().anchoredPosition = new Vector3(-800, 0, 0);

        // set animation
        LeanTween.moveX(menuScenes[sceneID], 0, sideChangeSpeed).setEase(sideChangeAnimationType);
        // reposition old scene
        if (swapRight)
            LeanTween.moveX(menuScenes[activeScene].GetComponent<RectTransform>(), -800, sideChangeSpeed).setEase(sideChangeAnimationType);
        else
            LeanTween.moveX(menuScenes[activeScene].GetComponent<RectTransform>(), 800, sideChangeSpeed).setEase(sideChangeAnimationType);

    }

    public void SwapLastScene()
    {
        if (sceneHistory.Peek() == 0)
        {
            playButton.SetActive(true);
            backButton.SetActive(false);
        }
        SwapScene(sceneHistory.Peek(), false);

        StartCoroutine(SwapSceneCleanup(sceneHistory.Peek(), false));
    }

    IEnumerator SwapSceneCleanup(int sceneID, bool isPushing)
    {
        //wait for animation to complete
        yield return new WaitForSeconds(sideChangeSpeed);

        // do cleanup
        if (isPushing)
            menuScenes[sceneHistory.Peek()].SetActive(false);
        else
        {
            menuScenes[activeScene].SetActive(false);
            sceneHistory.Pop();
        }
        activeScene = sceneID;
        // enable user input
        eventSystem.SetActive(true);
    }
}
