using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
    public GameObject[] enemies;
    public HealthController healthController;
    Vector3 currentPosition;
    Vector3 enemyPosition;
    public EnemyController enemyController;
    float closestDist;
    float attackCooldown = 5;
    float timer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if(Input.GetKey(KeyCode.P) && timer <=0)
        {
            Debug.Log("attacking closest enemy");
            GameObject target = GetClosestEnemy(enemies);
            enemyController = target.GetComponent<EnemyController>();

            enemyController.TakeDamage();
            timer = attackCooldown;
        }
        else if (Input.GetKey(KeyCode.P) && timer > 0)
        {
            Debug.Log("player attack on cooldown");
        }
    }

    public GameObject GetClosestEnemy(GameObject[] enemies)
    {
        GameObject target = null;
        currentPosition = transform.position;
        closestDist = Mathf.Infinity;
        float dist;

        foreach (GameObject potentialTarget in enemies)
        {
            enemyPosition = potentialTarget.transform.position;
            dist = Vector3.Distance(enemyPosition, currentPosition);
            if (dist < closestDist)
            {
                target = potentialTarget;
                closestDist = dist;
            }
        }

        return target;
    }
}
