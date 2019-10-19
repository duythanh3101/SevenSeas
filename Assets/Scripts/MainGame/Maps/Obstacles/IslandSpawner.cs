using MainGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawner : MonoBehaviour
{
    #region Public Properties

    public Transform islandParent;
    public MapGenerator mapGenerator;

    #endregion Public Methods


    #region Mono Behaviour
    // Start is called before the first frame update
    void Start()
    {
        islandParent = GameObject.Find("IslandParent").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion Mono Behaviour


    #region Public Methods
    /// <summary>
    /// Layout whirlpools in 4 corners of the map
    /// </summary>
    /// <param name="level"></param>
    public void Spawn(int level)
    {
        //TO DO ...
        int minimum = 4;
        int maximum = 4;

        mapGenerator.LayoutObjectAtRandom(mapGenerator.currentMap.obstaclePrefabs, minimum, maximum, islandParent);
    }
    #endregion Public Methods

}
