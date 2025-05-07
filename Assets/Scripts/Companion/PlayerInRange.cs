using System;
using UnityEngine;

public class PlayerInRange : MonoBehaviour
{
    public static PlayerInRange instance;

    public bool playerInRange;
    
    private void Start()
    {
        instance = this;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerInRange) playerInRange = false;
    }
}
