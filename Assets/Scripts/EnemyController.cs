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

    public PlayerController playerController;
    public HealthBar healthBar;
    public HealthBar cooldownBar;

    public Animator animator;

    //shooting
    public GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 1;
    [SerializeField] private float maxRange = 5;

    //shield
    public bool usesShield = false;
    [SerializeField] private GameObject shield;
    private bool shieldActivated;
    private float shieldDectivatedTimer;
    private float shieldActivatedTimer;
    public float deactivatedWindow = 2.0f;
    public float activatedWindow = 2.0f;

    //sound
    public AudioClip shootSound;
    public AudioClip deathSound;
    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        int shields = PlayerPrefs.GetInt("shield");
        if(shields == 1) {
            usesShield = true;
        }
        timer = durationOfCycle;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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

        if(Vector3.Distance(transform.position, playerController.gameObject.transform.position) < maxRange) {
            if (timerAttacks <= 0)
            {
            Debug.Log("shooting at player");
            ShootBullet();
            timerAttacks = timeBetweenAttacks;
            }
        }

        if(usesShield) {
            SetShield();
        }
    }

    public void ChangeMovement()
    {
        moveDir *= (-1);
        timer = durationOfCycle;
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player") && timerAttacks <= 0)
    //    {
    //        Debug.Log("attacking player");
    //        playerController.TakeDamage(1);
    //        timerAttacks = timeBetweenAttacks;
    //    }
    //    else if(timerAttacks > 0 && other.CompareTag("Player"))
    //    {
    //        Debug.Log("enemy attack cooldown");
    //    }
    //}

    public void TakeDamage()
    {
        if(usesShield && shieldActivated) {
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
        playerController.audioSource.PlayOneShot( deathSound, AudioListener.volume );
        this.gameObject.SetActive(false);
    }

    private void ShootBullet()
    {
        audioSource.PlayOneShot( shootSound, AudioListener.volume/2 );
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation, null); 
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        Vector2 direction = playerController.gameObject.transform.position - transform.position;
        Vector2 newvector = direction.normalized * bulletSpeed;
        bulletRigidbody.velocity = newvector;

    }

    private void SetShield()
    {
        if(!shieldActivated) {
            if(shieldDectivatedTimer >=0) 
            {
                shieldDectivatedTimer -= Time.fixedDeltaTime;
            }else
            {
                //activate
                shieldActivated = true;
                shield.SetActive(true);
                float roll = Random.Range(0.0f,1.0f);
                shieldActivatedTimer = activatedWindow + roll;
            }
        }
        else
        {
            //shield is active
            if(shieldActivatedTimer >=0) 
            {
                shieldActivatedTimer -= Time.fixedDeltaTime;
            }else
            {
                //deactivate
                shieldActivated = false;
                shield.SetActive(false);
                shieldDectivatedTimer = deactivatedWindow;
            }
        }
    }
}
