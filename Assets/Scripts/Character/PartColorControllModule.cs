using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartColorControllModule : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> partsWithColor;

    public void ChangeColor(Color color)
    {
        for (int i = 0; i < partsWithColor.Count; i++)
        {
            MeshRenderer part = partsWithColor[i];
            part.material.color = color;
            part.material.SetColor("_EmissionColor", color);
        }
    }
}
