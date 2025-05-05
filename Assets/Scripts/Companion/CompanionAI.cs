using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class CompanionAI : MonoBehaviour
{
    public DialogueController dialogueController;
    
    public Transform playerLocation;
    public GameObject currentClue;

    private enum State { Patrolling , Investigating , Identification }
    private State currentState;

    private NavMeshAgent agent;

    public GameObject canvas;
    public TMP_Text text;

    public bool inPlace;
    
    public static CompanionAI instance;

    private void Start()
    {
        instance = this;
        
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Patrolling;
        canvas.SetActive(false);
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                break;
            case State.Investigating:
                Investigate();
                break;
            case State.Identification:
                Identify();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Clue"))
        {
            currentClue = other.gameObject;
            currentState = State.Investigating;
        }
    }

    private void Patrol()
    {
        Vector3 hoverOffset = -playerLocation.forward * 2 + Vector3.up;
        Vector3 targetPosition = playerLocation.position + hoverOffset;
        agent.SetDestination(targetPosition);
        inPlace = false;
    }

    private void Investigate()
    {
        if (!currentClue) return;

        Vector3 targetPosition = currentClue.transform.position + Vector3.up * 1.5f;
        agent.SetDestination(targetPosition);

        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance <= agent.stoppingDistance + 2 && !agent.pathPending)
        {
            agent.ResetPath();
            currentState = State.Identification;
        }
    }


    private void Identify()
    {
        inPlace = true;
        
        Vector3 direction = (playerLocation.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            lookRotation,
            120f * Time.deltaTime
        );
        
        
        if (dialogueController.conversationOver)
        {
            currentState = State.Patrolling;
        }
    }
}