// For use on individual TP point objects, will pass teleportable objects to the TP Connector in parent object

using System;
using UnityEngine;

public class TEST_TP_Points : MonoBehaviour
{
    private TEST_TP_Connector connector_Script;
    [SerializeField] private bool is_Present;
    private bool obj_Detected = false;
    
    private void Start()
    {
        connector_Script = GetComponentInParent<TEST_TP_Connector>();
    }// end Start()
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && obj_Detected == true)
        {
            print("eh");
            connector_Script.TP_Sorter(is_Present);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        // *** Need to add other item tags
        if (other.CompareTag("Player") || other.CompareTag("Can TP"))
        {
            connector_Script.objects_To_TP.Remove(other.gameObject);
            obj_Detected = false;
        }
        
    }// end OnTriggerExit()
    
    // Check that collider is an allowed type to teleport
    private void OnTriggerEnter(Collider other)
    {  
        // *** Need to add other item tags
        if (other.CompareTag("Player") || other.CompareTag("Can TP"))
        {
            connector_Script.objects_To_TP.Add(other.gameObject);
            obj_Detected = true;
        }
        
    }// end OnTriggerEnter()
    
}// end TEST_TP_Points
