using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonElement;

[System.Serializable]
public class EnemyData
{
    public Property property;
    public string enemyID;
    public string enemyName;
    public int exp;

    public int maxHP;
    public int curHP;

    public int atk;
}
public class EnemyDatabase : MonoBehaviour
{
    public EnemyData[] enemies; // Populate this array in the Unity Editor

    public EnemyData GetEnemyDataById(string id)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.enemyID == id)
            {
                return enemy;
            }
        }
        return null; // Return null if no matching enemy is found
    }
}