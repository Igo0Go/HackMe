using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class CharacterInteractionReactor : MonoBehaviour
{
    public Action saveStateEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Interaction"))
        {
            saveStateEvent?.Invoke();
        }
    }
}
