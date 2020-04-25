using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    public const float OuterRadius = 2f;
    public const float InnerRadius = OuterRadius * 0.866025404f;

    public const float SolidFactor = 0.98f;
    public const float BlendFactor = 1 - SolidFactor;

    public const float ElevationStep = 1f;

    public const int TerracesPerSlope = 2;
    public const int TerraceSteps = TerracesPerSlope * 2 + 1;

    public const float HorizontalTerraceStepSize = 1f / TerraceSteps;
    public const float VerticalTerraceStepSize = 1f / (TerracesPerSlope + 1);

    

    public static Vector3[] Corners =
    {
        new Vector3(0, 0, OuterRadius),
        new Vector3(InnerRadius, 0, OuterRadius/2),
        new Vector3(InnerRadius, 0, -OuterRadius/2),
        new Vector3(0, 0, -OuterRadius),
        new Vector3(-InnerRadius, 0, -OuterRadius/2),
        new Vector3(-InnerRadius, 0, OuterRadius/2),
        new Vector3(0, 0, OuterRadius),
    };


    public static Vector3 GetFirstCorner(HexDirection direction)
    {
        return Corners[(int)direction];
    }

    public static Vector3 GetSecondCorner(HexDirection direction)
    {
        return Corners[(int)direction + 1];
    }

    public static Vector3 GetFirstSolidCorner(HexDirection direction)
    {
        return Corners[(int)direction] * SolidFactor;
    }

    public static Vector3 GetSecondSolidCorner(HexDirection direction)
    {
        return Corners[(int)direction + 1] * SolidFactor;
    }

    public static Vector3 GetBridge(HexDirection direction)
    {
        return (GetFirstCorner(direction) + GetSecondCorner(direction)) * BlendFactor;
    }

    public static Vector3 TerraceLerp(Vector3 a, Vector3 b, int step)
    {
        float h = step * HorizontalTerraceStepSize;
        a.x += (b.x - a.x) * h;
        a.z += (b.z - a.z) * h;

        float v = ((step + 1) / 2) * VerticalTerraceStepSize;
        a.y += (b.y - a.y) * v;
        return a;
    }

    public static Color TerraceLerp(Color a, Color b, int step)
    {
        float h = step * HorizontalTerraceStepSize;
        return Color.Lerp(a, b, h);
    }

}




