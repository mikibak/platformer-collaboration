using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //movement
    [SerializeField] private float moveSpeed = 0.1f;
    public float jumpForce = 6.0f;
    private Rigidbody2D rigidBody;
    private Animator animator;
    public LayerMask groundLayer;
    public static float rayLength = 0.25f;
    private float moveDir = 0;
    [SerializeField] private float jumpCooldown = 0;
    [SerializeField] private float maxJumpCooldown = 1;

    //animation
    private bool isWalking = false;
    private bool isFacingRight = true;

    //score and death
    private int score = 0;
    private const int maxKeys = 5;
    public ScoreManager scoreManager;

    //health
    public int maxHealth;
    public int health;
    public HealthBar healthBar;

    //sound
    public AudioSource audioSource;
    public AudioClip coinSound;

    //trees
    private bool canPlant;
    private GameObject contactedSoil;
    public GameObject PlantButton;
    public GameObject TreePrefab;
    private int trash = 0;
    private int seeds = 0;
    public GameObject plantText;
    public GameObject attackText;

    //Friend
    public GameObject Friend;

    //respawning
    [SerializeField] private Transform lastCheckpoint;
    [SerializeField] private bool respawning;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        scoreManager.SetMaxKeys(maxKeys);

        health = maxHealth;
        healthBar.SetMaxHealth(health);

        canPlant = false;
    }


    void FixedUpdate()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME && !respawning)
        {
            isWalking = false;
            //1 gdy w prawo, -1 gdy w lewo, 0 gdy brak inputu
            moveDir = Input.GetAxis("Horizontal");

            transform.Translate(moveDir * moveSpeed * Time.fixedDeltaTime, 0.0f, 0.0f, Space.World);

            //Animations
            if (moveDir != 0)
            {
                isWalking = true;
            }
            if ((moveDir < 0 && isFacingRight) || (moveDir > 0 && !isFacingRight)) Flip();

            //jumping
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
            if (jumpCooldown > 0)
            {
                jumpCooldown -= Time.fixedDeltaTime;
            }

            //respawn or death when player falls out of map 
            if (transform.position.y < -5 && !respawning)
            {
                if (health >= 1)
                {
                    Respawn();
                } else 
                {
                    Death();
                }
            }

            //settings variables for animator
            animator.SetBool("isGrounded", isGrounded());
            animator.SetBool("isWalking", isWalking);

            if (Input.GetKey(KeyCode.E))
            {
                PlantTree();
            }
        }
    }

    void Update() {
        if(respawning) {
            this.transform.position = Friend.transform.position;
        }
    }


    //function checking is player is grounded
    private bool isGrounded()
    {
        //do zmiany moze
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    private void Jump()
    {
        if (/*isGrounded() &&*/ jumpCooldown <= 0 && rigidBody.velocity.y == 0)
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCooldown = maxJumpCooldown;
        }
    }

    //Function fliping player sprite
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 thescale = transform.localScale;
        thescale.x *= (-1);
        transform.localScale = thescale;
    }


    public void Death()
    {
        this.gameObject.SetActive(false);

        GameManager.instance.GameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bonus"))
        {
            score += 1;
            Debug.Log("Score: " + score);
            scoreManager.AddPoints(1);
            other.gameObject.SetActive(false);
            audioSource.PlayOneShot( coinSound, AudioListener.volume );
        }

        if (other.CompareTag("Trash"))
        {
            trash += 1;
            Debug.Log("Trash: " + trash);
            scoreManager.AddTrash(1);
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Key"))
        {
            scoreManager.AddKey();
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent( other.transform );
        }
        if (other.CompareTag("Soil"))
        {
            canPlant = true;
            //PlantButton.SetActive(true);
            plantText.SetActive(true);
        }
        if (other.CompareTag("attack"))
        {
            attackText.SetActive(true);
        }
        if (other.CompareTag("Checkpoint"))
        {
            lastCheckpoint = other.transform;
        }
        if (other.CompareTag("Checkpoint") && respawning)
        {
            //stop respawning
            Debug.Log("Respawn successful!");
            respawning = false;
            Friend.GetComponent<Pathfinding.Examples.AstarSmoothFollow2>().target = this.transform;
        }
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit2D( Collider2D other ) {
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent( null );
        }
        if (other.CompareTag("Soil"))
        {
            //other.gameObject.tag="OccupiedSoil";
            //canPlant = false;
            PlantButton.SetActive(false);
        }
        if (other.CompareTag("Soil"))
        {
            canPlant = false ;
            PlantButton.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);

        if (health == 0)
        {
            Death();
        }
    }

    public void AddHealth(int healthAdded)
    {
        if (health != maxHealth) health += healthAdded;
        healthBar.SetHealth(health);
    }

    public void PlantTree()
    {
        if(canPlant && scoreManager.seeds>0) {
            Vector3 tree_position = this.transform.position;
            tree_position.x -= 0.3f;
            tree_position.y -= 0.2f;
            Instantiate(TreePrefab, tree_position, this.transform.rotation);
            scoreManager.SubstractSeeds(1);
        }

    }

    private void Respawn()
    {
        TakeDamage(1);
        Friend.GetComponent<Pathfinding.Examples.AstarSmoothFollow2>().target = lastCheckpoint.transform;;
        respawning = true;
    }
}
