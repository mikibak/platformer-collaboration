using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsController : MonoBehaviour
{
    public bool isUnlocked;
    public Animator doorsAnimator;
    public GameObject levelCompletedText;
    public GameObject DoorsUnlockedText;
    public GameObject DoorsLockedText;
    public float timeOfMessage =2;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isUnlocked)
        {
            doorsAnimator.SetBool("Entering", true);
            other.gameObject.SetActive(false);
            levelCompletedText.SetActive(true);
        }
        else if(!isUnlocked)
        {
            DoorsLockedText.SetActive(true);
            Invoke("HideTextLocked", timeOfMessage);
        }
    }

    public void SetUnlocked ()
    {
        isUnlocked = true;
        doorsAnimator.SetBool("AllKeys", true);
        DoorsUnlockedText.SetActive(true);
        Invoke("HideTextUnlocked", timeOfMessage);
    }

    public void HideTextUnlocked()
    {
        DoorsUnlockedText.SetActive(false);
    }

    public void HideTextLocked()
    {
        DoorsLockedText.SetActive(false);
    }
}
