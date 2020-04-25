using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleType obstacleType;

    public Material thornsMaterial;
    public Material stoneMaterial;

    public void OnEnable()
    {
        obstacleType = ObstacleType.None;
    }

    public void SetType(ObstacleType type)
    {
        obstacleType = type;
        if (type == ObstacleType.Stones)
        {
            gameObject.GetComponent<Renderer>().material = stoneMaterial;
        }
        else if (type == ObstacleType.Thorns)
        {
            gameObject.GetComponent<Renderer>().material = thornsMaterial;
        }
    }
}
