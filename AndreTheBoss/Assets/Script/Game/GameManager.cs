using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CharacterReader characterReader;

    public List<Enemy> EnemyPawns;
    public List<Monster> MonsterPawns;

    public Enemy EnemyPrefab_thief;
    public Enemy EnemyPrefab_sword;
    public Enemy EnemyPrefab_magic;
    public Monster MonsterPrefab_boss;
    public Monster MonsterPrefab_zombie;
    public Monster MonsterPrefab_elf;
    public Monster MonsterPrefab_dwarf;
    public Monster MonsterPrefab_giant;

    public HexMap hexMap;

    public GameObject EnemyRoot;
    public GameObject MonsterRoot;
	
	//0,0
	public HealthBarManager healthbarmanager;


    public void OnEnable()
    {
        InitCharacters();
        InitCharacterReader();
        hexMap.GenerateCells();
        hexMap.HideCells();
        SpawnEnemies();
        SpawnMonsters();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitCharacters()
    {
        EnemyPawns = new List<Enemy>();
        MonsterPawns = new List<Monster>();

        EnemyRoot = new GameObject();
        EnemyRoot.transform.SetParent(transform);
        EnemyRoot.transform.position = Vector3.zero;

        MonsterRoot = new GameObject();
        MonsterRoot.transform.SetParent(transform);
        MonsterRoot.transform.position = Vector3.zero;
    }

    public void InitCharacterReader()
    {
        characterReader = new CharacterReader();
        characterReader.ReadFile();
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < 3; i++)
        {
            int ran = Random.Range(0, 3);
            EnemyType type = (EnemyType)ran;

            CharacterReader.CharacterData data = characterReader.GetCharacterData(PawnType.Enemy, type.ToString(), 1);
            
			Enemy newEnemy=new Enemy();
			switch(type)
			{
				case EnemyType.wanderingSwordman:
					newEnemy = Instantiate<Enemy>(EnemyPrefab_sword);
					break;
				case EnemyType.magicApprentice:
					newEnemy = Instantiate<Enemy>(EnemyPrefab_magic);
					break;
				case EnemyType.thief:
					newEnemy = Instantiate<Enemy>(EnemyPrefab_thief);
					break;
			}
			//0,0
            newEnemy.InitializeEnemy(type, type.ToString(), data.attack, data.defense, data.life, data.dexterity, data.attackRange);
            EnemyPawns.Add(newEnemy);

            HexCell cell = hexMap.GetRandomCellToSpawn();
            cell.pawn = newEnemy;
            newEnemy.currentCell = cell;
            newEnemy.transform.SetParent(EnemyRoot.transform);
            newEnemy.transform.position = cell.transform.position;
			//0,0
			newEnemy.Describe=data.describe;
			newEnemy.Healthbar=healthbarmanager.InitializeHealthBar(newEnemy);

            hexMap.RevealCellsFrom(cell);
        }

    }

    public void SpawnMonsters()
    {
        for (int i = 0; i < 3; i++)
        {
            int ran = Random.Range(0, 3);
            MonsterType type = (MonsterType)ran;

            CharacterReader.CharacterData data = characterReader.GetCharacterData(PawnType.Monster, type.ToString(), 1);
            Monster newMonster = new Monster();
			switch(type)
			{
				case MonsterType.boss:
					newMonster = Instantiate<Monster>(MonsterPrefab_boss);
					break;
				case MonsterType.zombie:
					newMonster = Instantiate<Monster>(MonsterPrefab_zombie);
					break;
				case MonsterType.elf:
					newMonster = Instantiate<Monster>(MonsterPrefab_elf);
					break;
				case MonsterType.dwarf:
					newMonster = Instantiate<Monster>(MonsterPrefab_dwarf);
					break;
				case MonsterType.giant:
					newMonster = Instantiate<Monster>(MonsterPrefab_giant);
					break;
			}
			//0,0
            newMonster.InitializeMonster(type, type.ToString(), data.attack, data.defense, data.life, data.dexterity, data.attackRange);
            MonsterPawns.Add(newMonster);

            HexCell cell = hexMap.GetRandomCellToSpawn();
            cell.pawn = newMonster;
            newMonster.currentCell = cell;
            newMonster.transform.SetParent(MonsterRoot.transform);
            newMonster.transform.position = cell.transform.position;
			//0,0
			newMonster.Describe=data.describe;
			newMonster.Healthbar=healthbarmanager.InitializeHealthBar(newMonster);

            hexMap.RevealCellsFrom(cell);
        }
    }
}
