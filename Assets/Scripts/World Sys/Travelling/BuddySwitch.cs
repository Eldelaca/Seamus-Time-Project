using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class BuddySwitch : MonoBehaviour
{
    public Transform companion;
    public Transform teleportTarget; 
    private Transform player;

    private InputSystem_Actions inputActions;
    private InputAction shootAction;

    private bool playerInRange = false;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        shootAction = inputActions.Player.Shoot;
    }

    private void OnEnable()
    {
        shootAction.Enable();
        shootAction.performed += OnShoot;
    }

    private void OnDisable()
    {
        shootAction.performed -= OnShoot;
        shootAction.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (playerInRange && player != null && companion != null && teleportTarget != null)
        {
            NavMeshAgent agent = companion.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(teleportTarget.position, out hit, 5.0f, NavMesh.AllAreas))
                {
                    agent.Warp(hit.position);
                }
                else
                {
                    Debug.LogWarning("Teleport target not on NavMesh. Warping failed.");
                }
            }
            else
            {
                companion.position = teleportTarget.position;
            }

        }
    }
}
