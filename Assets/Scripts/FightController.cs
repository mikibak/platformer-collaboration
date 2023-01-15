using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FightController : MonoBehaviour
{
    public GameObject[] enemies;
    public PlayerController playerController;
    Vector3 currentPosition;
    Vector3 enemyPosition;
    public EnemyController enemyController;
    public BossController bossController;
    float closestDist;
    float attackCooldown = 2;
    public float timer = 0;
    public ParticleSystem gunTrail;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;
    public GameObject gun;
    public Slider staminaSlider;
    public AudioClip shootSound;

    
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
        if(Input.GetKey(KeyCode.Q) && timer <=0)
        {
            GetComponent<PlayerController>().audioSource.PlayOneShot( shootSound, AudioListener.volume/2 );
            Debug.Log("attacking closest enemy");
            GameObject target = GetClosestEnemy();
            if (target.name == "Boss") bossController = target.GetComponent<BossController>();
            else enemyController = target.GetComponent<EnemyController>();
            Vector3 targetDirection = target.transform.position - transform.position;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 2*Mathf.PI, 0.0f);

            velocityModule.x = newDirection.x;
            velocityModule.y = newDirection.y;
            velocityModule.z = newDirection.z;

            //may be used to target attacks on enemy, not in the general direction
            //velocityModule.speedModifierMultiplier = targetDirection.magnitude;

            gunTrail.Play();
            if (target.name == "Boss") bossController.TakeDamage();
            else enemyController.TakeDamage();
            timer = attackCooldown;
        }
        else if (Input.GetKey(KeyCode.Q) && timer > 0)
        {
            Debug.Log("player attack on cooldown");
        }



        staminaSlider.value = attackCooldown - timer;
    }

    public GameObject GetClosestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
