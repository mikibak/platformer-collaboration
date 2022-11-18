using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FoxController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.1f;
    public float jumpForce = 6.0f;
    private Rigidbody2D rigidBody;
    public LayerMask groundLayer;
    public static float rayLength = 0.25f;
    // Start is called before the first frame update

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    //GetKey(KeyCode.RightArrow
    void Update()
    {
        if(Input.GetAxis("Horizontal")>0)
        {
            transform.Translate(moveSpeed*Time.deltaTime, 0.0f, 0.0f, Space.World);
        }
        if (Input.GetAxis("Horizontal")<0)
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        }
        if (Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        //Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1, false);


        //Restart poziomu po wypadnieciu z mapy
        //w przyszlosci pewnie do zmiany - ustawienie jakiegos ekranu game over
        if(transform.position.y < -5)
        {
            SceneManager.LoadScene("188555_188968_188593");
        }
    }

    private bool isGrounded()
    {
        //do zmiany ??? co to za sposob wgl???
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    private void Jump()
    {
        if (isGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("jumping");
        }
    }
}
