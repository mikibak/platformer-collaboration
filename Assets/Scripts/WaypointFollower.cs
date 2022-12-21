using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 10f;
    [SerializeField] private int currentWaypoint = 0;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, waypoints[currentWaypoint].transform.position);
        if(distance < 0.1f) 
        {
            currentWaypoint++;
            currentWaypoint = currentWaypoint % waypoints.Length;
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, speed*Time.fixedDeltaTime);
    }
}
