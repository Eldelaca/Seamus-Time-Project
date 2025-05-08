using UnityEngine;

public class Wave : MonoBehaviour
{
    public float scale = 1.0f;  // Wave scale
    public float speed = 1.0f;  // Wave speed
    private bool recalculateNormals = false;

    public GameObject targetObject; 
    private Vector3[] baseVertices;
    private Vector3[] vertices;

    void Start()
    {
        if (targetObject == null) 
        {
            targetObject = gameObject;
        }
    }

    void Update()
    {
        CalcNoise();
    }

    void CalcNoise()
    {
        if (targetObject == null)
            return;

        Mesh mesh = targetObject.GetComponent<MeshFilter>().mesh;
        if (baseVertices == null)
            baseVertices = mesh.vertices;

        vertices = new Vector3[baseVertices.Length];

        float timex = Time.time * speed + 0.1365143f;
        float timey = Time.time * speed + 1.21688f;
        float timez = Time.time * speed + 2.5564f;

        // Apply Perlin noise to the object's vertices to create a "wave" effect
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseVertices[i];
            vertex.x += Mathf.PerlinNoise(timex + vertex.x, timex + vertex.y) * scale;
            vertex.y += Mathf.PerlinNoise(timey + vertex.x, timey + vertex.y) * scale;
            vertex.z += Mathf.PerlinNoise(timez + vertex.x, timez + vertex.y) * scale;
            vertices[i] = vertex;
        }

        mesh.vertices = vertices;
        if (recalculateNormals)
        {
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }

    public void AdjustSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void ResetSpeed(float defaultSpeed)
    {
        speed = defaultSpeed;
    }
}
