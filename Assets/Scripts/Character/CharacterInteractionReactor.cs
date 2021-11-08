using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class CharacterInteractionReactor : MonoBehaviour
{
    public static Action SaveStateEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Interaction"))
        {
            SaveStateEvent?.Invoke();
        }
    }
}
