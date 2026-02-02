using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEditor;

public class Controller : MonoBehaviour
{
    private NavMeshAgent agent;
    private int lastAreaIndex;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lastAreaIndex = getCurrentSurfaceIndex();
    }

    void FixedUpdate()
    {
        CheckSurface();
    }


    private int getCurrentSurfaceIndex()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 0.1f, NavMesh.AllAreas);
        return hit.mask;
    }

    private void CheckSurface()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 0.1f, NavMesh.AllAreas))
        {
            int currentAreaIndex = hit.mask;

            if (currentAreaIndex != lastAreaIndex)
            {
                Debug.Log("Area changed to " + currentAreaIndex);
            }

            lastAreaIndex = currentAreaIndex;
        }
    }
}