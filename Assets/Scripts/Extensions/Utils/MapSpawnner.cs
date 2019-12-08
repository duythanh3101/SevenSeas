using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawnner : MonoBehaviour
{
    [Header("Object configurations")]
    [SerializeField]
    private GameObject whirlpoolPrefab;
    [SerializeField]
    private GameObject islandPrefab;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private int islandCount;
    [SerializeField]
    private int enemyCount;

    public int currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        SetupLevel(currentLevel);
    }

    public void SetupLevel(int level)
    {
        SpawnWhirlpools();
        SpawnIslands();
        SpawnPlayer();
        SpawnEnemies();
        
    }


    void SpawnWhirlpools()
    {
        
    }


    void SpawnIslands()
    {

    }

    void SpawnEnemies()
    {

    }

    void SpawnPlayer()
    {

    }

}
