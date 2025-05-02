using UnityEngine;

public class Appear : MonoBehaviour
{
    /// Change this when dialogue appears simple script for now
    /// 

    public GameObject appear;

    private void Start()
    {
       if (appear != null)
        {
            appear.SetActive(false);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") &&
            appear != null)
        {
           appear.SetActive(true);  
        }
    }
}
