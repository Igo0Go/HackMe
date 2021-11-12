using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColorSecuritySystem : MonoBehaviour
{
    public Action<string> stateSavedEvent;

    [SerializeField, Range(0,6)] private int threshold = 0;

    private ColorStateProcessor processor;

    public void SetUp()
    {
        processor = new ColorStateProcessor(threshold);
    }

    public void AddState(int[] state)
    {
        if(!processor.IsAlienState(state))
        {
            processor.AddToAlien(state);
            stateSavedEvent?.Invoke("Состояние сохранено");
        }
    }

    public bool IsAlienClass(int[] state, out string message)
    {
        if (processor.IsAlienStateClass(state))
        {
            message = "Отслеживается";
            return true;
        }
        message = "Не отслеживается";
        return false;
    }
}
