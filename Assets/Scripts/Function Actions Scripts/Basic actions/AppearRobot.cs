using UnityEngine;

public class AppearRobot : MonoBehaviour
{
    public Transform companion;
    public Transform tp; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            companion.position = tp.position;
            Destroy(gameObject);
        }
    }
}
