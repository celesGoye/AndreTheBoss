using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
public class CharacterReader
{
    private string path = "/Resources/CharacterData.xml";

    public XmlDocument xmlDoc;
    public class CharacterData
    {
        public int attack;
        public int defense;
        public int life; 
        public int dexterity; 
        public int attackRange;
		//0,0
		public string describe;
    };

    public void ReadFile()
    {
        xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + path);
    }

    public CharacterData GetCharacterData(PawnType pawnType, string characterName, int level)
    {
        CharacterData data = new CharacterData();
        string xpath = characterName;
        if (pawnType == PawnType.Enemy)
            xpath = "/characters/enemies/" + xpath;
        else if (pawnType == PawnType.Monster)
            xpath = "/characters/monsters/" + xpath;
        
        XmlElement node = (XmlElement)xmlDoc.SelectSingleNode(xpath).ChildNodes[level-1];

        if(node == null)
        {
            Debug.Log("On CharacterReader: " + characterName + " not found");
            return null;
        }

        data.attack = int.Parse(node["attack"].InnerXml);
        data.defense = int.Parse(node["defense"].InnerXml);
        data.life = int.Parse(node["life"].InnerXml);
        data.dexterity = int.Parse(node["dexterity"].InnerXml);
        data.attackRange = int.Parse(node["attackRange"].InnerXml);
		//0,0
		data.describe=node["describe"].InnerXml.Trim();
        return data;
    }

}
