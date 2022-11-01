using UnityEngine;
using System;

public class PlayerInteract : MonoBehaviour
{
    // TODO: Raycast in front of the player to call the target's Interactable effect.

    // TODO: Display an overlay on the player's HUD displaying the action name.
    PlayerInputHandler inputHandler;
    PlayerOpenUI openUI;
    UserInterface vendorUI;
    UserInterface exchangeUI;
    private void Start()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        openUI = GetComponent<PlayerOpenUI>();
        foreach (UserInterface ui in GameManager.Instance.UserInterfaces)
        {
            if (ui.name == "VendorUI")
            {
                vendorUI = ui;
            }
            if (ui.name == "ExchangeUI")
            {
                exchangeUI = ui;
            }
        }
    }
    private void Update()
    {
        if (inputHandler.GetInteractInput())
        {
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 2f, -1, QueryTriggerInteraction.Ignore);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.name == "VendorConsole")
                {
                    Debug.Log("Opened Vendor");
                    openUI.OpenUI(vendorUI);
                }
                else if (hit.collider.name == "ExchangeConsole")
                {
                    Debug.Log("Opened Exchange");
                    openUI.OpenUI(exchangeUI);
                }
            }
        }
    }
}
