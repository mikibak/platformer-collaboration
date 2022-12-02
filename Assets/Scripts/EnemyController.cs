using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyController : MonoBehaviour
{
    
    public float speed;
    public int direction; //direction can be 0 - horizontal, 1 - vertical
    public float durationOfCycle;
    public int healthStart;
    public int health;
    public float timeBetweenAttacks;
    private int moveDir = 1;
    private int randomDirection;
    public float timer;
    public float timerAttacks = 0;

    public HealthController PlayerHealthController;
    public HealthBar healthBar;
    public HealthBar cooldownBar;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        timer = durationOfCycle;
        PlayerHealthController = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>();
        healthBar.SetMaxHealth(health);
        healthBar.SetHealth(health);
        cooldownBar.SetMaxHealth(timeBetweenAttacks);
        cooldownBar.SetHealth(timeBetweenAttacks);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        timerAttacks -= Time.deltaTime;
        cooldownBar.SetHealth(timeBetweenAttacks - Mathf.Clamp(timerAttacks, 0f, timeBetweenAttacks));
        if (timer <= 0) ChangeMovement();

        if (direction == 0)
        {
            transform.Translate(moveDir * speed * Time.fixedDeltaTime, 0.0f, 0.0f, Space.World);
        }
        else if (direction == 1)
        {
            transform.Translate(0.0f, moveDir * speed * Time.fixedDeltaTime, 0.0f, Space.World);
        }
    }

    public void ChangeMovement()
    {
        moveDir *= (-1);
        timer = durationOfCycle;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && timerAttacks <= 0)
        {
            Debug.Log("attacking player");
            PlayerHealthController.TakeDamage(1);
            timerAttacks = timeBetweenAttacks;
        }
        else if(timerAttacks > 0 && other.CompareTag("Player"))
        {
            Debug.Log("enemy attack cooldown");
        }
    }

    public void TakeDamage()
    {
        health--;
        healthBar.SetHealth(health);
        Debug.Log("Enemy health: " + health.ToString());
        if (health <= 0)
        {
            Invoke("DestroyEnemy", 0.5f);
            Death();
        }
    }

    private void Death()
    {
        Debug.Log("Enemy dead");
        animator.SetBool("isDead", true);
    }

    private void DestroyEnemy()
    {
        this.gameObject.SetActive(false);
    }
}
