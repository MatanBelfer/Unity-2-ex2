using System;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class SurfaceDetector : MonoBehaviour
{
    private int lastAreaMask
    {
        get => _lastAreaMask;
        set
        {
            if (value != _lastAreaMask)
            {
                _lastAreaMask = value;
                OnSurfaceChange?.Invoke(value);
            }
        }
    }

    private int _lastAreaMask = -1;
    public UnityEvent<int> OnSurfaceChange;

    void FixedUpdate()
    {
        CheckSurface();
    }

    private void CheckSurface()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 0.1f, NavMesh.AllAreas))
        {
            lastAreaMask = hit.mask;
        }
    }

    private void Start()
    {
        OnSurfaceChange.AddListener(
            mask => UIManager.Instance.LogMessage($"{gameObject.name} is on surface: " +
                SurfaceTypeManager.Instance.GetSurfaceNameByMask(mask)));
    }
}