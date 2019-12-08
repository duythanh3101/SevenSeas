using Assets.Scripts.Extensions.Utils;
using BaseSystems.Observer;
using BaseSystems.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class MapGenerator : Singleton<MapGenerator>
    {
        #region Public Properties
        public Map currentMap;

        public static int level = 1;

        public WhirlpoolSpawner whirlpoolSpawner;
        public IslandSpawner islandSpawner;
        public EdgeSpawner edgeSpawner;
        public Transform player;
        #endregion Public Properties


        #region Private Properties

        private static List<Vector3> allPositions = new List<Vector3>();

        #endregion Private Properties


        #region Mono Behaviour
        protected override void Awake()
        {
            SetupScene(level);
        }

        #endregion Mono Behaviour


        #region Private Methods
        /// <summary>
        /// Initialise map list
        /// </summary>
        private void InitialiseList()
        {
            allPositions.Clear();
            int halfColCount = (int)currentMap.mapSize.x / 2 + 1;
            int halfRowCount = (int)currentMap.mapSize.y / 2 + 1;

            for (int col = -halfColCount; col < halfColCount; col++)
            {
                for (int row = -halfRowCount; row < halfRowCount; row++)
                {
                    Vector3 pos = new Vector3(col + currentMap.tileSize / 2, row + currentMap.tileSize / 2, 0f);
                    allPositions.Add(pos);
                }
            }
        }

        /// <summary>
        /// RandomPosition returns a random position from our list gridPositions.
        /// </summary>
        /// <returns></returns>
        private Vector3 RandomPosition()
        {
            int randomIndex = Random.Range(0, allPositions.Count);

            Vector3 randomPosition = allPositions[randomIndex];

            allPositions.RemoveAt(randomIndex);

            return randomPosition;
        }

        #endregion Private Methods


        #region Public Methods

        /// <summary>
        /// LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create. 
        /// </summary>
        /// <param name="tileArray"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public void LayoutObjectAtRandom(TransformList transforms, int minimum, int maximum, Transform parent = null)
        {
            int objectCount = Random.Range(minimum, maximum + 1);

            for (int i = 0; i < objectCount; i++)
            {
                Vector3 randomPosition = RandomPosition();

                Transform tileChoice = transforms[Random.Range(0, transforms.Length)];

                LayoutObjectAtPosition(tileChoice, randomPosition, parent);
            }
        }

        /// <summary>
        /// LayoutObjectAtRandom accepts an array of game objects random
        /// </summary>
        /// <param name="tileArray"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public void LayoutObjectAtRandom(Transform tileChoice, Transform parent = null)
        {
            Vector3 randomPosition = RandomPosition();
            if (tileChoice != null)
            {
                tileChoice.position = randomPosition;
            }
        }

        /// <summary>
        /// Layout object at position
        /// </summary>
        /// <param name="tileChoice"></param>
        /// <param name="position"></param>
        /// <param name="parent"></param>
        public void LayoutObjectAtPosition(Transform tileChoice, Vector3 position, Transform parent = null)
        {
            allPositions.Remove(position);
            Transform toInstance = Instantiate(tileChoice, position, Quaternion.identity);
            //this.PostEvent(ObserverEventID.OnSpawnerObject, toInstance);

            if (parent != null)
            {
                toInstance.SetParent(parent);
            }
        }

        /// <summary>
        /// SetupScene initializes our level and calls the previous functions to lay out the game board
        /// </summary>
        /// <param name="level"></param>
        public void SetupScene(int level)
        {
            //Reset our list of gridpositions.
            InitialiseList();

            // Spawn edge
            //edgeSpawner.Spawn();

            //Layout player 
            LayoutObjectAtPosition(player, new Vector3(-0.5f, 0.5f, 0f));

            whirlpoolSpawner.Spawn(level);

            islandSpawner.Spawn(level);
        }

        #endregion Public Methods
    }
}