using UnityEngine;

public class Restart : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;

    private void Start()
    {
        lastCheckpointPosition = transform.position;
    }

    public void SetCheckpointPosition(Vector3 position)
    {
        lastCheckpointPosition = position;
    }

    public void ResetPosition()
    {
        transform.position = lastCheckpointPosition;
    }
}
