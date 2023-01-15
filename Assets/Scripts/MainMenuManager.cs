using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject controlsText;
    private bool controlsShown = false;
    public Text shieldsText;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("shield", 0);
        SetShieldsButtonText();
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

    public void ToggleShields() 
    {
        int savedShields = PlayerPrefs.GetInt("shield");
        if(savedShields == 1) {
            PlayerPrefs.SetInt("shield", 0);
        } else {
            PlayerPrefs.SetInt("shield", 1);
        }
        SetShieldsButtonText();
    }

    private void SetShieldsButtonText()
    {
        int savedShields = PlayerPrefs.GetInt("shield");
        if(savedShields == 1) {
            shieldsText.text = "SHIELDS TURNED ON (HARD)";
        } else {
            shieldsText.text = "SHIELDS TURNED OFF (EASY)";
        }
    }
}
