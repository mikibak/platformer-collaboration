using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    public float speed;
    public int direction; //direction can be 0 - horizontal, 1 - vertical
    public float durationOfCycle;
    private int moveDir = 1;
    private int randomDirection;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = durationOfCycle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
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
}
