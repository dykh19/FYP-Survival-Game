using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : EnemyBehavior
{
    public Monster_Spawner parent_MonSpawn;
    private Animator animatorBoss;
    public Health Health;
    public GameItem creepDrop;
    public GameItem essence;
    public GameItem bossCore;
    //Add Boss Core//

    //Pathing Variables
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public float wanderSpeed = 5f;
    public float chaseSpeed = 7f;

    //Attacking Variables
    public float timeBetweenAttacks;
    public double newTimeBetweenAttacks;
    bool alreadyAttacked;
    private float lastAttackTime;
    public float Damage;
    private float phase1Dmg;
    private float phase2Dmg;
    private float phase3Dmg;
    private float phase4Dmg;

    // Start is called before the first frame update
    void Start()
    {
        Health = this.GetComponent<Health>();
        parent_MonSpawn = GetComponentInParent<Monster_Spawner>();
        player = GameObject.FindGameObjectWithTag("Player").transform.root.gameObject;
        baseObj = GameObject.FindGameObjectWithTag("Base").transform.root.gameObject;
        agent = GetComponent<NavMeshAgent>();
        Damage = GameStats.BaseEnemyDamage[3] * GameStats.EnemyHealthModifier[(int)GameManager.Instance.CurrentDifficulty];
        animatorBoss = GetComponentInChildren<Animator>();
        phase1Dmg = Damage + (Damage * 0.1f);
        phase2Dmg = Damage + (Damage * 0.2f);
        phase3Dmg = Damage + (Damage * 0.3f);
        phase4Dmg = Damage + (Damage * 0.5f);

        //animatorBoss = GetComponentInChildren<animatorBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckWave();

        if (playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer))
        {
            playerSpotted = true;
        }
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);
        baseInSightRange = Physics.CheckSphere(transform.position, sightRange, isBase);
        baseInAttackRange = Physics.CheckSphere(transform.position, attackRange, isBase);

        //Boss only spawns in Waves
        //if player not spotted yet / at all
        if ((!playerInSightRange && !playerInAttackRange && (!playerSpotted || playerSpotted) && (!baseInSightRange || baseInSightRange) && (!baseInAttackRange || baseInAttackRange) && (isWave || !isWave)))
        {
            //print("Wandering(!isWave)");
            animatorBoss.SetBool("ChasingPlayer", true);
            agent.speed = wanderSpeed;
            Wandering();
        }
        //If player is in range of enemy's sight, monster will chase.
        if ((playerInSightRange && !playerInAttackRange && (playerSpotted || !playerSpotted) && (!baseInSightRange || baseInSightRange) && (!baseInAttackRange || baseInAttackRange) && (isWave || !isWave)))
        {
            //print("Chasing Player(!isWave)");
            animatorBoss.SetBool("ChasingPlayer", true);
            agent.speed = chaseSpeed;
            Chase();
        }
        //If player is in attack range and in sight range, monster will attack.
        if (playerInSightRange && playerInAttackRange && playerSpotted && (!baseInSightRange || baseInSightRange) && (!baseInAttackRange || baseInAttackRange) && (isWave || !isWave))
        {
            //print("Attacking Player(!isWave)");
            animatorBoss.SetBool("AttackPlayer", true);
            Attack(player);
        }

        //Need to have different health mechanics if loops
        if (Health.CurrentHealth >= Health.MaxHealth * 0.75)
        {
            print("Boss Phase 1");
            //newTimeBetweenAttacks = 2 * 0.9;
            //timeBetweenAttacks = (float)newTimeBetweenAttacks;
            //Damage = phase1Dmg;
            //Phase 1
        }
        if(Health.CurrentHealth >= Health.MaxHealth * 0.50 && Health.CurrentHealth < Health.MaxHealth * 0.75)
        {
            print("Boss Phase 2");
            newTimeBetweenAttacks = 2 * 1.1;
            timeBetweenAttacks = (float)newTimeBetweenAttacks;
            Damage = phase2Dmg;
            //Phase 2
        }
        if(Health.CurrentHealth >= Health.MaxHealth * 0.25 && Health.CurrentHealth < Health.MaxHealth * 0.50)
        {
            print("Boss Phase 3");
            newTimeBetweenAttacks = 2 * 1.2;
            timeBetweenAttacks = (float)newTimeBetweenAttacks;
            Damage = phase3Dmg;
            //Phase 3
        }
        if(Health.CurrentHealth <= Health.MaxHealth * 0.25)
        {
            print("Boss Final Phase");
            newTimeBetweenAttacks = 2 * 1.4;
            timeBetweenAttacks = (float)newTimeBetweenAttacks;
            Damage = phase4Dmg;
            //Final Phase
        }
    }

    public override void Wandering()
    {
        animatorBoss.SetBool("ChasingPlayer", false);
        animatorBoss.SetBool("AttackPlayer", false);
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

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    public override void Chase()
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, player.transform.position, -1, path);
        agent.path = path;
    }

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

    public override void Attack(GameObject target)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, transform.position, -1, path);
        agent.path = path;
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));

        if(!alreadyAttacked)
        {
            if (target.gameObject.tag == "Player")
            {
                target.gameObject.GetComponent<Health>().TakeDamage(Damage);

                Debug.Log("Hit Player");
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
        if (creepDrop != null)
        {
            GameManager.Instance.PlayerInventory.AddItem(creepDrop, 2 + dropBonus);
        }
        if (Random.Range(0, 101) <= 50 && essence != null)
        {
            GameManager.Instance.PlayerInventory.AddItem(essence, 1 + dropBonus);
        }
        if(Random.Range(0, 101) <= 10 && bossCore != null)
        {
            GameManager.Instance.PlayerInventory.AddItem(bossCore, 1 + dropBonus);
        }
        if (gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }
}