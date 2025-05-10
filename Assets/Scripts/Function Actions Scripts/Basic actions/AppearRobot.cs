using UnityEngine;
using UnityEngine.AI;

public class AppearRobot : MonoBehaviour
{
    public Transform companion;
    public Transform tp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (companion != null && tp != null)
            {
                NavMeshAgent agent = companion.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(tp.position, out hit, 5.0f, NavMesh.AllAreas))
                    {
                        agent.Warp(hit.position);
                    }
                    else
                    {
                        // Checking if its finding the navmesh or not
                        Debug.LogWarning("Teleport destination not on NavMesh. Warping failed.");
                    }
                }
                else
                {
                    companion.position = tp.position;
                }
            }

            Destroy(gameObject);
        }
    }
}
