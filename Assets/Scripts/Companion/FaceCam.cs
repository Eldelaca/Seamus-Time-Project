using UnityEngine;

public class FaceCam : MonoBehaviour
{
    public Transform robotPos; 
    public Vector3 offset; 
    public Camera mainCam;

    void LateUpdate()
    {
        if (robotPos != null)
        {
            Vector3 screenPos = mainCam.WorldToScreenPoint(robotPos.position + offset);
            transform.position = screenPos;
        }
    }
}
