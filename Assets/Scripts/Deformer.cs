using UnityEngine;

public class Deformer : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Vector3[] vertices;

    // Start is called before the first frame update
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        // Modify one single vertex of the mesh
        vertices = meshFilter.mesh.vertices;
        vertices[0] = new Vector3(-1, 0, 0);
        meshFilter.mesh.vertices = vertices;
    }

    // Update is called once per frame
    private void Update()
    {
        // Get Mouse Position in world space
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("mousePos = " + mousePos);
        // Vector3 localMousePos = transform.InverseTransformPoint(mousePos);
        // Debug.Log("localMousePos = " + localMousePos);
        // vertices[0] = localMousePos;
        // meshFilter.mesh.vertices = vertices;
    }
}