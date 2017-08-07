
using UnityEngine;
using System.Collections.Generic;

public class LowPoly : MonoBehaviour 
{
    private MeshFilter MeshFilter = null;
    public int XCount = 5;
    public int YCount = 5;

    private void Awake()
    {
        MeshFilter = this.GetComponent<MeshFilter>();
    }

    private void GenerateLowPoly()
    {
        Mesh mesh = new Mesh();
        mesh.name = "LowPoly";
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        // 1. calc vertices pos by random arithmetic

        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    private void OnDrawGizmos()
    {
        Mesh mesh = new Mesh();
        mesh.name = "LowPoly";
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        // 1. calc vertices pos by random arithmetic

        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
    }
}