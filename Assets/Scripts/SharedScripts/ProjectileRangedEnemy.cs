using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRangedEnemy : MonoBehaviour
{
    public float Damage;
    /*    public GameObject player;
        public Vector3 targetPlayer;
        public Vector3 direction;
        public float speed = 2f;

    private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                targetPlayer = player.transform.position;
            }
            else
            {
                print("Player = null");
            }

            direction = (targetPlayer - transform.position).normalized * speed;
        }

        private void Update()
        {
            transform.Translate(direction * Time.deltaTime);
        }*/

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
        else if (other.gameObject.tag == "Base")
        {
            Destroy(this.gameObject);
            other.gameObject.transform.root.GetComponent<Health>().TakeDamage(Damage);

            Debug.Log("Hit Base");
        }
    }
}
