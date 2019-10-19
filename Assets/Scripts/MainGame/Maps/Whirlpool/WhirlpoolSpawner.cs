using MainGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WhirlpoolSpawner : MonoBehaviour
{
    #region Public Properties
    public Transform whirlpoolParent;
    public MapGenerator mapGenerator;

    #endregion Public Properties


    #region Mono Behaviour

    // Start is called before the first frame update
    void Start()
    {
        whirlpoolParent = GameObject.Find("WhirlpoolParent").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion Mono Behaviour


    #region Public Methods

    /// <summary>
    /// Layout whirlpools in 4 corners of the map.
    /// </summary>
    public void Spawn(int level)
    {
        Transform tileChoice = mapGenerator.currentMap.whirlpoolPrefabs[Random.Range(0, mapGenerator.currentMap.whirlpoolPrefabs.Count)];

        // Bottom Left corner
        Vector3 position = new Vector3(-mapGenerator.currentMap.mapSize.x / 2 + mapGenerator.currentMap.tileSize / 2, -mapGenerator.currentMap.mapSize.x / 2 + mapGenerator.currentMap.tileSize / 2);
        mapGenerator.LayoutObjectAtPosition(tileChoice, position, whirlpoolParent);

        // Above Left corner
        position = new Vector3(-mapGenerator.currentMap.mapSize.x / 2 + mapGenerator.currentMap.tileSize / 2, mapGenerator.currentMap.mapSize.x / 2 - mapGenerator.currentMap.tileSize / 2);
        mapGenerator.LayoutObjectAtPosition(tileChoice, position, whirlpoolParent);

        // Above right corner
        position = new Vector3(mapGenerator.currentMap.mapSize.x / 2 - mapGenerator.currentMap.tileSize / 2, mapGenerator.currentMap.mapSize.x / 2 - mapGenerator.currentMap.tileSize / 2);
        mapGenerator.LayoutObjectAtPosition(tileChoice, position, whirlpoolParent);

        // Bottom right corner
        position = new Vector3(mapGenerator.currentMap.mapSize.x / 2 - mapGenerator.currentMap.tileSize / 2, -mapGenerator.currentMap.mapSize.x / 2 + mapGenerator.currentMap.tileSize / 2);
        mapGenerator.LayoutObjectAtPosition(tileChoice, position, whirlpoolParent);

    }
    #endregion Public Methods

}
