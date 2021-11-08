using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(IVectorInputMoveModule))]
[RequireComponent(typeof(Rigidbody))]
public class CharacterMove : MonoBehaviour
{
    [SerializeField, Min(0)] private float speed;

    private Rigidbody rb;
    private Transform myTransform;

    public static bool Blocked { get; set; }


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myTransform = transform;

        IVectorInputMoveModule inputMoveModule = GetComponent<IVectorInputMoveModule>();

        inputMoveModule.inputVectorEvent += MoveByVectorWithSpeed;
    }

    /// <summary>
    /// Перемещать персонажа по вектору
    /// </summary>
    /// <param name="direction">Единичный вектор - направление</param>
    public void MoveByVectorWithSpeed(Vector3 direction)
    {
        if(!Blocked)
        {
            Vector3 newPosition = myTransform.position + direction * speed * Time.deltaTime;
            rb.MovePosition(newPosition);
            myTransform.forward = direction;
        }
    }
}
