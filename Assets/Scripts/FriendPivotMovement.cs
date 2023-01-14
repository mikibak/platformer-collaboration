using UnityEngine;
using System.Collections;

public class FriendPivotMovement : MonoBehaviour
{
    private Vector3 startPos;

    public float speed = 1;
    public float xScale = 1;
    public float yScale = 1;

    void Start () {
        startPos = transform.localPosition;
    }

    void Update () {
        transform.localPosition = startPos + (Vector3.right * Mathf.Sin(Time.timeSinceLevelLoad/2*speed)*xScale - Vector3.up * Mathf.Sin(Time.timeSinceLevelLoad * speed)*yScale);
    } 
}