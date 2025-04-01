using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    private Vector3 startPosition; // Stores the cords of players starting pos
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = transform;
        startPosition = playerTransform.position; // Stores the players first spawn pos when loading into the scene
    }


    // Restart Function
    public void Reset()
    {
        if (CheckpointManager.Instance.HasCheckpoint())
        {
            // Goes to the last Checkpoint
            playerTransform.position = CheckpointManager.Instance.GetCheckpointPosition();
            Debug.Log("Restart last checkpoint");
        }
        else
        {
            // Restarts the whole scene if no checkpoint is found
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
