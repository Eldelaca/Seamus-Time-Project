using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class DoorTransition : MonoBehaviour
{
    [Header("Door Settings")]
    public GameObject targetLocation;
    public GameObject companion;

    private bool playerInRange;

    private InputSystem_Actions inputSystemActions;
    private InputAction inputAction;
    private GameObject player;

    private void Awake()
    {
        inputSystemActions = new InputSystem_Actions();
        inputAction = inputSystemActions.Player.Input;
    }

    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.performed += OnInputPerformed;
    }

    private void OnDisable()
    {
        inputAction.performed -= OnInputPerformed;
        inputAction.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.gameObject;
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

    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        TryTeleport();
    }

    private void TryTeleport()
    {
        if (playerInRange && targetLocation != null && player != null)
        {
            player.transform.position = targetLocation.transform.position;

            if (companion != null)
            {
                NavMeshAgent agent = companion.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(targetLocation.transform.position, out hit, 5.0f, NavMesh.AllAreas))
                    {
                        agent.Warp(hit.position);
                    }
                    else
                    {
                        Debug.LogWarning("Target location not on NavMesh. Companion warping failed.");
                        companion.transform.position = targetLocation.transform.position;
                    }
                }
                else
                {
                    companion.transform.position = targetLocation.transform.position;
                }
            }
        }
    }
}
