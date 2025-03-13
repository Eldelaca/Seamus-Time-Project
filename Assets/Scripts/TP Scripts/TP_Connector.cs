// For relocating objects found in a TP point and relocating them to the corresponding point in other timeline
// Also handles detection of player to determine if they are interacting with this Connector's points.

using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TP_Connector : MonoBehaviour
{
    [Header("References")] 
    private TP_Timer tp_Timer_Script;
    private Rigidbody player_RB;
    [SerializeField] private LayerMask tpable_Layer;
    
    [Header("Teleport Points")] 
    [SerializeField] private GameObject present_TP_Point;
    [SerializeField] private GameObject past_TP_Point;
    
    // Obj_TP_Points for objects such as boxes, separate location to prevent collision and flinging with Player on TP
    [SerializeField] private GameObject present_Obj_TP_Point;
    [SerializeField] private GameObject past_Obj_TP_Point;
    
    [SerializeField] private float tp_Search_Radius; // Radius of OverlapSphere to search for non-player objects to TP
    
    [Header("Visible for testing - Don't touch")]
    public List<GameObject> objects_To_TP;
    [SerializeField] private bool can_TP = true;
    public bool in_Past = false;

    private Coroutine last_Coroutine = null;

    private void Start()
    {
        tp_Timer_Script = GameObject.FindGameObjectWithTag("Timer").GetComponent<TP_Timer>();
        player_RB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }// end Start()


    // Send objects to past or present based current timeline
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
            
            
            last_Coroutine = StartCoroutine(tp_Timer_Script.Timer(gameObject));
        }
        
        // Past to present
        else if (in_Past == true)
        {
            StopCoroutine(last_Coroutine);
            tp_Timer_Script.timer_Active = false;
            
            Check_For_TP_Objects();
            
            foreach (GameObject obj in objects_To_TP)
            {
                TP_To_Present(obj);
            }
            
        }
        
        player_RB.isKinematic = false;
        
        // Small delay to prevent teleporting back on same input and spam related bugs
        Invoke(nameof(Reset_TP), 1f);
    }// end TP_Sorter()
    
    
    // Checks TP point in a radius to detect non-player objects to TP, adds to list
    public void Check_For_TP_Objects()
    {
        // Determines which point to search
        Vector3 target_TP_Point;
        
        if (in_Past == true)
            target_TP_Point = past_TP_Point.transform.position;
        else
            target_TP_Point = present_TP_Point.transform.position;
        
        
        Collider[] hit_Objs = Physics.OverlapSphere(target_TP_Point, tp_Search_Radius, tpable_Layer);

        foreach (Collider hit_Obj in hit_Objs)
        {
            objects_To_TP.Add(hit_Obj.gameObject);
        }

    }// end Check_For_TP_Objects()
    
    
    #region --- TP Functions ---
    
    private void TP_To_Past(GameObject obj)
    {
        //print("attempting to tp past");
        
        if (!obj.gameObject.CompareTag("Player"))
            obj.transform.position = past_Obj_TP_Point.transform.position;
        
        else
            obj.transform.position = past_TP_Point.transform.position;
       
        in_Past = true;
    }// end TP_To_Past()

    
    private void TP_To_Present(GameObject obj)
    {
        //print("attempting to tp present");
        
        if (!obj.gameObject.CompareTag("Player"))
            obj.transform.position = present_Obj_TP_Point.transform.position;
        else
            obj.transform.position = present_TP_Point.transform.position;
        
        in_Past = false;
    }// end TP_To_Present()

    
    private void Reset_TP()
    {
        objects_To_TP.Clear();
        can_TP = true;
    }// end Reset_TP()

    #endregion

    
}// TP_Connector
