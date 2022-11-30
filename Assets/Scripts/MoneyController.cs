using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    private int score = 0;
    public ScoreManager scoreManager;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Bonus")) {
            score += 1;
            Debug.Log( "Score: " + score );
            scoreManager.AddPoints(1);
            other.gameObject.SetActive( false );
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
