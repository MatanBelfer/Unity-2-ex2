using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class SurfaceTypeManager : MonoBehaviour
{
    [SerializeField] private string[] surfaceNames;
    public static SurfaceTypeManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnValidate()
    {
        surfaceNames = NavMesh.GetAreaNames();
    }

    public string GetSurfaceNameByMask(int mask)
    {
        int index = math.ceillog2(mask);
        return surfaceNames[index];
    }
}
