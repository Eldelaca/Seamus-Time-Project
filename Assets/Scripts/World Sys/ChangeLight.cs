using UnityEngine;

public class ChangeLight : MonoBehaviour
{
    
    public Player_Timeline playerTimeline;  
    public Light directionalLight;          

    private Color defaultColor;

    // Change this line if needed if you want a different Colour
    private Color pastColor = new Color(161f / 255f, 69f / 255f, 69f / 255f); // Grabs the RGB
                                        
    void Start()
    {
        if (directionalLight != null)
        {
            defaultColor = directionalLight.color;
        }
    }

    void Update()
    {
        if (playerTimeline == null || directionalLight == null) return;

        if (playerTimeline.in_Present)
        {
            directionalLight.color = defaultColor;
        }
        else
        {
            directionalLight.color = pastColor;
        }
    }
}
