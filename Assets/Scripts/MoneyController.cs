using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    private int score = 0;
    private int keysFound = 0;
    private const int maxKeys = 3;
    public ScoreManager scoreManager;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Bonus")) {
            score += 1;
            Debug.Log( "Score: " + score );
            scoreManager.AddPoints(1);
            other.gameObject.SetActive( false );
        }

        if (other.CompareTag("Key"))
        {
            keysFound += 1;
            scoreManager.AddKey();
            other.gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        scoreManager.SetMaxKeys(maxKeys);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
