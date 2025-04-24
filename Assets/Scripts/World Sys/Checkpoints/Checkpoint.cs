using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();
            Restart playerReset = other.GetComponent<Restart>();

            if (health != null && playerReset != null)
            {
                Debug.Log("Checkpoint is Saved");
                // Save the player's position
                playerReset.SetCheckpointPosition(other.transform.position);
                CheckpointManager.Instance.SetCheckpoint(other.transform.position, health.GetCurrentHealth());
            }

            Destroy(gameObject);
        }
    }
}
