using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// NOTE: This is to be attached to Game Objects that can be broken by the player.

[RequireComponent(typeof(AudioSource))]
public class Breakable : MonoBehaviour
{
    public GameItem dropItem;
    public float durability = 50;

    private RectTransform durabilityBar;
    private AudioSource hitSoundEffect;
    private float currentDurability;

    private void Awake()
    {
        hitSoundEffect = GetComponent<AudioSource>();
        currentDurability = durability;
    }

    public void OnHit(float damage)
    {
        hitSoundEffect.Play();
        // TODO: Particles on hit.

        currentDurability -= damage;

        if (currentDurability <= 0)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            if (dropItem is not null)
                GameManager.Instance.playerInventory.AddItem(dropItem);
            Invoke("Break", 1);
        }

        if (durabilityBar is null)
            CreateDurabilityBar();

        UpdateDurabilityBar();
    }

    private void CreateDurabilityBar()
    {
        // TODO: Create the UI bar.
    }

    private void UpdateDurabilityBar()
    {
        // TODO: Update the UI.
    }

    private void Break()
    {
        // TODO: Play "break" sound.
        // TODO: Particles on break.

        Destroy(gameObject);

        
    }
}
