using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexStatus : MonoBehaviour
{
    public Text txtDescribe;
    public Text txtType;


    public void UpdateHexStatusPanel(HexCell hexCell)
    {
        UpdatePanel(hexCell.hexType);
    }
    private void UpdatePanel(HexType hexType)
    {
        txtDescribe.text="describe".ToString();
		txtType.text=hexType.ToString();
    }
}
