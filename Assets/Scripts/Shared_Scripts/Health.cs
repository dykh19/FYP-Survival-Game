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

    //Heal the entity by given argument
    public void Heal(float HealthHealed)
    {
        CurrentHealth += HealthHealed;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

    //Damage the entity by given argument, if health reaches 0, kill the entity
    public void TakeDamage(float DamageTaken)
    {
        CurrentHealth -= DamageTaken;
        if(CurrentHealth <= 0)
        {
            // Entity is dead
            HandleDeath();
        }
    }

    //Force kill the entity
    public void ForceKill()
    {
        CurrentHealth = 0;
        HandleDeath();
    }

    //Checks if entity is player or enemy, and handle death accordingly
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
        }
    }

    //Change the Health from default value
    public void SetHealth(float newHealth)
    {
        MaxHealth = newHealth;
        CurrentHealth = newHealth;
    }

}
