using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject controlsText;
    private bool controlsShown = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLevelOneButtonPress ()
    {
        SceneManager.LoadSceneAsync("Level1");
    }

    public void OnLevelTwoButtonPress ()
    {
        SceneManager.LoadSceneAsync("Level2");
    }

    public void OnExitToDesktopButtonPressed ()
    {
        #if UNITY_EDITOR 
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void ToggleShowControls ()
    {
        controlsShown = !controlsShown;
        controlsText.SetActive(controlsShown);
    }
}
