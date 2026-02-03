using System;
using UnityEngine;

public class GizmoShowBounds : MonoBehaviour
{
    [SerializeField] private Color color;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube( transform.position, transform.lossyScale);
    }
}
