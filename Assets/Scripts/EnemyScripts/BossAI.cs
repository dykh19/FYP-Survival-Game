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
    //Add Boss Core//

    //Pathing Variables
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

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


    Monster_Spawner spawn;

    // Start is called before the first frame update
    void Start()
    {
        Health = this.GetComponent<Health>();
        parent_MonSpawn = GetComponentInParent<Monster_Spawner>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        baseObj = GameObject.FindGameObjectWithTag("Base").transform;
        agent = GetComponent<NavMeshAgent>();
        Damage = GameStats.BaseEnemyDamage[4] * GameStats.EnemyHealthModifier[(int)GameManager.Instance.CurrentDifficulty];
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
        if(Health.CurrentHealth >= Health.MaxHealth * 0.75)
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
            newTimeBetweenAttacks = 2 * 0.8;
            timeBetweenAttacks = (float)newTimeBetweenAttacks;
            Damage = phase2Dmg;
            //Phase 2
        }
        if(Health.CurrentHealth >= Health.MaxHealth * 0.25 && Health.CurrentHealth < Health.MaxHealth * 0.50)
        {
            print("Boss Phase 3");
            newTimeBetweenAttacks = 2 * 0.7;
            timeBetweenAttacks = (float)newTimeBetweenAttacks;
            Damage = phase3Dmg;
            //Phase 3
        }
        if(Health.CurrentHealth <= Health.MaxHealth * 0.25)
        {
            print("Boss Final Phase");
            newTimeBetweenAttacks = 2 * 0.5;
            timeBetweenAttacks = (float)newTimeBetweenAttacks;
            Damage = phase4Dmg;
            //Final Phase
        }
    }

    public override void Chase()
    {
        //agent.SetDestination(player.transform.position);
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, player.transform.position, -1, path);
        agent.path = path;
    }

    public override void Attack(Transform target)
    {
        //agent.SetDestination(transform.position);
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
        if (gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }
}