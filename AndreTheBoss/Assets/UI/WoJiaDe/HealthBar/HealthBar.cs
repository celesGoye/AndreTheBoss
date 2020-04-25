using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Camera mainCam;
	public Pawn pawn;
	public Slider slider;
	
	public int offsetx;
	public int offsety;
	
	private float maxlife;
	private float currentlife;
	
	void Update()
	{
		Vector3 pos = mainCam.WorldToScreenPoint(pawn.GetComponent<Transform>().position);
		pos.x -= Screen.width * 0.5f+offsetx;
		pos.y -= Screen.height * 0.5f+offsety;
		transform.localPosition = pos;
	}
	public void UpdateLife(){
		maxlife=pawn.MaxLife;
		currentlife=pawn.Life;
		slider.value=currentlife/maxlife;
	}
}
