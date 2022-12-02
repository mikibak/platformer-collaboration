using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    public int maxHealth = 10;
    public int health;
    public HealthBar healthBar;
    public GameObject deathText;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        healthBar.SetHealth(health);

        if(health==0)
        {
            Death();
        }
    }

    public void AddHealth(int healthAdded)
    {

        if(health != maxHealth) health += healthAdded;

        healthBar.SetHealth(health);
    }

    public void Death()
    {
        this.gameObject.SetActive(false);
        deathText.SetActive(true);
        Debug.Log("zgon");
        Invoke("Restart", 5);
    }

    private void Restart()
    {
        SceneManager.LoadScene("188555_188968_188593");
        deathText.SetActive(false);
    }
}
