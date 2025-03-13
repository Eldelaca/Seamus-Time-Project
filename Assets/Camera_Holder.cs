using System;
using UnityEngine;

public class Camera_Holder : MonoBehaviour
{
    private GameObject player_Ref;


    private void Start()
    {
        player_Ref = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        transform.position = player_Ref.transform.position;
    }
    
}// end Camera_Holder
