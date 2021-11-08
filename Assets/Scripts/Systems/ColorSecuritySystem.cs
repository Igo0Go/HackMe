using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColorSecuritySystem : MonoBehaviour
{
    public static Action<string> stateSavedEvent;

    private static List<int[]> savedStates = new List<int[]>();

    public static void AddState(int[] state)
    {
        if(!IsSavedState(state))
        {
            Debug.Log(string.Format("|b|h|la|ra|ll|rl|"));
            string result = "|";
            int[] item = new int[6];

            for (int i = 0; i < state.Length; i++)
            {
                result += state[i] + "|";
                item[i] = state[i];
            }
            Debug.Log(result);
            savedStates.Add(item);
            stateSavedEvent?.Invoke("Состояние сохранено");
        }
    }

    public static string CheckState(int[] state)
    {
        if(IsSavedState(state))
        {
            return "Отслеживается";
        }
        return "Не отслеживается";
    }

    private static bool IsSavedState(int[] state)
    {
        for (int i = 0; i < savedStates.Count; i++)
        {
            if(CompareStates(savedStates[i], state))
            {
                return true;
            }
        }
        return false;
    }

    private static bool CompareStates(int[] leftstate, int[] rightState)
    {
        for (int i = 0; i < leftstate.Length; i++)
        {
            if(leftstate[i] != rightState[i])
            {
                return false;
            }
        }
        return true;
    }
}
