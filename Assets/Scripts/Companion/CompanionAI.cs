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

    void Start()
    {
        instance = this;
        
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Patrolling;
        canvas.SetActive(false);
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
            case State.Identification:
                Identify();
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
        inPlace = false;
    }

    void Investigate()
    {
        if (currentClue)
            agent.SetDestination(currentClue.transform.position + Vector3.up * 1.5f);
    }

    void Identify()
    {
        canvas.SetActive(true);
        inPlace = true;

        if (dialogueController.conversationOver) currentState = State.Patrolling;
    }
}