using System;
using UnityEngine;

public class Test_Player_Movement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject present_Player_Ref;
    private CharacterController present_Player_Controller;
    
    [SerializeField] private GameObject past_Player_Ref;
    private CharacterController past_Player_Controller;
    
    [Header("Movement Values")]
    [SerializeField] private float move_Speed = 5f;

    
    private void Start()
    {
        present_Player_Controller = present_Player_Ref.GetComponent<CharacterController>();
        past_Player_Controller = past_Player_Ref.GetComponent<CharacterController>();
    }// end Start()

    private void Update()
    {
        float move_X = Input.GetAxis("Horizontal");
        
        Vector3 movement = new Vector3(move_X * move_Speed * Time.deltaTime, 0f, 0f);
        present_Player_Controller.Move(movement);
        past_Player_Controller.Move(movement);
        
    }
    
}// end Test_Player_Movement
