using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int healthMax;
    public int health;
    public float timeBetweenAttacks;
    public float timerAttacks = 0;

    public PlayerController playerController;
    public HealthBar healthBar;
    public HealthBar cooldownBar;

    //animator
    public Animator animator;

    //shooting
    public GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 1;
    [SerializeField] private float maxRange = 5;

    //shield
    public bool usesShield = true;
    [SerializeField] private GameObject shield;
    private bool shieldActivated;
    private float shieldDectivatedTimer;
    private float shieldActivatedTimer;
    public float deactivatedWindow = 2.0f;
    public float activatedWindow = 2.0f;

    //movement
    private Vector3 startPos;

    public float speed = 1;
    public float xScale = 1;
    public float yScale = 1;


    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        healthBar.SetMaxHealth(health);
        healthBar.SetHealth(health);
        cooldownBar.SetMaxHealth(timeBetweenAttacks);
        cooldownBar.SetHealth(timeBetweenAttacks);
        startPos = transform.localPosition;
    }

    private void FixedUpdate()
    {
        timerAttacks -= Time.deltaTime;
        cooldownBar.SetHealth(timeBetweenAttacks - Mathf.Clamp(timerAttacks, 0f, timeBetweenAttacks));
        transform.localPosition = startPos + (Vector3.right * Mathf.Sin(Time.timeSinceLevelLoad / 2 * speed) * xScale - Vector3.up * Mathf.Sin(Time.timeSinceLevelLoad * speed) * yScale);

        if (Vector3.Distance(transform.position, playerController.gameObject.transform.position) < maxRange)
        {
            if (timerAttacks <= 0)
            {
                Debug.Log("shooting at player");
                ShootBullet();
                timeBetweenAttacks = Random.Range(3.0f, 6.0f);
                timerAttacks = timeBetweenAttacks;
            }
        }


        if (usesShield)
        {
            SetShield();
        }
    }

    public void TakeDamage()
    {
        if (usesShield && shieldActivated)
        {
            return;
        }

        health--;
        healthBar.SetHealth(health);
        Debug.Log("Enemy health: " + health.ToString());
        if (health <= 0)
        {
            Invoke("DestroyEnemy", 0.5f);
            playerController.AddPoints(2);
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

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation, null);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        Vector2 direction = playerController.gameObject.transform.position - transform.position;
        Vector2 newvector = direction.normalized * bulletSpeed;
        bulletRigidbody.velocity = newvector;

    }

    private void SetShield()
    {
        if (!shieldActivated)
        {
            if (shieldDectivatedTimer >= 0)
            {
                shieldDectivatedTimer -= Time.fixedDeltaTime;
            }
            else
            {
                //activate
                shieldActivated = true;
                shield.SetActive(true);
                float roll = Random.Range(0.0f, 1.0f);
                shieldActivatedTimer = activatedWindow + roll;
            }
        }
        else
        {
            //shield is active
            if (shieldActivatedTimer >= 0)
            {
                shieldActivatedTimer -= Time.fixedDeltaTime;
            }
            else
            {
                //deactivate
                shieldActivated = false;
                shield.SetActive(false);
                shieldDectivatedTimer = deactivatedWindow;
            }
        }
    }
}
