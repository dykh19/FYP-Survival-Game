using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 17/08/2022.

public class PlayerStatus : MonoBehaviour
{
    public float playerHealth = 100;
    public int maxPlayerHealth = 100;
    public float playerEnergy = 100;
    public int maxPlayerEnergy = 100;
    public float energyRecoveryRate = 0.1f;
    public bool canInteract = false;

    void FixedUpdate()
    {
        PassiveEnergyRecovery();
    }

    void PassiveEnergyRecovery()
    {
        if (playerEnergy < maxPlayerEnergy)
        {
            var energy = playerEnergy + energyRecoveryRate;
            playerEnergy = Mathf.Clamp(energy, 0, maxPlayerEnergy);
        }
    }

    // TODO: void TakeDamage(int damage)
    // TODO: void Heal(int health)
}
