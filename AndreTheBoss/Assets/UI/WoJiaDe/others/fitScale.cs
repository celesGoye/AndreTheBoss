using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class fitScale : MonoBehaviour
{
	public float heightRatio;
	[Range(0,1)]
	public float widthScale;
	
	private RectTransform rt;
    void OnEnable()
	{
		rt=this.GetComponent<RectTransform>();
	}
    void Update()
    {
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, UnityEngine.Screen.height*widthScale);
		rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, UnityEngine.Screen.height*widthScale*heightRatio);
    }
}
