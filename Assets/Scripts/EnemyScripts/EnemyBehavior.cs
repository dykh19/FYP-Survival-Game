using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    //Setting up Surface and Identification variables
    public NavMeshAgent agent;
    public Transform player;
    public Transform baseObj;
    public LayerMask isGround, isPlayer, isBase;

    public Vector3 distanceToWalkPoint;

    public bool isDead, isWave = false;

    //Spotting Player Variables
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, baseInSightRange, baseInAttackRange, playerSpotted;
    public bool isWandering;

    public static int dropBonus = 0;

    public virtual void Die()
    {
        //Override
    }

    public virtual void Wandering()
    {
        //Override
    }

    public virtual void findBase()
    {
        //Override
    }

    public virtual void findWalkPoint()
    {
        //Override
    }

    public virtual void Chase()
    {
        //Override
    }

    public virtual void Attack(Transform target)
    {
        //Override
    }

    public virtual void ResetAttack()
    {
        //Override
    }

    public void CheckWave()
    {
        isWave = this.transform.parent.GetComponent<Monster_Spawner>().isWave;
    }

    //For Debugging Purposes
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
