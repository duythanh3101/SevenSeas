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

        [SerializeField]
        private List<string> objectTag;

        //Properties
        public Vector2 TileSize { get; private set; }
        public Vector2 BackgroundSize { get; private set; }

        private Dictionary<GameObject, Vector2> staticObjectDicts = new Dictionary<GameObject, Vector2>();
        private Dictionary<GameObject, Vector2> dynamicObjectDicts = new Dictionary<GameObject, Vector2>();

        private List<Vector2> possiblePositions = new List<Vector2>(); // The available position from the first creating level

        //Cache value
        private SpriteRenderer backgroundSR;

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
            InitObjectDictionary();
            InitStaticPositions();

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

        void InitObjectDictionary()
        {
            int count = objectTag.Count;

            for (int i = 0; i < count; i++)
            {
                var objectsFound = GameObject.FindGameObjectsWithTag(objectTag[i]);

                if (objectsFound.Length > 0)
                {
                    if (IsStaticObject(objectTag[i]))
                    {
                        foreach (var obj in objectsFound)
                        {
                            //Debug.Log((Vector2)obj.transform.position);
                            staticObjectDicts.Add(obj, (Vector2)obj.transform.position);
                        }
                    }
                    else
                    {
                        foreach (var obj in objectsFound)
                        {
                            dynamicObjectDicts.Add(obj, (Vector2)obj.transform.position);
                        }
                    }
                }
            }
        }

        void InitStaticPositions()
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

                    if (IsExists(pos, staticObjectDicts) || IsExists(pos, dynamicObjectDicts))
                        continue;
                    possiblePositions.Add(pos);

                    //if (!staticObjectDicts.ContainsValue(pos) && !dynamicObjectDicts.ContainsValue(pos))
                    //{
                        
                    //    possiblePositions.Add(pos);
                    //}
                }
            }
        }

        bool IsExists(Vector2 pos,Dictionary<GameObject, Vector2> dic)
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
            for (int i =0; i < possiblePos.Count; i++)
            {
                if (possiblePos[i] == pos)
                    possiblePos.RemoveAt(i);
            }
        }

        public void LayoutUnitAtRandomPosition(GameObject unit, bool recycle)
        {
            Vector2 randomPos = possiblePositions[Random.Range(0, possiblePositions.Count)];
            if (recycle) //reuse the game object, just update the position
            {
                unit.transform.position = randomPos;
                unit.SetActive(true);

                //Make sure to update all the position
                UpdatePossiblePosition(unit, randomPos,recycle);

            }
            else // Create the new unit instance
            {
                Instantiate(unit, randomPos, Quaternion.identity);

                //Make sure to update all the position
                UpdatePossiblePosition(unit, randomPos, recycle);
            }
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
                    staticObjectDicts.Add(obj, newPos);
                    RemovePossiblePosition(possiblePositions, newPos);
                    //Debug.Log("remove: " + newPos);
                }
                else
                {
                    dynamicObjectDicts.Add(obj, newPos);
                    RemovePossiblePosition(possiblePositions, newPos);
                }

                 //Debug.Log("Remove " + newPos + "Obj name:  " + obj.name + "status: " + status);
            }
        }


    }
}

