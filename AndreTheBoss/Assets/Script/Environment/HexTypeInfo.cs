using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTypeInfo : MonoBehaviour
{
    public HexType hexType;
    
    public void ChangeType(HexType hexType)
    {
        this.hexType = hexType;
        //TextMesh tm = transform.GetComponentsInChildren<TextMesh>()[0];
        //tm.text = (hexType != HexType.Plain) ? hexType.ToString() : "";
    }
}
