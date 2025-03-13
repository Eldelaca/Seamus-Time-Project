using System;
using UnityEngine;

public class TEST_Two_Track_Swap : MonoBehaviour
{
    [SerializeField] private GameObject present_Camera;
    [SerializeField] private GameObject past_Camera;

    [SerializeField] private bool in_Present = true;
    [SerializeField] private bool in_Past = false;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (in_Present == true)
            {
                print("go past");
                in_Present = false;
                present_Camera.SetActive(false);
                in_Past = true;
                past_Camera.SetActive(true);
            }   
            
            else if (in_Past == true)
            {
                print("go present");
                in_Present = true;
                present_Camera.SetActive(true);
                in_Past = false;
                past_Camera.SetActive(false);
            }
        }
        
    }// end Update()
    
    
}// end TEST_Two_Track_Swapping
