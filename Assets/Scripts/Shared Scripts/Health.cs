using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth;

    public float CurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void Heal(float HealthHealed)
    {
        CurrentHealth += HealthHealed;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

    public void TakeDamage(float DamageTaken)
    {
        CurrentHealth -= DamageTaken;
        if(CurrentHealth <= 0)
        {
            // Entity is dead
            HandleDeath();
        }
    }

    public void ForceKill()
    {
        CurrentHealth = 0;
        HandleDeath();
    }

    public void HandleDeath()
    {
        //If player, end game
        if(this.tag == "Player")
        {
            //Do player dies things here
        }

        //If enemy, call enemy death function
        if(this.tag == "Enemy")
        {
            Destroy(this.gameObject);
            Debug.Log("Enemy Dead");
        }
    }

}
