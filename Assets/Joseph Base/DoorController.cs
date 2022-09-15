using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    Animator _doorAnim;

    private void OnTriggerEnter(Collider other)
    {
       _doorAnim.SetBool("character_nearby", true);
    }

    private void OnTriggerExit(Collider other)
    {
       _doorAnim.SetBool("character_nearby", false);
    }

    // Start is called before the first frame update
    void Start()
    {
      _doorAnim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
