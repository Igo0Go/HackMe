using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorStateProcessorTest : MonoBehaviour
{
    [SerializeField]
    private int[] currentState;
    private ColorStateProcessor processor;

    Dictionary<int, string> colors = new Dictionary<int, string>();

    private void Awake()
    {
        processor = new ColorStateProcessor(0);
    }

    [ContextMenu("�������� �� ����� ����������")]
    public void test_IsAlienState()
    {
        Debug.Log("IsAlineState()=> " + GetStateString() + ": " + (processor.IsAlienState(currentState) ? "AlienState" : "MineState"));
    }

    [ContextMenu("�������� �� ����� ����������")]
    public void test_IsMineState()
    {
        Debug.Log("IsMineState()=> " + GetStateString() + ": " + (processor.IsMineState(currentState) ? "MineState" : "AlienState"));
    }

    [ContextMenu("�������� �� ����� �������")]
    public void test_IsAlienClass()
    {
        Debug.Log("IsAlineClass()=> " + GetStateString() + ": " + (processor.IsAlienStateClass(currentState) ? "AlienState" : "MineState"));
    }

    [ContextMenu("�������� � �����")]
    public void test_AddToAlien()
    {
        processor.AddToAlien(currentState);
        Debug.Log("AddToAline()=> " + GetStateString() + ": ���������");
    }


    private string GetStateString()
    {
        string result = "[";

        for (int i = 0; i < currentState.Length; i++)
        {
            result += currentState[i];
            if(i < currentState.Length-1)
            {
                result += "|";
            }
        }
        result += "]";

        return result;
    }
}
