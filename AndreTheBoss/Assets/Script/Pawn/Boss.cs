using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster
{
    public void InitializeBoss(MonsterType monsterType, string name,
    int attack, int defense, int life, int dexterity, int attackRange)
    {
        InitializeMonster(MonsterType.boss, name, attack, defense, life, dexterity, attack);
    }
}
