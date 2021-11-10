using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class CharacterInteractionReactor : MonoBehaviour
{
    public Action saveStateEvent;
    public Action<SuitSystem> suitSystemPointUsed;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Interaction"))
        {
            saveStateEvent?.Invoke();
        }
        else if(other.CompareTag("SuitPoint"))
        {
            SuitSystem suitPoint = other.GetComponent<SuitSystem>();
            suitSystemPointUsed?.Invoke(suitPoint);
            suitPoint.OpenSystem(transform);
        }
    }
}
