using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float MaxHealth;

    public float CurrentHealth;

    public float HealthRegenValue = 0f;
    public float TimeLastRegen;

    //public UnityAction OnPlayerDie;

    public bool IsDead = false;

    private void Awake()
    {
        if (this.CompareTag("Player"))
        {
            GameManager.Instance.LoadData -= LoadPlayerHealth;
            GameManager.Instance.SaveData -= SavePlayerHealth;
            GameManager.Instance.LoadData += LoadPlayerHealth;
            GameManager.Instance.SaveData += SavePlayerHealth;
        }

        if (this.CompareTag("Base"))
        {
            GameManager.Instance.SaveData -= SaveBaseHealth;
            GameManager.Instance.SaveData += SaveBaseHealth;
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
        else if (GameManager.Instance.LoadingSavedGame == true && this.CompareTag("Base"))
        {
            LoadBaseHealth();
        }
        else
        {
            // Set Current health to Max Health set in awake
            CurrentHealth = MaxHealth;
        }
        
    }

    private void Update()
    {
        HealthRegen();
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
        if(this.CompareTag("Player"))
        {
            //Do player dies things here
            GameManager.Instance.OnPlayerDie?.Invoke();
        }

        //If enemy, call enemy death function
        if(this.CompareTag("Enemy"))
        {
            this.GetComponent<EnemyBehavior>().Die();
            Debug.Log("Enemy Dead");
        }

        //Handle Base death
        if(this.CompareTag("Base"))
        {
            GameManager.Instance.OnPlayerDie?.Invoke();
        }
    }

    //Change the Health from default value
    public void SetHealth(float newHealth)
    {
        MaxHealth = newHealth;
        CurrentHealth = newHealth;
    }

    public void HealthRegen()
    {
        TimeLastRegen += Time.deltaTime;
        if (TimeLastRegen >= 1f)
        {
            TimeLastRegen = 0f;
            CurrentHealth += HealthRegenValue;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }
    }

    void LoadPlayerHealth()
    {
        MaxHealth = GameManager.Instance.PlayerStats.maxHealth;
        CurrentHealth = GameManager.Instance.PlayerStats.currentHealth;
        //Debug.Log("Loaded Player Health");
        
    }

    void SavePlayerHealth()
    {
        GameManager.Instance.PlayerStats.maxHealth = MaxHealth;
        GameManager.Instance.PlayerStats.currentHealth = CurrentHealth;
        GameManager.Instance.LoadData -= LoadPlayerHealth;
        //Debug.Log("Saved Player Health");
    }

    void LoadBaseHealth()
    {
        MaxHealth = GameManager.Instance.PlayerStats.maxBaseHealth;
        CurrentHealth = GameManager.Instance.PlayerStats.currentBaseHealth;
        Debug.Log("Loaded Base Health");

    }

    void SaveBaseHealth()
    {
        GameManager.Instance.PlayerStats.maxBaseHealth = MaxHealth;
        GameManager.Instance.PlayerStats.currentBaseHealth = CurrentHealth;
        GameManager.Instance.LoadData -= LoadBaseHealth;
        //Debug.Log("Saved Player Health");
    }
}
