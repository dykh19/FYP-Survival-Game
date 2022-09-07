using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float MaxHealth;

    public float CurrentHealth;

    //public UnityAction OnPlayerDie;

    public bool IsDead = false;

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
        if (!IsDead)
        {
            CurrentHealth -= DamageTaken;
            if(CurrentHealth <= 0)
            {
                // Entity is dead
                IsDead = true;
                HandleDeath();
            }
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
            GameManager.Instance.OnPlayerDie?.Invoke();
        }

        //If enemy, call enemy death function
        if(this.tag == "Enemy")
        {
            this.GetComponent<EnemyBehavior>().Die();
            Debug.Log("Enemy Dead");
        }

        //Handle Base death
    }

}
