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
    private const int maxKeys = 3;
    public ScoreManager scoreManager;

    //health
    public int maxHealth;
    public int health;
    public HealthBar healthBar;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        scoreManager.SetMaxKeys(maxKeys);

        health = maxHealth;
        healthBar.SetMaxHealth(health);
    }


    void FixedUpdate()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
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
            if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
            if (jumpCooldown > 0)
            {
                jumpCooldown -= Time.fixedDeltaTime;
            }

            //death when player falls out of map 
            if (transform.position.y < -15)
            {
                Death();
            }

            //settings variables for animator
            animator.SetBool("isGrounded", isGrounded());
            animator.SetBool("isWalking", isWalking);
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
        if (isGrounded() && jumpCooldown <= 0 && rigidBody.velocity.y == 0)
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
    }

    private void OnTriggerExit2D( Collider2D other ) {
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent( null );
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
}
