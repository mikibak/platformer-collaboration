using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public Text keysText;
    int score = 0;
    int keys = 0;
    int maxKeys;
    public DoorsController doorsController;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "POINTS: " + score.ToString();
        keysText.text = "KEYS: " + keys.ToString() + "/" + maxKeys.ToString();
    }

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = "POINTS: " + score.ToString();
    }

    public void AddKey()
    {
        keys++;
        keysText.text = "KEYS: " + keys.ToString() + "/" + maxKeys.ToString();
        if(keys==maxKeys)
        {
            doorsController.SetUnlocked();
        }
    }

    public void SetMaxKeys(int max)
    {
        maxKeys = max;
        keysText.text = "KEYS: " + keys.ToString() + "/" + maxKeys.ToString();

    }
}
