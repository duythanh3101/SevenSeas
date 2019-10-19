using Extension.Attributes;
using Extension.ExtraTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    [Serializable]
    public class MapList : ReorderableArray<Map> { }

    [Serializable]
    public class TransformList : ReorderableArray<Transform> { }

    [Serializable]
    public class Map
    {
        public Vector2 mapSize = new Vector2(CommonConstants.NUMBER_OF_ROWS, CommonConstants.NUMBER_OF_COLUMNS);

        [Positive]
        public float tileSize = 1;

        [Reorderable]
        public TransformList obstaclePrefabs;

        [Reorderable]
        public TransformList whirlpoolPrefabs;

        [Reorderable]
        public TransformList edgePrefabs;

        [Reorderable]
        public TransformList enemyPrefabs;

        public Map(Vector2 mapSize, float tileSize = 1f)
        {
            this.mapSize = mapSize;
            this.tileSize = tileSize;
        }

        public Vector2 mapCentre
        {
            get
            {
                return new Vector2(mapSize.x / 2, mapSize.y / 2);
            }
        }
    }
}

