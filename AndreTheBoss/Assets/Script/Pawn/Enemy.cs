using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Pawn
{
    public EnemyType enemyType;
    public void InitializeEnemy(EnemyType enemyType, string name,
    int attack, int defense, int life, int dexterity, int attackRange)
    {
        this.enemyType = enemyType;
        Name = enemyType.ToString();
        InitializePawn(PawnType.Enemy, name, attack, defense, life, dexterity, attackRange);
    }

    
}
