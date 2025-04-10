using UnityEngine;

public class DeathSceneManager : MonoBehaviour
{
    // Reference to the CheckpointManager
    public DeathMenuUI deathUi;

    // Trigger death screen logic from the checkpoint manager
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            deathUi.TriggerDeathScreen();
        }
    }

}
