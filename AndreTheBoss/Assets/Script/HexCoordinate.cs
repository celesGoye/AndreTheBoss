using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HexCoordinate
{
    [SerializeField]
    private int x, z;

    public HexCoordinate(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static HexCoordinate FromOffsetCoordinate(int x, int z)
    {
        return new HexCoordinate(x - z/2, z);
    }

    public static HexCoordinate FromPosition(Vector3 position)
    {
        float offset = position.z / (HexMetrics.OuterRadius * 3f);
        float x = position.x / (HexMetrics.InnerRadius * 2);
        float y = -x;
        x -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x-y);

        if(iX + iY + iZ != 0)
        {
            Debug.Log("rounding error!");
            float dx = Mathf.Abs(iX - x);
            float dy = Mathf.Abs(iY - y);
            float dz = Mathf.Abs(iZ + x + y);
            if(dx > dy && dx > dz)
            {
                iX = -iY - iZ;
            }
            else if(dz > dy)
            {
                iZ = -iY - iX;
            }
        }

        return new HexCoordinate(iX, iZ);
    }

    public int X
    {
        get { return x; }
        set { x = value; }
    }

    public int Z
    {
        get { return z; }
        set { z = value; }
    }

    public int Y
    {
        get { return -X-Z; }
    }

    public override string ToString()
    {
        return "(" + x.ToString() + ", " + Y.ToString() + ", " + z.ToString() + ")";
    }

    public string ToStringOnSeperateLines()
    {
        return x.ToString() + "\n" + Y.ToString() + "\n" + z.ToString();
    }
}
