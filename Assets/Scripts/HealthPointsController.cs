using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HealthPointsController : MonoBehaviour
{
    public HealthController healthController;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HealthPoint"))
        {
            Debug.Log("Adding health");
            healthController.AddHealth(1);
            other.gameObject.SetActive(false);
        }
    }
}
