using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreepAI : EnemyBehavior
{
    private Monster_Spawner parent_MonSpawn;
    private Animator animatorCreep;
    public GameItem creepDrop;

    //Pathing Variables
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public float wanderSpeed = 3f;
    public float chaseSpeed = 7f;
    

    //Attacking Variables
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public float Damage;

    Monster_Spawner spawn;

    public void Start()
    {
        parent_MonSpawn = GetComponentInParent<Monster_Spawner>();  //set parent's Monster_Spawner script
        player = GameObject.FindGameObjectWithTag("Player").transform;  //set player object
        baseObj = GameObject.FindGameObjectWithTag("Base").transform; //set base object
        agent = GetComponent<NavMeshAgent>();   //set NavMesh agent
        Damage = GameStats.BaseEnemyDamage[0] * GameStats.EnemyAttackModifier[(int)GameManager.Instance.CurrentDifficulty];
        animatorCreep = GetComponentInChildren<Animator>();
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        //InvokeRepeating("UpdateAI", 1f, 2f);
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

        UpdateAI();
    }

    //Wandering state
    public override void Wandering()
    {
        animatorCreep.SetBool("playerInAttackRange", false);
        animatorCreep.SetBool("AttackPlayer", false);
        animatorCreep.SetBool("AttackBase", false);
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
        animatorCreep.SetBool("AttackPlayer", false);
        animatorCreep.SetBool("AttackBase", false);

        /*NavMeshHit hit = new NavMeshHit();
        NavMesh.FindClosestEdge(baseObj.position, out hit, -1);*/

        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position,baseObj.position, -1 , path);
        agent.path = path;

        
        
        for (int i = 0; i < agent.path.corners.Length - 1; i++)
            Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red);
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
            if(NavMesh.SamplePosition(walkPoint, out closestHit, 500, 1))
            {
                walkPoint = closestHit.position;
            }
        }

        if(Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
        {
            walkPointSet = true;
        }
    }

    //Player is spotted and monster gives chase
    public override void Chase()
    {
        animatorCreep.SetBool("AttackPlayer", false);
        animatorCreep.SetBool("AttackBase", false);
        animatorCreep.SetBool("BaseSpotted", false);
        agent.SetDestination(player.transform.position);
    }

    //When player is within monster attack range, monster stops moving and hits the player
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
        //~~~~~~~~~~~~~~~~~~~~Set Attack Cooldown Period Here~~~~~~~~~~~~~~~~~~~~//
        alreadyAttacked = false;
    }

    //When monster dies.
    //Code for points given on enemy kill goes here inside the MonsterSpawner.cs
    public override void Die()
    {
        animatorCreep.SetBool("Die", true); //Need to find a way to implement death anim
        print("Creep Dying");
        parent_MonSpawn.creepDie();
        if (creepDrop != null)
        {
            GameManager.Instance.PlayerInventory.AddItem(creepDrop);//To change quantity
        }

        if (Random.Range(0, 101) <= 10)
        {
            Instantiate(parent_MonSpawn.healthPickupPrefab, transform.position, Quaternion.identity);
        }

        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    void UpdateAI()
    {
        /**********Conditions for state of monster.**********/
        //When isWave (Creeps solely target the player base)
        //If during wave, player and base both not in sight or attack range. Find base.
        if (((!playerInSightRange || playerInSightRange) && (!playerInAttackRange || playerInAttackRange) && (playerSpotted || !playerSpotted) && !baseInSightRange && !baseInAttackRange && isWave) ||
            ((!playerInSightRange || playerInSightRange) && (!playerInAttackRange || playerInAttackRange) && (playerSpotted || !playerSpotted) && baseInSightRange && !baseInAttackRange && isWave))
        {
            //print("Finding Base(isWave)");
            animatorCreep.SetBool("isWave", true);
            findBase();
        }
        //If during wave, player not spotted and base within sight and attack range. Attack Base.
        if ((!playerInSightRange || playerInSightRange) && (!playerInAttackRange || playerInAttackRange) && (playerSpotted || !playerSpotted) && baseInSightRange && baseInAttackRange && isWave)
        {
            //print("Attacking Base(isWave)");
            animatorCreep.SetBool("AttackBase", true);
            Attack(baseObj);
        }

        //When !isWave (Creeps will target the player)
        //If not in wave and player not in sight while base in sight/attack range, monster wanders.
        if ((!playerInSightRange && !playerInAttackRange && (!playerSpotted || playerSpotted) && (!baseInSightRange || baseInSightRange) && (!baseInAttackRange || baseInAttackRange) && !isWave))
        {
            //print("Wandering(!isWave)");
            animatorCreep.SetBool("Wandering", true);
            agent.speed = wanderSpeed;
            Wandering();
        }
        //If player is in range of enemy's sight, monster will chase.
        if ((playerInSightRange && !playerInAttackRange && (playerSpotted || !playerSpotted) && (!baseInSightRange || baseInSightRange) && (!baseInAttackRange || baseInAttackRange) && !isWave))
        {
            //print("Chasing Player(!isWave)");
            animatorCreep.SetBool("playerInAttackRange", true);
            agent.speed = chaseSpeed;
            Chase();
        }
        //If player is in attack range and in sight range, monster will attack.
        if (playerInSightRange && playerInAttackRange && playerSpotted && (!baseInSightRange || baseInSightRange) && (!baseInAttackRange || baseInAttackRange) && !isWave)
        {
            //print("Attacking Player(!isWave)");
            animatorCreep.SetBool("AttackPlayer", true);
            Attack(player);
        }
    }
}
