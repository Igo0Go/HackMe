using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class SuitSystem : MonoBehaviour
{
    public Action openColorMenuEvent;

    [SerializeField] private Transform characterPoint;
    [SerializeField] private Transform field;

    [SerializeField] private Vector3 fieldDownLocalPosition;
    [SerializeField] private Vector3 fieldDownLocalScale;
    [SerializeField] private Vector3 fieldUpLocalPosition;
    [SerializeField] private Vector3 fieldUpLocalScale;

    private Transform character;

    [ContextMenu("Сохранить верхнее состояние поля")]
    public void SaveUpfieldPoint()
    {
        fieldUpLocalPosition = field.localPosition;
        fieldUpLocalScale = field.localScale;
    }

    [ContextMenu("Сохранить нижнее состояние поля")]
    public void SaveDownFieldPoint()
    {
        fieldDownLocalPosition = field.localPosition;
        fieldDownLocalScale = field.localScale;
    }

    public void OpenSystem(Transform pers)
    {
        character = pers;
        StartCoroutine(MoveCharacterCoroutine());
    }

    public void CloseSystem()
    {
        StartCoroutine(MoveFieldCoroutine(false));
    }

    private IEnumerator MoveCharacterCoroutine()
    {
        CharacterMove.Blocked = true;

        Vector3 startCharacterPoisition = character.position;
        Quaternion startCharacterRotation = character.rotation;

        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime;

            character.position = Vector3.Lerp(startCharacterPoisition, characterPoint.position, t);
            character.rotation = Quaternion.Lerp(startCharacterRotation, characterPoint.rotation, t);

            yield return null;
        }

        StartCoroutine(MoveFieldCoroutine(true));
        openColorMenuEvent?.Invoke();
    }

    private IEnumerator MoveFieldCoroutine(bool toUp)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;

            if (toUp)
            {
                field.localPosition = Vector3.Lerp(fieldDownLocalPosition, fieldUpLocalPosition, t);
                field.localScale = Vector3.Lerp(fieldDownLocalScale, fieldUpLocalScale, t);
            }
            else
            {
                field.localPosition = Vector3.Lerp(fieldUpLocalPosition, fieldDownLocalPosition, t);
                field.localScale = Vector3.Lerp(fieldUpLocalScale, fieldDownLocalScale, t);
            }
            yield return null;
        }

        if (!toUp)
        {
            CharacterMove.Blocked = false;
        }
    }
}
