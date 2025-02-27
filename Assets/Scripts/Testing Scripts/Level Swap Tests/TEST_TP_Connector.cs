// For relocating objects found in a TP point and relocating them to the corresponding point in other timeline

using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TEST_TP_Connector : MonoBehaviour
{
    [Header("Teleport Points")] 
    [SerializeField] private Transform present_TP_Point;
    [SerializeField] private Transform past_TP_Point;

    public List<GameObject> objects_To_TP;
    private bool can_TP = true;

    // Send objects to past or present based on bool in Player_Script script
    public void TP_Sorter(bool in_Present)
    {
        if (can_TP == false)
            return;
        
        can_TP = false;
        
        if (in_Present == true)
        {
            foreach (GameObject obj in objects_To_TP)
            {
                TP_To_Past(obj);
            }
        }
        
        else if (in_Present == false)
        {
            foreach (GameObject obj in objects_To_TP)
            {
                TP_To_Present(obj);
            }
        }
        
        Invoke(nameof(Reset_TP), 0.25f);
    }// end TP_Sorter()
    
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
        can_TP = true;
    }
    
}// TEST_TP_Connector
