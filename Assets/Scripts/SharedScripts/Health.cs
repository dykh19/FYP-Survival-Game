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

    private void Awake()
    {
        if (this.CompareTag("Player"))
        {
            GameManager.Instance.LoadData += LoadHealth;
        }
        //If owner of this health script is enemy, adjust the health based on difficulty and mob type
        if (this.CompareTag("Enemy"))
        {
            if (this.GetComponent("CreepAI") != null)
            {
                MaxHealth = GameStats.BaseEnemyHealth[0] * GameStats.EnemyHealthModifier[(int)GameManager.Instance.CurrentDifficulty];
            }
            else if (this.GetComponent("EliteMAI") != null)
            {
                MaxHealth = GameStats.BaseEnemyHealth[1] * GameStats.EnemyHealthModifier[(int)GameManager.Instance.CurrentDifficulty];
            }
            else if (this.GetComponent("EliteRAI") != null)
            {
                MaxHealth = GameStats.BaseEnemyHealth[2] * GameStats.EnemyHealthModifier[(int)GameManager.Instance.CurrentDifficulty];
            }
            else if (this.GetComponent("Boss") != null)
            {
                MaxHealth = GameStats.BaseEnemyHealth[3] * GameStats.EnemyHealthModifier[(int)GameManager.Instance.CurrentDifficulty];
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.LoadingSavedGame == true && this.CompareTag("Player"))
        {
            return;
        }
        else
        {
            // Set Current health to Max Health set in awake
            CurrentHealth = MaxHealth;
        }
        
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

    //Change the Health from default value
    public void SetHealth(float newHealth)
    {
        MaxHealth = newHealth;
        CurrentHealth = newHealth;
    }

    void LoadHealth()
    {
        MaxHealth = GameManager.Instance.PlayerStats.MaxHealth;
        CurrentHealth = GameManager.Instance.PlayerStats.CurrentHealth;
    }

}
