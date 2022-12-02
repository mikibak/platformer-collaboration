using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsController : MonoBehaviour
{
    public bool isUnlocked;
    public Animator doorsAnimator;
    public GameObject levelCompletedText;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isUnlocked)
        {
            doorsAnimator.SetBool("Entering", true);
            Debug.Log("Level complete");
            other.gameObject.SetActive(false);
            levelCompletedText.SetActive(true);
        }
    }

    public void SetUnlocked ()
    {
        isUnlocked = true;
        doorsAnimator.SetBool("AllKeys", true);
    }
}
