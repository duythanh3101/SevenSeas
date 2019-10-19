using UnityEngine;
using Random = UnityEngine.Random;

namespace MainGame
{
    public class EdgeSpawner : MonoBehaviour
    {
        #region Public Properties

        public Transform edgeParent;
        public MapGenerator mapGenerator;

        #endregion Public Methods


        #region Mono Behaviour
        // Start is called before the first frame update
        void Start()
        {
            edgeParent = GameObject.Find("EdgeParent").transform;
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion Mono Behaviour


        #region Public Methods
        public void Spawn()
        {
            Transform tileChoice = mapGenerator.currentMap.edgePrefabs[Random.Range(0, mapGenerator.currentMap.edgePrefabs.Count)];

            int halfColCount = (int)mapGenerator.currentMap.mapSize.x / 2 + 1;
            int halfRowCount = (int)mapGenerator.currentMap.mapSize.y / 2 + 1;

            int colSign = 1;
            int rowSign = 1;
            for (int col = -halfColCount; col < halfColCount + 1; col++)
            {
                for (int row = -halfRowCount; row < halfRowCount + 1; row++)
                {
                    if (col == halfColCount|| row == halfRowCount || col == -halfColCount || row == -halfRowCount)
                    {
                        colSign = GetOppositeSign(col);
                        rowSign = GetOppositeSign(row);
                      
                        Vector3 pos = new Vector3(col + colSign * mapGenerator.currentMap.tileSize / 2, row + rowSign * mapGenerator.currentMap.tileSize / 2, 0f);
                        mapGenerator.LayoutObjectAtPosition(tileChoice, pos, edgeParent);
                    }

                }
            }
        }


        #endregion Public Methods


        #region Private Methods

        private int GetOppositeSign(int number)
        {
            int sign = 1;
            if (number > 0)
            {
                sign = -1;
            }
            else
            {
                sign = 1;
            }
            return sign;
        }

        #endregion Private Methods

    }
}