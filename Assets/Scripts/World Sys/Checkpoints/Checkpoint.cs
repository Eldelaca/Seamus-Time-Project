using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Checkpoint is Saved");
            CheckpointManager.Instance.SetCheckpoint(other.transform.position);
        }
    }
}
