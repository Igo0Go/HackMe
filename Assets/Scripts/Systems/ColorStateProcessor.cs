using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ColorStateProcessor
{
    private List<int[]> alienStates;
    private List<int[]> mineStates;
    private StatePack pack;

    private int threshold;

    public ColorStateProcessor(int threshold)
    {
        pack = new StatePack();

        mineStates = new List<int[]>()
        {
            //          b  h la ra  ll rl
            new int[] { 6, 6, 6, 6, 6, 6},
            new int[] { 6, 3, 6, 6, 6, 6},
            new int[] { 6, 3, 3, 3, 6, 6},
            new int[] { 6, 3, 3, 3, 3, 3},
            new int[] { 3, 3, 3, 3, 3, 3},
        };
        alienStates = new List<int[]>()
        {
            new int[] { 0, 0, 0, 0, 0, 0}
        };
        this.threshold = threshold;
        RecalculateWeightsForClasses();
    }

    public bool CheckState(int[] state)
    {
        if(IsAlienState(state) || IsMineState(state))
        {
            return true;
        }
        return false;
    }

    public void AddToMine(int[] state)
    {
        int[] newState = new int[]
        {
            state[0], state[1], state[2], state[3], state[4], state[5]
        };
        mineStates.Add(newState);
        RecalculateWeightsForParts(state);
        pack.lastState = newState;
        RecalculateWeightsForClasses();

    }
    public void AddToAlien(int[] state)
    {
        int[] newState = new int[]
        {
            state[0], state[1], state[2], state[3], state[4], state[5]
        };
        alienStates.Add(newState);
        RecalculateWeightsForParts(state);
        pack.lastState = newState;
        RecalculateWeightsForClasses();
    }

    private void RecalculateWeightsForClasses()
    {
        float bufer = 0;

        for (int i = 0; i < mineStates.Count; i++)
        {
            bufer += CalculateWeightForState(mineStates[i]);
        }

        pack.weightForMine = bufer / mineStates.Count;

        bufer = 0;

        for (int i = 0; i < alienStates.Count; i++)
        {
            bufer += CalculateWeightForState(alienStates[i]);
        }

        pack.weightForAlien = bufer / alienStates.Count;
    }

    private void RecalculateWeightsForParts(int[] currentState)
    {
        //рассчитываем смещение цветов по каждой части
        float[] vector = new float[]
        {
            pack.lastState[0] - currentState[0],
            pack.lastState[1] - currentState[1],
            pack.lastState[2] - currentState[2],
            pack.lastState[3] - currentState[3],
            pack.lastState[4] - currentState[4],
            pack.lastState[5] - currentState[5]
        };

        //нормализуем вектор
        float maxModule = 0;
        foreach (var item in vector)
        {
            if(Mathf.Abs(item) > maxModule)
            {
                maxModule = Mathf.Abs(item);
            }
        }

        //смещаем веса
        for (int i = 0; i < vector.Length; i++)
        {
            pack.weightForParts[i] += Mathf.Clamp(pack.weightForParts[i] + vector[i], 0.1f, 1);
        }

        float maxValue = 0;
        for (int i = 0; i < pack.weightForParts.Length; i++)
        {
            if (maxValue < pack.weightForParts[i])
            {
                maxValue = pack.weightForParts[i];
            }
        }
        for (int i = 0; i < pack.weightForParts.Length; i++)
        {
            pack.weightForParts[i] /= maxValue;
        }
    }

    public bool IsAlienState(int[] state)
    {
        for (int i = 0; i < alienStates.Count; i++)
        {
            if (CompareStates(alienStates[i], state, 0))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsMineState(int[] state)
    {
        for (int i = 0; i < mineStates.Count; i++)
        {
            if (CompareStates(mineStates[i], state, 0))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsAlienStateClass(int[] state)
    {
        if(IsAlienState(state)) //он не сменил костюм
        {
            return true;
        }

        if(IsMineState(state))//он в открытую использует нашу форму
        {
            return true;
        }

        float stateWeight = CalculateWeightForState(state);

        if(Mathf.Abs(pack.weightForAlien - stateWeight) < Mathf.Abs(pack.weightForMine - stateWeight))
        {
            return true;
        }
        return false;
    }

    private bool IsNearState(int[] state)
    {
        for (int i = 0; i < alienStates.Count; i++)
        {
            if (CompareStates(alienStates[i], state, threshold))
            {
                return true;
            }
        }
        return false;
    }

    private float CalculateWeightForState(int[] state)
    {
        float result = 0;

        for (int i = 0; i < state.Length; i++)
        {
            result += state[i] * pack.weightForParts[i];
        }

        return result;
    }

    private bool CompareStates(int[] leftstate, int[] rightState, int threshold)
    {
        int currentThreshold = 0;
        for (int i = 0; i < leftstate.Length; i++)
        {
            if (leftstate[i] != rightState[i])
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

public class StatePack
{
    public int[] lastState = new int[] { 0, 0, 0, 0, 0, 0};
    public float weightForMine;
    public float weightForAlien;                   //b    h     la    ra    ll    rl
    public float[] weightForParts = new float[] { 0.3f, 0.2f, 0.3f, 0.3f, 0.3f, 0.3f };
}
