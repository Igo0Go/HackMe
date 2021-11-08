using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_VectorInputMoveModule : MonoBehaviour, IVectorInputMoveModule
{
    [SerializeField, Tooltip("Точка, относительно которой будут выбираться вектора вперёд и вправо. Может отличаться от камеры")]
    private Transform inputMoveOriginTransform;

    [SerializeField, Tooltip("Главная камера")]
    private Camera viewCamera;

    [SerializeField, Tooltip("Слой, на который будет реагировать клик для движения")]
    private LayerMask moveClickMask;

    public event Action<Vector3> inputVectorEvent;

    private RaycastHit hitInfo;
    private Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            InputPointforMove();
        }
        else
        {
            InputVectorForMove();
        }
    }

    public void InputVectorForMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 direction = inputMoveOriginTransform.forward * z + inputMoveOriginTransform.right * x;

        if(direction != Vector3.zero)
        {
            direction.y = 0;
            direction.Normalize();
            inputVectorEvent?.Invoke(direction);
        }
    }

    public void InputPointforMove()
    {
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitInfo, 100, moveClickMask))
        {
            if(hitInfo.collider.CompareTag("NavMeshGround"))
            {
                Vector3 direction = hitInfo.point - myTransform.position;
                direction.y = 0;
                direction.Normalize();
                inputVectorEvent?.Invoke(direction);
            }
        }
    }
}
