using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    public GameObject platformPrefab;
    private static int PLATFORMS_NUM = 9;
    private GameObject[] platforms;
    private Vector3[] positions;
    private Vector3[] offset;
    private float maxY;
    private float minY;
    public float speed = 1;
    public float width = 1;

    void Awake()
    {
        platforms = new GameObject[ PLATFORMS_NUM ];
        positions = new Vector3[ PLATFORMS_NUM ];
        offset = new Vector3[ PLATFORMS_NUM ];
        for(int i=0; i<PLATFORMS_NUM; i++) {
            positions[i].x = transform.position.x + i*width;
            positions[i].y = transform.position.y + Mathf.Sin(i*Mathf.PI * 45.0f/180.0f);
            positions[i].z = 0;
            offset[i] = new Vector3(0, 1, 0);
            platforms[ i ] = Instantiate( platformPrefab, positions[ i ], Quaternion.identity );
        }
        maxY = transform.position.y + 1;
        minY = transform.position.y - 1;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<PLATFORMS_NUM; i++) {
            if(platforms[ i ].transform.position.y >= maxY || platforms[ i ].transform.position.y <= minY) {
                offset[i] = -offset[i];
            }
            //platforms[ i ].transform.position = Vector3.MoveTowards( platforms[ i ].transform.position, 
            //new Vector3(platforms[ i ].transform.position.x,  transform.position.y + Mathf.Sin(offset[i].y*Mathf.PI * 45.0f/180.0f), 0), speed * Time.deltaTime );
            float calculatedPosition = transform.position.y + Mathf.Sin( speed * (Time.time*Mathf.PI+i));
            platforms[ i ].transform.position = new Vector3(platforms[ i ].transform.position.x, calculatedPosition, transform.position.z);
        }
    }
}
