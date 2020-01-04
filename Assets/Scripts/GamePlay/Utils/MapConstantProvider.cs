using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Extensions.Utils;


namespace SevenSeas
{
    public class MapConstantProvider : MonoBehaviour
    {
        public static MapConstantProvider Instance = null;
        [SerializeField]
        private GameObject backgroundMap;

        [Header("Level Initialization")]
        [Header("Whirlpool")]
        [SerializeField]
        private GameObject whirlpoolPrefab;
        [SerializeField]
        private Transform whirlpoolParent;
        [Header("Island")]
        [SerializeField]
        private GameObject islandPrefab;
        [SerializeField]
        private int islandCount = 5;
        [SerializeField]
        private Transform islandParent;
        [Header("Enemies")]
        [SerializeField]
        private GameObject[] enemiesPrefab;
        [SerializeField]
        private int normalEnemyCount;
        [SerializeField]
        private int advanceEnemyCount;
        
        [SerializeField]
        private Transform enemyParent;
        [Header("Player")]
        [SerializeField]
        private GameObject playerPrefab;
      
       

        public int currentLevel;

        //Properties
        public Vector2 TileSize { get; private set; }
        public Vector2 BackgroundSize { get; private set; }

        public Vector2 CenterPosition { get { return centerPosition; } }
        public Vector2 PlayerPos { get { return dynamicObjectDicts[player]; } }

        public Dictionary<GameObject, Vector2> staticObjectDicts = new Dictionary<GameObject, Vector2>();
        public Dictionary<GameObject, Vector2> dynamicObjectDicts = new Dictionary<GameObject, Vector2>();

        private List<Vector2> possiblePositions = new List<Vector2>(); // The available position from the first creating level

        //Cache value
        private SpriteRenderer backgroundSR;
        private GameObject player;


        private Vector2 centerPosition;

        private int centerNumber;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
                DestroyImmediate(gameObject);

            backgroundSR = backgroundMap.GetComponent<SpriteRenderer>();
            BoatController.OnBoatMovedPosition += BoatController_OnBoatMovedPosition;
            BoatController.OnSpawnSkull += BoatController_OnSpawnSkull;
        }

        void OnDestroy()
        {
            BoatController.OnBoatMovedPosition -= BoatController_OnBoatMovedPosition;
            BoatController.OnSpawnSkull -= BoatController_OnSpawnSkull;
        }

        private void BoatController_OnBoatMovedPosition(GameObject moveObj, Vector2 newPos)
        {
            //This only change the position of moved obj, not instantiate new one
            UpdatePossiblePosition(moveObj, newPos, true);
        }

        private void BoatController_OnSpawnSkull(GameObject staticObj, Vector2 newPos)
        {
            //Instantiate a new one
            UpdatePossiblePosition(staticObj, newPos, false);
        }

        void Start()
        {
            InitValues();
            InitPossiblePositions();
            SetupLevel(currentLevel);
        }

        [Header("Debug")]
        public bool drawStaticPosition;
        public bool drawPossiblePosition;
        public bool drawDynamicPosition;

