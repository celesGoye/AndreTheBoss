using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public static Color StartColor = Color.blue;
    public static  Color EndColor = Color.green;
    public static Color RouteColor = Color.white;
    public static Color AttackColor = Color.red;

    public void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
