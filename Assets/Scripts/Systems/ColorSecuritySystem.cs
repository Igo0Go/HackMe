using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColorSecuritySystem : MonoBehaviour
{
    public Action<string> stateSavedEvent;

    [SerializeField, Range(0,6)] private int threshold = 0;

    private List<int[]> savedStates = new List<int[]>();

    public void AddState(int[] state)
    {
        if(!IsSavedState(state))
        {
            int[] item = new int[6];

            for (int i = 0; i < state.Length; i++)
            {
                item[i] = state[i];
            }
            savedStates.Add(item);
            stateSavedEvent?.Invoke("Состояние сохранено");
        }
    }

    public bool ContaisState(int[] state, out string message)
    {
        if (IsNearState(state))
        {
            message = "Отслеживается";
            return true;
        }
        message = "Не отслеживается";
        return false;
    }

    private bool IsSavedState(int[] state)
    {
        for (int i = 0; i < savedStates.Count; i++)
        {
            if(CompareStates(savedStates[i], state, 0))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsNearState(int[] state)
    {
        for (int i = 0; i < savedStates.Count; i++)
        {
            if (CompareStates(savedStates[i], state, threshold))
            {
                return true;
            }
        }
        return false;
    }

    private bool CompareStates(int[] leftstate, int[] rightState, int threshold)
    {
        int currentThreshold = 0;
        for (int i = 0; i < leftstate.Length; i++)
        {
            if(leftstate[i] != rightState[i])
            {
                currentThreshold++;
            }
        }

        if (currentThreshold <= threshold)
        {
            return true;
        }

        return false;
    }
}
