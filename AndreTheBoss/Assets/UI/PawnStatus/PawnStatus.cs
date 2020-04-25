using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PawnStatus : MonoBehaviour
{
    public Text txtAttak;
    public Text txtDefense;
    public Text txtLife;
    public Text txtDexterity;
    public Text txtAttackRange;
    //0,0
	public Text txtName;
	public Text txtLevel;
	public Text txtDescribe;
	public Image imgAvatar;
	
	private Sprite sprite;
	
    public void UpdatePawnStatusPanel(Pawn pawn)
    {	
        UpdatePanel(pawn.Attack, pawn.Defense, pawn.Life, pawn.Dexterity, pawn.AttackRange,pawn.Name,pawn.MaxLife,pawn.Describe);
    }
	
	//0,0
    private void UpdatePanel(int attack, int def, int life, int dex, int atkRange,string name,int maxLife,string describe)
    {
        txtAttak.text = attack.ToString();
        txtDefense.text = def.ToString();
		if((float)life/maxLife<0.4f)
			txtLife.text ="<color=#FF0000>"+ life.ToString()+"</color>/"+maxLife.ToString();
		else
			txtLife.text =life.ToString()+"/"+maxLife.ToString();
		txtDescribe.text=describe;
        txtDexterity.text = dex.ToString();
        txtAttackRange.text = atkRange.ToString();
        txtName.text = name.ToString();
		txtLevel.text="0".ToString();
		
		if((sprite=Resources.Load("UI/avatar/avatar_"+name, typeof(Sprite)) as Sprite)!=null)
			imgAvatar.sprite =sprite;
    }
}
