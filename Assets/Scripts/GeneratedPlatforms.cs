using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    public GameObject platformPrefab;
    private static int PLATFORMS_NUM = 6;
    private GameObject[] platforms;
    private Vector3[] positions;
    private Vector3 offset;
    private float maxX;
    private float maxY;
    private float minY;
    public float speed = 1;

    void Awake()
    {
        offset.y = 3;
        platforms = new GameObject[ PLATFORMS_NUM ];
        positions = new Vector3[ PLATFORMS_NUM ];
        for(int i=0; i<PLATFORMS_NUM; i++) {
            positions[i].x = transform.position.x + i;
            positions[i].y = transform.position.y + Mathf.Sin(i);
            positions[i].z = 0;
            platforms[ i ] = Instantiate( platformPrefab, positions[ i ], Quaternion.identity );
        }
        maxX = transform.position.x + PLATFORMS_NUM;
        maxY = transform.position.y + 1;
        minY = transform.position.y - 1;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<PLATFORMS_NUM; i++) {
            if(platforms[ i ].transform.position.y >= maxY || platforms[ i ].transform.position.y <= minY) {
                offset = -offset;
            }
            Vector3.MoveTowards( platforms[ i ].transform.position, platforms[ i ].transform.position + offset, speed * Time.deltaTime );
        }
    }
}
