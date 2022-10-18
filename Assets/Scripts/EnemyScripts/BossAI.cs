using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : EnemyBehavior
{
    public Monster_Spawner parent_MonSpawn;
    private Animator animatorBoss;

    //Pathing Variables
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking Variables
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    private float lastAttackTime;
    public float Damage;

    Monster_Spawner spawn;

    // Start is called before the first frame update
    void Start()
    {
        parent_MonSpawn = GetComponentInParent<Monster_Spawner>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        baseObj = GameObject.FindGameObjectWithTag("Base").transform;
        agent = GetComponent<NavMeshAgent>();
        Damage = GameStats.BaseEnemyDamage[0] * GameStats.EnemyHealthModifier[(int)GameManager.Instance.CurrentDifficulty];
        //animatorBoss = GetComponentInChildren<animatorBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckWave();

        if(playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer))
        {
            playerSpotted = true;
        }
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        //Boss only spawns in Waves
        //if player not spotted yet / at all
        if(((!playerInSightRange || playerInSightRange) && !playerInAttackRange && (!playerSpotted || playerSpotted)))
        {
            Chase();
        }
        //if player in attack range
        if(playerInSightRange && playerInAttackRange && playerSpotted)
        {
            Attack(player);
        }
        
        //Need to have different health mechanics if loops

    }

    public override void Chase()
    {
        agent.SetDestination(player.transform.position);
    }

    public override void Attack(Transform target)
    {
        agent.SetDestination(transform.position);
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));

        if(!alreadyAttacked)
        {
            if (target.gameObject.tag == "Player")
            {
                target.gameObject.GetComponent<Health>().TakeDamage(Damage);

                Debug.Log("Hit Player");
            }

            if (target.gameObject.tag == "Base")
            {
                target.gameObject.GetComponent<Health>().TakeDamage(Damage);

                Debug.Log("Hit Base");
            }
            //~~~~~~~~~~~~~~~~~~~~Attack Code Here~~~~~~~~~~~~~~~~~~~~//
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public override void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public override void Die()
    {
        print("Boss Dying");
        parent_MonSpawn.bossDie();
        if(gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }
}
