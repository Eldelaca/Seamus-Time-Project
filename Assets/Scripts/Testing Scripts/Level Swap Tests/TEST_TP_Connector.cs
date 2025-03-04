// For relocating objects found in a TP point and relocating them to the corresponding point in other timeline

using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TEST_TP_Connector : MonoBehaviour
{
    [Header("References")] 
    private TEST_Timer tp_Timer_Script;
    [SerializeField] private LayerMask tpable_Layer;
    
    [Header("Teleport Points")] 
    [SerializeField] private Transform present_TP_Point;
    [SerializeField] private Transform past_TP_Point;

    [Header("Visible for testing - Don't touch")]
    public List<GameObject> objects_To_TP;
    [SerializeField] private bool can_TP = true;
    public bool in_Past = false;


    private void Start()
    {
        tp_Timer_Script = GameObject.FindGameObjectWithTag("Timer").GetComponent<TEST_Timer>();
    }// end Start()

    
    // Send objects to past or present based on bool in Player_Script script
    public void TP_Sorter()
    {
        if (can_TP == false)
            return;
        
        can_TP = false;
        
        // Present to past
        if (in_Past == false)
        {
            Check_For_TP_Objects();
            
            foreach (GameObject obj in objects_To_TP)
            {
                TP_To_Past(obj);
            }
            
            in_Past = true;
            
            StartCoroutine(tp_Timer_Script.TP_Timer(gameObject));
        }
        
        // Past to present
        else if (in_Past == true)
        {
            Check_For_TP_Objects();
            
            foreach (GameObject obj in objects_To_TP)
            {
                TP_To_Present(obj);
            }
            
            in_Past = false;
        }
        
        // Small delay to prevent teleporting back on same input
        Invoke(nameof(Reset_TP), 0.25f);
    }// end TP_Sorter()
    
    
    // Checks TP point in a radius to detect non-player objects to TP, adds to list
    public void Check_For_TP_Objects()
    {
        Vector3 target_TP_Point;

        if (in_Past == true)
            target_TP_Point = past_TP_Point.transform.position;
        else
            target_TP_Point = present_TP_Point.transform.position;
        
        Collider[] hit_Objs = Physics.OverlapSphere(target_TP_Point, 5f, tpable_Layer);

        foreach (Collider hit_Obj in hit_Objs)
        {
            objects_To_TP.Add(hit_Obj.gameObject);
        }

    }// end Check_For_TP_Objects()
    
    
    private void TP_To_Past(GameObject obj)
    {
        print("attempting to tp past");
        obj.transform.position = past_TP_Point.position;
    }// end TP_To_Past()

    
    private void TP_To_Present(GameObject obj)
    {
        print("attempting to tp present");
        obj.transform.position = present_TP_Point.position;
    }// end TP_To_Present()

    
    private void Reset_TP()
    {
        objects_To_TP.Clear();
        can_TP = true;
    }// end Reset_TP()

    

    
}// TEST_TP_Connector
