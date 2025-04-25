// For use on individual TP point objects, will pass teleportable objects to the TP Connector in parent object

using System;
using UnityEngine;

public class TP_Points : MonoBehaviour
{
    [Header("Is this point in the Present timeline?")]
    [SerializeField] private bool is_Present;
    
    private TP_Connector connector_Script;
    private PlayerMovement player_Script;
    
    
    private void Start()
    {
        connector_Script = GetComponentInParent<TP_Connector>();
        player_Script = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }// end Start()
    
    
    // Check that collider is an allowed type to teleport
    private void OnTriggerEnter(Collider other)
    {  
        if (other.CompareTag("Player"))
        {
            connector_Script.objects_To_TP.Add(other.gameObject);
            player_Script.current_TP_Connector = connector_Script;
        }
        
    }// end OnTriggerEnter()    
    
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            connector_Script.objects_To_TP.Remove(other.gameObject);
            player_Script.current_TP_Connector = null;
        }
        
    }// end OnTriggerExit()
    
    
}// end TP_Points