        void OnDrawGizmos()
        {
            if (drawPossiblePosition)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < possiblePositions.Count; i++)
                {
                    Gizmos.DrawCube(possiblePositions[i], Vector3.one * 0.5f);
                }
            }

            if (drawStaticPosition)
            {
                Gizmos.color = Color.red;
                foreach (var obj in staticObjectDicts)
                {
                    Gizmos.DrawCube(obj.Value, Vector3.one * 0.4f);
                }
            }

            if (drawDynamicPosition)
            {
                Gizmos.color = Color.yellow;
                foreach (var obj in dynamicObjectDicts)
                {
                    Gizmos.DrawCube(obj.Value, Vector3.one * 0.4f);
                }
            }
        }
     
        void SetupLevel(int level)
        {
            //Precalculate the number of islands, whirlpools, enemies base on level

            SpawnWhirlpools();
            SpawnIslands();
            SpawnEnemies();
            SpawnPlayer();
        }

        void SpawnWhirlpools()
        {
            //Top left
            Vector2 pos = new Vector2(centerPosition.x - BackgroundSize.x /2  + TileSize.x / 2, centerPosition.y + BackgroundSize.y / 2 - TileSize.y / 2);
            LayoutUnitAtSpecific(whirlpoolPrefab, pos,whirlpoolParent);

            //Top right
            pos = new Vector2(centerPosition.x + BackgroundSize.x / 2 - TileSize.x / 2, centerPosition.y + BackgroundSize.y / 2 - TileSize.y / 2);
            LayoutUnitAtSpecific(whirlpoolPrefab, pos, whirlpoolParent);

            //Bottom left
            pos = new Vector2(centerPosition.x - BackgroundSize.x / 2 + TileSize.x / 2, centerPosition.y - BackgroundSize.y / 2 + TileSize.y / 2);
            LayoutUnitAtSpecific(whirlpoolPrefab, pos, whirlpoolParent);
             
            //Bottom right
            pos = new Vector2(centerPosition.x + BackgroundSize.x / 2 - TileSize.x / 2, centerPosition.y - BackgroundSize.y / 2 + TileSize.y / 2);
            LayoutUnitAtSpecific(whirlpoolPrefab, pos, whirlpoolParent);

        }

        void SpawnIslands()
        {
            for (int i =0; i < islandCount;i++)
            {
                LayoutUnitAtRandomPosition(islandPrefab, false,islandParent);
            }
        }

        void SpawnEnemies()
        {
            GameObject normalEnemyPrefab = enemiesPrefab[0];
            for (int i = 0; i < normalEnemyCount;i++ )
            {
                LayoutUnitAtRandomPosition(normalEnemyPrefab, false, enemyParent);
            }

            GameObject advanceEnemyPrefab = enemiesPrefab[1];
            for (int i = 0; i < advanceEnemyCount;i++ )
            {
                LayoutUnitAtRandomPosition(advanceEnemyPrefab, false, enemyParent);
            }
        }

        void SpawnPlayer()
        {
            LayoutUnitAtRandomPosition(playerPrefab, false);
            player = GameObject.FindGameObjectWithTag(playerPrefab.tag);
        }

        void InitValues()
        {
            BackgroundSize = UtilMapHelpers.CalculateBackgroundSize(backgroundSR, backgroundMap.transform.lossyScale);
            TileSize = UtilMapHelpers.CalculateCellSize(BackgroundSize);

            centerNumber = (int)Mathf.Sqrt(CommonConstants.NUMBER_OF_CELLS) / 2;
            centerPosition = backgroundMap.transform.position;
           
        }

        bool IsStaticObject(string tag)
        {
            return (tag == "Whirlpool" || tag == "Obstacle");
        }

        void InitPossiblePositions()
        {

            int row = (int)Mathf.Sqrt(CommonConstants.NUMBER_OF_CELLS);
            int col = row;

            for (int irow = 0; irow < row; irow++)
            {
                for (int icol = 0; icol < col; icol++)
                {

                    Vector2 pos = new Vector2(
                  (centerPosition.x + UtilMapHelpers.GetHorizontalSign(icol, centerNumber) * (TileSize.x * Mathf.Abs(centerNumber - icol))) + TileSize.x / 2,
                  (centerPosition.y + UtilMapHelpers.GetVerticalSign(irow, centerNumber) * (TileSize.y * Mathf.Abs(centerNumber - irow))) - TileSize.y / 2);
                    possiblePositions.Add(pos);
                }
            }
        }

        bool IsExists(Vector2 pos, List<Vector2> list)
        {
            foreach (var obj in list)
            {
                if (pos == obj)
                    return true;
            }
            return false;
        }

        bool IsExists(Vector2 pos, Dictionary<GameObject, Vector2> dic)
        {
            foreach (var obj in dic)
            {
                if (pos == obj.Value)
                    return true;
            }

            return false;
        }

        void RemovePossiblePosition(List<Vector2> possiblePos, Vector2 pos)
        {
            for (int i = 0; i < possiblePos.Count; i++)
            {
                if (possiblePos[i] == pos)
                    possiblePos.RemoveAt(i);
            }
        }

        public void LayoutUnitAtSpecific(GameObject unit,Vector2 pos, Transform parent = null)
        {
            if (!IsExists(pos,possiblePositions))
            {
                Debug.Log("Cant spawn: " + unit.name + "at: " + pos);
                return;
            }

            bool isStatic = IsStaticObject(unit.tag);
            var ins = Instantiate(unit, pos, Quaternion.identity,parent);
            if (isStatic)
            {
                staticObjectDicts.Add(ins, pos);
            }
            else
            {
                dynamicObjectDicts.Add(ins, pos);
            }
            RemovePossiblePosition(possiblePositions, pos);
        }

        public void SpawnUnitOnDestroyedObject(GameObject unit, Vector2 pos, GameObject destroyedObject)
        {
            bool isStatic = IsStaticObject(unit.tag);

            //Remove the destroyed object from the dictionary
            if (staticObjectDicts.ContainsKey(destroyedObject))
            {
                staticObjectDicts.Remove(destroyedObject);
            }
            else if (dynamicObjectDicts.ContainsKey(destroyedObject))
            {

                //Make sure it's not player, because we only disable the player, not actually destroying it
                if (!destroyedObject.CompareTag("PlayerShip"))
                {
                    dynamicObjectDicts.Remove(destroyedObject);
                }

                
            }

            //Check to spawn the unit on destroyed object
            if (isStatic)
            {
                if (IsExists(pos, staticObjectDicts))
                    return;// To prevent the two object spawn on the same position
                //Debug.Log("Spawn skull");
                var ins = Instantiate(unit, pos, Quaternion.identity);
                staticObjectDicts.Add(ins, pos);
            }
            else
            {
                if (IsExists(pos, dynamicObjectDicts))
                    return;// To prevent the two object spawn on the same position
                var ins = Instantiate(unit, pos, Quaternion.identity);
                dynamicObjectDicts.Add(ins, pos);
            }
           
            RemovePossiblePosition(possiblePositions, pos);
        }

        public bool ContainsInPossiblePositionIncludePlayer(Vector2 valuePos)
        {
            foreach (var pos in possiblePositions)
            {
                if (valuePos == pos)
                    return true;
            }

            if (valuePos == PlayerPos)
                return true;
            
            return false;
        }

        private bool ContainsPosInDictionaryExceptPlayer(Vector2 valuePos, Dictionary<GameObject, Vector2> objectDictionary)
        {
            //Debug.Log("Player pos: " + dynamicObjectDicts[player]);
            foreach (var obj in objectDictionary)
            {
                if (obj.Key != player)
                {
                    if (valuePos == obj.Value)
                    {
                        //Debug.Log(obj.Key.name + " Place on this: " + obj.Value);
                        return true;
                    }
                }  
            }
            return false;
        }

       public bool ContainsPosInUnitDictionary(Vector2 valuePos)
        {
           

            return (ContainsPosInDictionaryExceptPlayer(valuePos, staticObjectDicts) || 
                ContainsPosInDictionaryExceptPlayer(valuePos, dynamicObjectDicts));
        }

        public void LayoutUnitAtRandomPosition(GameObject unit, bool recycle, Transform parent = null)
        {
            Vector2 randomPos = possiblePositions[Random.Range(0, possiblePositions.Count)];
            if (recycle) //reuse the game object, just update the position
            {
                unit.transform.position = randomPos;
                unit.SetActive(true);

                //Make sure to update all the position
                UpdatePossiblePosition(unit, randomPos, recycle);

            }
            else // Create the new unit instance
            {
                var unitIns =  Instantiate(unit, randomPos, Quaternion.identity,parent);

                //Make sure to update all the position
                UpdatePossiblePosition(unitIns, randomPos, recycle);
            }
        }

        public void SetRespawningPosition(GameObject player)
        {
            Vector2 randomPos = possiblePositions[Random.Range(0, possiblePositions.Count)];
            player.transform.position = randomPos;
            player.SetActive(true);
            dynamicObjectDicts[player] = randomPos;
        }

        void UpdatePossiblePosition(GameObject obj, Vector2 newPos, bool recycle)
        {
            bool isStatic = IsStaticObject(obj.tag);
            //Check the has-object-pos dictionary
            if (recycle) // reuse
            {
                //In the possible position, add the current pos stored in the object dict and remove the new pos
                if (isStatic)
                {
                    //Debug.Log("Add " + staticObjectDicts[obj] + " Remove " + newPos + " Obj name:" + obj.name);
                   
                    
                    possiblePositions.Add(staticObjectDicts[obj]);
                    staticObjectDicts[obj] = newPos;
                    RemovePossiblePosition(possiblePositions, newPos);


                }
                else
                {
                    //Debug.Log("Add " + dynamicObjectDicts[obj] + " Remove " + newPos + " Obj name:" + obj.name);
                    possiblePositions.Add(dynamicObjectDicts[obj]);
                    dynamicObjectDicts[obj] = newPos;
                    RemovePossiblePosition(possiblePositions, newPos);
                }
            }
            else //Instantiate new object
            {
                //In the possible positions list, just remove the current pos 
                if (isStatic)
                {
                    if (IsExists(newPos, staticObjectDicts))
                        return;
                    staticObjectDicts.Add(obj, newPos);
                    RemovePossiblePosition(possiblePositions, newPos);
                    //Debug.Log("remove: " + newPos);
                }
                else
                {
                    if (IsExists(newPos, dynamicObjectDicts))
                        return;
                    dynamicObjectDicts.Add(obj, newPos);
                    RemovePossiblePosition(possiblePositions, newPos);
                }

                //Debug.Log("Remove " + newPos + "Obj name:  " + obj.name + "status: " + status);
            }
        }


    }
}

