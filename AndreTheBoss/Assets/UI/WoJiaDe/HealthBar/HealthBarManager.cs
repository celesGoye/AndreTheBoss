using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public List<HealthBar> healthBars;
	public HealthBar healthBar_Prefab;
	public Camera mainCam;
	
	public HealthBar InitializeHealthBar(Pawn pawn)
	{
		HealthBar hb = new HealthBar();
		hb=Instantiate<HealthBar>(healthBar_Prefab);
		healthBars.Add(hb);
		
		hb.transform.SetParent(this.transform);
		hb.pawn=pawn;
		hb.slider=hb.GetComponent<Slider>();
		hb.UpdateLife();
		hb.mainCam=mainCam;
		return hb;
	}
	public void RemoveHealthBar(HealthBar hb)
	{
		int i=0;
		foreach(HealthBar bar in healthBars)
		{
			if(bar==hb)
				healthBars.Remove(bar);
			i++;
		}
	}
}
