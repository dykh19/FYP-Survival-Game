using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float HealAmount;
    public AudioClip HealSFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().Heal(HealAmount);
            transform.GetChild(0).gameObject.SetActive(false);
            other.GetComponent<AudioSource>().PlayOneShot(HealSFX);
            Invoke("DelayedDestroy", 2);
            
        }
    }

    void DelayedDestroy()
    {
        Destroy(gameObject);
    }
}
