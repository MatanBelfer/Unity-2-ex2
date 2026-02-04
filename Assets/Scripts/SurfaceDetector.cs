using UnityEngine;
using UnityEngine.AI;

public class SurfaceDetector : MonoBehaviour
{
    private NavMeshAgent agent;
    private int lastAreaIndex;


    void Start()
    {
        print(string.Join(", ", NavMesh.GetAreaNames()));
        agent = GetComponent<NavMeshAgent>();
        lastAreaIndex = GetCurrentSurfaceMask();
    }

    void FixedUpdate()
    {
        CheckSurface();
    }


    private int GetCurrentSurfaceMask()
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
            int currentAreaMask = hit.mask;

            if (currentAreaMask != lastAreaIndex)
            {
                Debug.Log("Area changed to " + currentAreaMask);
            }

            lastAreaIndex = currentAreaMask;
        }
    }
}