using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster: Pawn
{
    public MonsterType monsterType;
    public void InitializeMonster(MonsterType monsterType, string name,
        int attack, int defense, int life, int dexterity, int attackRange)
    {
        this.monsterType = monsterType;
        Name = monsterType.ToString();
        InitializePawn(PawnType.Monster, name, attack, defense, life, dexterity, attackRange);
    }

}
