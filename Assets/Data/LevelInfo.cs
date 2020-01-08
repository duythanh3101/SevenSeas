using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelInfo
{
    public int ID {get;set;}
    public int levelInCheckPoint {get;set;}
    public int checkPoint {get;set;}
    public int islandCount {get;set;}
    public int normalEnemyCount {get;set;}
    public int advanceEnemyCount {get;set;}
    public int firingEnemyCount {get;set;}

    public LevelInfo(int id, int level, int cp, int island, int normalEnemy, int advanceEnemy, int firingEnemy)
    {
        ID = id;
        levelInCheckPoint = level;
        checkPoint = cp;
        islandCount = island;
        normalEnemyCount = normalEnemy;
        advanceEnemyCount = advanceEnemy;
        firingEnemyCount = firingEnemy;
    }

    public static LevelInfo Parse(string data)
    {
        string[] col = data.Split(';');
        return new LevelInfo(int.Parse(col[0]),int.Parse(col[1]),int.Parse(col[2]),int.Parse(col[3]),int.Parse(col[4]),int.Parse(col[5]),int.Parse(col[6]));

    }
}
