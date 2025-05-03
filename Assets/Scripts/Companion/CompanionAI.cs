using UnityEngine;
using UnityEngine.AI;

public class CompanionAI : MonoBehaviour
{
    public Transform playerLocation;
    public GameObject currentClue;

    private enum State { Patrolling, Investigating }
    private State currentState;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Patrolling;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                break;
            case State.Investigating:
                Investigate();
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Clue"))
        {
            currentClue = other.gameObject;
            currentState = State.Investigating;
        }
    }

    void Patrol()
    {
        Vector3 hoverOffset = -playerLocation.forward * 2 + Vector3.up;
        Vector3 targetPosition = playerLocation.position + hoverOffset;
        agent.SetDestination(targetPosition);
    }

    void Investigate()
    {
        if (currentClue)
            agent.SetDestination(currentClue.transform.position);
    }
}