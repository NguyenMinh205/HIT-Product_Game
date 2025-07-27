using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    public List<DataEnemy> dataEnemy = new List<DataEnemy>();
    public List<DataEnemy> dataBoss = new List<DataEnemy>();

}

[System.Serializable]
public class DataEnemy
{
    public string idEnemy;
    public string nameEnemy;
    public int hpEnemy;
    public List<int> damageEnemy;
    public int armorIncreased;

    public Sprite spriteEnemyIdle;

    public RuntimeAnimatorController controller;

    public ItemBase itemEffect;

    public List<ProcedureActionEnemy> actions = new List<ProcedureActionEnemy>();
}
[System.Serializable]
public class ProcedureActionEnemy
{
    public List<TypeEnemyAction> actionEnemy = new List<TypeEnemyAction>();
}
public enum TypeEnemyAction
{
    Attack,
    Defend,
    Heal,
    Effect,
    Buff,
    DeBuff,
    Drop
}
