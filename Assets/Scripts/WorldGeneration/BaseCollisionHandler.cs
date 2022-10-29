using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollisionHandler : MonoBehaviour
{

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());

    }
}
