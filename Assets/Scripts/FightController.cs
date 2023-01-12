using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightController : MonoBehaviour
{
    public GameObject[] enemies;
    public PlayerController playerController;
    Vector3 currentPosition;
    Vector3 enemyPosition;
    public EnemyController enemyController;
    float closestDist;
    float attackCooldown = 2;
    public float timer = 0;
    public ParticleSystem gunTrail;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;
    public GameObject gun;
    public Slider staminaSlider;

    
    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        velocityModule = gunTrail.velocityOverLifetime;
        staminaSlider.maxValue = attackCooldown;
        staminaSlider.value = attackCooldown;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if((Input.GetKey(KeyCode.P) || Input.GetMouseButtonDown(0)) && timer <=0)
        {
            Debug.Log("attacking closest enemy");
            GameObject target = GetClosestEnemy();
            enemyController = target.GetComponent<EnemyController>();
            Vector3 targetDirection = target.transform.position - transform.position;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 2*Mathf.PI, 0.0f);

            velocityModule.x = newDirection.x;
            velocityModule.y = newDirection.y;
            velocityModule.z = newDirection.z;

            //may be used to target attacks on enemy, not in the general direction
            //velocityModule.speedModifierMultiplier = targetDirection.magnitude;

            gunTrail.Play();

            enemyController.TakeDamage();
            timer = attackCooldown;
        }
        else if (Input.GetKey(KeyCode.P) && timer > 0)
        {
            Debug.Log("player attack on cooldown");
        }

        staminaSlider.value = attackCooldown - timer;
    }

    public GameObject GetClosestEnemy()
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
