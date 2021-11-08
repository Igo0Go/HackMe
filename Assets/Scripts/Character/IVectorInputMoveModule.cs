using System;
using UnityEngine;

public interface IVectorInputMoveModule
{
    event Action<Vector3> inputVectorEvent;

    void InputVectorForMove();
}
