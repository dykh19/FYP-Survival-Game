using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyProjectile : MonoBehaviour
{
    public float Damage;

    public void SetDamage(float Damage)
    {
        this.Damage = Damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            other.gameObject.GetComponent<Health>().TakeDamage(Damage);
            
            Debug.Log("Hit Player");
        }

        if (other.gameObject.tag == "Base")
        {
            Destroy(this.gameObject);
            other.gameObject.GetComponent<Health>().TakeDamage(Damage);

            Debug.Log("Hit Base");
        }
    }
}
