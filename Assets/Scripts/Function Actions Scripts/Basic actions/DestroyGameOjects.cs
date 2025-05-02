using UnityEngine;

public class DestroyGameOjects : MonoBehaviour
{
    /// <summary>
    ///  Simply Destroys things on Contact
    ///  Short Time Use
    /// </summary>

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
