using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{

    public bool isInRange;
    public KeyCode interactor;
    public UserInterface shop;
    public PlayerOpenUI UIController;

    // Start is called before the first frame update
    void Start()
    {
        UIController = FindObjectOfType<PlayerOpenUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isInRange)
        {
            if (GameManager.Instance.CurrentGameState == GameState.INGAME && Input.GetKeyDown(interactor))
            {
                foreach (var ui in GameManager.Instance.UserInterfaces)
                {
                    if ((ui.keyCode != KeyCode.None) && Input.GetKeyDown(ui.keyCode))
                    {
                        UIController.OpenUI(shop);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}
