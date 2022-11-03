using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EliteMAI : EnemyBehavior
{
    private Monster_Spawner parent_MonSpawn;
    private Animator animatorMAI;
    public GameItem creepDrop;
    public GameItem essence;

    //Pathing Variables
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public float wanderSpeed = 5f;
    public float chaseSpeed = 5f;

    //Attacking Variables
    public float Damage;
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public void Start()
    {
        parent_MonSpawn = GetComponentInParent<Monster_Spawner>();  //set parent's Monster_Spawner script
        player = GameObject.FindGameObjectWithTag("Player").transform;  //set player object
        baseObj = GameObject.FindGameObjectWithTag("Base").transform; //set base object
        agent = GetComponent<NavMeshAgent>();   //set NavMesh agent
        Damage = GameStats.BaseEnemyDamage[1] * GameStats.EnemyAttackModifier[(int)GameManager.Instance.CurrentDifficulty];
        animatorMAI = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
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
        if ((!playerInSightRange && !playerInAttackRange && !playerSpotted && !baseInSightRange && !baseInAttackRange && isWave) ||
            (!playerInSightRange && !playerInAttackRange && !playerSpotted && baseInSightRange && !baseInAttackRange && isWave))
        {
            animatorMAI.SetBool("isWave", true);
            print("Finding Base(isWave)");
            agent.speed = chaseSpeed;
            findBase();
        }
        //If during wave, player not spotted and base within sight and attack range. Attack Base.
        if(!playerInSightRange && !playerInAttackRange && !playerSpotted && baseInSightRange && baseInAttackRange && isWave)
        {
            print("Attacking Base(isWave)");
            animatorMAI.SetBool("AttackBase", true);
            Attack(baseObj);
        }
        //If during wave, player is spotted while elite mob is hitting/spotted/has not spotted base. Chase player.
        if((playerInSightRange && !playerInAttackRange && !playerSpotted && (baseInSightRange || !baseInSightRange) && (baseInAttackRange || !baseInAttackRange) && isWave) ||
            (playerInSightRange && !playerInAttackRange && playerSpotted && (baseInSightRange || !baseInSightRange) && (baseInAttackRange || !baseInAttackRange) && isWave))
        {
            animatorMAI.SetBool("playerInAttackRange", true);
            animatorMAI.SetBool("isWave", false); //Used to trigger attack anim for Elite Melee during Waves
            print("Chasing Player(isWave)");
            Chase();
        }
        //Keeping Aggro
        if ((!playerInSightRange) && (!playerInAttackRange) && playerSpotted && (baseInSightRange || !baseInSightRange) && (baseInAttackRange || !baseInAttackRange) && isWave)
        {
            animatorMAI.SetBool("playerInAttackRange", true);
            //print("Chasing Player(isWave)");
            Chase();
        }
        //If during wave, player is within attack range. Attack player.
        if (playerInSightRange && playerInAttackRange && playerSpotted && (baseInSightRange || !baseInSightRange) && (baseInAttackRange || !baseInAttackRange) && isWave)
        {
            animatorMAI.SetBool("AttackPlayer", true);
            animatorMAI.SetBool("isWave", false); //Used to trigger attack anim for Elite Melee during Waves
            print("Attacking Player(isWave)");
            Attack(player);
        }

        //When !isWave
        //If not in wave and player not in sight while base in sight/attack range, monster wanders.
        if ((!playerInSightRange && !playerInAttackRange && !playerSpotted && (!baseInSightRange || baseInSightRange) && (!baseInAttackRange || baseInAttackRange) && !isWave))
        {
            print("Wandering(!isWave)");
            animatorMAI.SetBool("Wandering", true);
            agent.speed = wanderSpeed;
            Wandering();
        }
        //If player is in range of enemy's sight, monster will chase.
        if ((playerInSightRange && !playerInAttackRange && (playerSpotted || !playerSpotted) && (!baseInSightRange || baseInSightRange) && (!baseInAttackRange || baseInAttackRange) && !isWave))
        {
            animatorMAI.SetBool("playerInAttackRange", true);
            print("Chasing Player(!isWave)");
            agent.speed = chaseSpeed;
            Chase();
        }
        //If player is in attack range and in sight range, monster will attack.
        if (playerInSightRange && playerInAttackRange && playerSpotted && (!baseInSightRange || baseInSightRange) && (!baseInAttackRange || baseInAttackRange) && !isWave)
        {
            animatorMAI.SetBool("AttackPlayer", true);
            print("Attacking Player(!isWave)");
            Attack(player);
        }
    }

    //Wandering state
    public override void Wandering()
    {
        animatorMAI.SetBool("playerInAttackRange", false);
        animatorMAI.SetBool("AttackPlayer", false);
        animatorMAI.SetBool("AttackBase", false);
        isWandering = true;
        if (!walkPointSet)
        {
            findWalkPoint();
        }

        if (walkPointSet)
        {
            //agent.SetDestination(walkPoint);
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, walkPoint, -1, path);
            agent.path = path;
        }

        distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    public override void findBase()
    {
        animatorMAI.SetBool("AttackPlayer", false);
        animatorMAI.SetBool("AttackBase", false);
        //agent.SetDestination(baseObj.transform.position);
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, baseObj.transform.position, -1, path);
        agent.path = path;
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
        animatorMAI.SetBool("AttackPlayer", false);
        animatorMAI.SetBool("AttackBase", false);
        //agent.SetDestination(player.transform.position);
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, player.transform.position, -1, path);
        agent.path = path;
    }

    //When player is within monster attack range, monster stops moving and hits the player
    public override void Attack(Transform target)
    {
        //agent.SetDestination(transform.position);
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, transform.position, -1, path);
        agent.path = path;
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));

        if (!alreadyAttacked)
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
    //Code for points given on enemy kill goes here inside the eliteMDie() in MonsterSpawner.cs
    public override void Die()
    {
        print("Elite Melee Dying");
        parent_MonSpawn.eliteMDie();
        if (creepDrop != null)
        {
            GameManager.Instance.PlayerInventory.AddItem(creepDrop, 1 + dropBonus);
        }
        if (Random.Range(0, 101) <= 20 && essence != null)
        {
            GameManager.Instance.PlayerInventory.AddItem(essence, 1 + dropBonus);
        }
        if (gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }
}
