using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    private Vector3 lastCheckpointPosition;
    private bool hasCheckpoint = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Doesn't destroy the data between each scene reload
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Updates new Checkpoint
    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
        hasCheckpoint = true;
    }
    
    // Grabs last checkpoint
    public Vector3 GetCheckpointPosition()
    {
        return lastCheckpointPosition; 
    }

    // Boolean Check
    public bool HasCheckpoint()
    {
        return hasCheckpoint;
    }
}
