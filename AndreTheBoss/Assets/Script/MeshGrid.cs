using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


public class MeshGrid : MonoBehaviour
{
    public int sizeX = 10;
    public int sizeZ = 5;

    public HexCell cellPrefab;

    private HexCell[] cells;

    private HexMesh hexMesh;

    public Text cellLabelPrefab;

    private Canvas gridCanvas;


    public Color defaultColor = Color.white;
    public Color selectedColor = Color.magenta;

   public void OnEnable()
    {
        if(cells == null)
        {
            cells = new HexCell[sizeX * sizeZ];
        }
            

        gridCanvas = GetComponentInChildren<Canvas>();

        int i = 0;
        for (int z = 0; z < sizeZ; z++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                CreateCells(x, z, i++);
            }
        }

        hexMesh = GetComponent<HexMesh>();

        hexMesh.Triangulate(cells);
    }

    void CreateCells(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (2 * HexMetrics.InnerRadius);
        position.y = 0;
        position.z = z * (HexMetrics.OuterRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);

        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinate = HexCoordinate.FromOffsetCoordinate(x, z);
        cell.color = defaultColor;
        
        if (x > 0)
        {
            cell.SetNeighbour(HexDirection.W, cells[i - 1]);
        }
        if(z > 0)
        {
            if((z & 1) == 0)
            {
                cell.SetNeighbour(HexDirection.SE, cells[i - sizeX]);
                if(x > 0)
                {
                    cell.SetNeighbour(HexDirection.SW, cells[i - sizeX - 1]);
                }
            }
            else
            {
                cell.SetNeighbour(HexDirection.SW, cells[i - sizeX]);
                if(x < sizeX-1)
                {
                    cell.SetNeighbour(HexDirection.SE, cells[i - sizeX + 1]);
                }
            }
        }

        Text text = Instantiate<Text>(cellLabelPrefab);
        text.rectTransform.SetParent(gridCanvas.transform, false);
        text.rectTransform.anchoredPosition = new Vector2(position.x, position.z);

        text.text = cell.coordinate.ToStringOnSeperateLines();

        cell.uiRect = text.rectTransform;
    }

    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinate coordinate = HexCoordinate.FromPosition(position);
        int index = coordinate.X + coordinate.Z * sizeX + coordinate.Z / 2;
        return cells[index];
    }

    public void Refresh()
    {
        hexMesh.Triangulate(cells);
    }

}
