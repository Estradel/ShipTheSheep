using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deformer : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private Vector3[] vertices;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        // Modify one single vertex of the mesh
        vertices = meshFilter.mesh.vertices;
        vertices[0] = new Vector3(-1, 0, 0);
        meshFilter.mesh.vertices = vertices;
    }

    // Update is called once per frame
    void Update()
    {
        // Get Mouse Position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("mousePos = " + mousePos);
        // Vector3 localMousePos = transform.InverseTransformPoint(mousePos);
        // Debug.Log("localMousePos = " + localMousePos);
        // vertices[0] = localMousePos;
        // meshFilter.mesh.vertices = vertices;
    }
}
