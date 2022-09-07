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
        //If owner of this health script is enemy, adjust the health based on difficulty and mob type
        if (this.tag == "Enemy")
        {
            if (this.GetComponent("Creep") != null)
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
        // Set Current health to Max Health set in awake
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
