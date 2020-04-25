using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    Mesh hexMesh;

    private List<Vector3> vertices;

    private List<int> triangles;

    private List<Color> colors;

    private MeshCollider meshCollider;

    public void OnEnable()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();
        
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
    }

    public void Triangulate(HexCell[] cells)
    {
        if (cells == null)
            return;

        hexMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        colors.Clear();

        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }

        hexMesh.vertices = vertices.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();
        meshCollider.sharedMesh = hexMesh;
        hexMesh.colors = colors.ToArray();
    }

    private void Triangulate(HexCell cell)
    {
        for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; dir++)
        {
            Triangulate(dir, cell);
        }
    }

    private void Triangulate(HexDirection direction, HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        Vector3 v1 = center + HexMetrics.GetFirstSolidCorner(direction);
        Vector3 v2 = center + HexMetrics.GetSecondSolidCorner(direction);
        AddTriangle(center, v1, v2);
        AddTriangleColor(cell);

    }

    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int currentIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(currentIndex);
        triangles.Add(currentIndex + 1);
        triangles.Add(currentIndex + 2);
    }

    private void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int currentIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
        triangles.Add(currentIndex + 2);
        triangles.Add(currentIndex + 1);
        triangles.Add(currentIndex);
        triangles.Add(currentIndex + 2);
        triangles.Add(currentIndex + 3);
        triangles.Add(currentIndex + 1);
    }

    private void AddTriangleColor(HexCell cell)
    {
        colors.Add(cell.color);
        colors.Add(cell.color);
        colors.Add(cell.color);
    }

    private void AddTriangleColor(Color c1, Color c2, Color c3)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
    }

}
