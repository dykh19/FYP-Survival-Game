using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EliteRAI : EnemyBehavior
{
    public Monster_Spawner parent_MonSpawn;
    private Animator animatorRAI;
    public GameItem RAIDrop;

    //Pathing Variables
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public float wanderSpeed = 3f;
    public float chaseSpeed = 7f;

    //Attacking Variables
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject bullet;
    private float lastAttackTime = -1f;
    public float Damage;

    Monster_Spawner spawn;

    public void Start()
    {
        parent_MonSpawn = GetComponentInParent<Monster_Spawner>();  //set parent's Monster_Spawner script
        player = GameObject.FindGameObjectWithTag("Player").transform;  //set player object
        baseObj = GameObject.FindGameObjectWithTag("Base").transform; //set base object
        agent = GetComponent<NavMeshAgent>();   //set NavMesh agent
        Damage = GameStats.BaseEnemyDamage[2] * GameStats.EnemyHealthModifier[(int)GameManager.Instance.CurrentDifficulty];
    }

    // Update is called once per frame
    private void Update()
    {
        //Check that monster's health is not = 0
        CheckHealth();
        CheckWave();

        //If player object within InSightRange sphere, player is spotted & playerInSightRange = true. If within InAttackRange sphere, playerInAttackRange = true
        if (playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer))
        {
            playerSpotted = true;
        }
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);
        baseInSightRange = Physics.CheckSphere(transform.position, sightRange, isBase);
        baseInAttackRange = Physics.CheckSphere(transform.position, attackRange, isBase);

        /**********Conditions for state of monster.**********/
        //When isWave
        //If during wave, player and base both not in sight or attack range. Find base.
        if (((!playerInSightRange) && (!playerInAttackRange) && (playerSpotted || !playerSpotted) && !baseInSightRange && !baseInAttackRange && isWave) ||
            ((!playerInSightRange) && (!playerInAttackRange) && (playerSpotted || !playerSpotted) && baseInSightRange && !baseInAttackRange && isWave))
        {
            //print("Finding Base(isWave)");
            findBase();
        }
        //If during wave, player not spotted and base within sight and attack range. Attack Base.
        if ((!playerInSightRange) && (!playerInAttackRange) && (playerSpotted || !playerSpotted) && baseInSightRange && baseInAttackRange && isWave)
        {
            //print("Attacking Base(isWave)");
            Attack(baseObj);
        }
        //If during wave, player is spotted while elite mob is hitting/spotted/has not spotted base. Chase player.
        if ((playerInSightRange) && (!playerInAttackRange) && (playerSpotted || !playerSpotted) && (baseInSightRange || !baseInSightRange) && (baseInAttackRange || !baseInAttackRange) && isWave)
        {
            //print("Chasing Player(isWave)");
            Chase();
        }
        //If during wave, player is within attack range. Attack player.
        if (playerInSightRange && playerInAttackRange && (playerSpotted || !playerSpotted) && (baseInSightRange || !baseInSightRange) && (baseInAttackRange || !baseInAttackRange) && isWave)
        {
            //print("Attacking Player(isWave)");
            Attack(player);
        }

        //When !isWave
        //If not in wave and player not in sight while base in sight/attack range, monster wanders.
        if ((!playerInSightRange && !playerInAttackRange && (playerSpotted || !playerSpotted) && (!baseInSightRange || baseInSightRange) && (!baseInAttackRange || baseInAttackRange) && !isWave))
        {
            print("Wandering(!isWave)");
            Wandering();
        }
        //If player is in range of enemy's sight, monster will chase.
        if ((playerInSightRange && !playerInAttackRange && (playerSpotted || !playerSpotted) && (!baseInSightRange || baseInSightRange) && (!baseInAttackRange || baseInAttackRange) && !isWave))
        {
            print("Chasing Player(!isWave)");
            Chase();
        }
        //If player is in attack range and in sight range, monster will attack.
        if (playerInSightRange && playerInAttackRange && (playerSpotted || !playerSpotted) && (!baseInSightRange || baseInSightRange) && (!baseInAttackRange || baseInAttackRange) && !isWave)
        {
            print("Attacking Player(!isWave)");
            Attack(player); 
        }
    }

    //Wandering state
    public override void Wandering()
    {
        isWandering = true;
        if (!walkPointSet)
        {
            findWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    public override void findBase()
    {
        agent.SetDestination(baseObj.transform.position);
    }

    //Finding the walk point for the monster's wander phase
    public override void findWalkPoint()
    {
        var rayOrigin = new Vector3(Random.Range(-walkPointRange, walkPointRange), 100f, Random.Range(-walkPointRange, walkPointRange));
        var ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            walkPoint = hit.point + hit.normal;
            NavMeshHit closestHit;
            if (NavMesh.SamplePosition(walkPoint, out closestHit, 500, 1))
            {
                walkPoint = closestHit.position;
            }
        }

        if (Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
        {
            walkPointSet = true;
        }
    }

    //Player is spotted and monster gives chase
    public override void Chase()
    {
        agent.SetDestination(player.transform.position);
    }

    //When player is within monster attack range, monster stops moving and hits the player
    /*public override void Attack(Transform target)
    {
        agent.SetDestination(transform.position);
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));

        if(!alreadyAttacked)
        {
            //~~~~~~~~~~~~~~~~~~~~Attack Code Here~~~~~~~~~~~~~~~~~~~~//
            Rigidbody rb = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }*/

    public override void Attack(Transform target)
    {
        agent.SetDestination(transform.position);
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));

        if (Time.time > lastAttackTime + timeBetweenAttacks)
        {
            //~~~~~~~~~~~~~~~~~~~~Attack Code Here~~~~~~~~~~~~~~~~~~~~//
            Rigidbody rb = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            bullet.GetComponent<ProjectileRangedEnemy>().SetDamage(Damage);
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 4f, ForceMode.Impulse);
            Destroy(rb.gameObject, 10);
            Debug.Log("Attacking " + target.name);

            alreadyAttacked = true;
            lastAttackTime = Time.time;
        }
    }

    public override void ResetAttack()
    {
        //~~~~~~~~~~~~~~~~~~~~Set Attack Cooldown Period Here~~~~~~~~~~~~~~~~~~~~//
        alreadyAttacked = false;
    }

    //When monster dies.
    //Code for points given on enemy kill goes here inside the EliteRDie() in MonsterSpawner.cs
    public override void Die()
    {
        print("Elite Range Dying");
        parent_MonSpawn.eliteRDie();
        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }
}
