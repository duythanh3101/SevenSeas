using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Extensions.Utils;

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

    private Dictionary<Vector2, GameObject> objectInfoDictionary = new Dictionary<Vector2, GameObject>();
    private List<Vector2>  staticAvailablePositions = new List<Vector2>(); // The available position from the first creating level

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
    }

    void Start()
    {
        InitValues();
        InitObjectDictionary();
        InitStaticPositions();

    }
    void InitValues()
    {
        BackgroundSize = UtilMapHelpers.CalculateBackgroundSize(backgroundSR, backgroundMap.transform.lossyScale);
        TileSize = UtilMapHelpers.CalculateCellSize(BackgroundSize);

        centerNumber = (int)Mathf.Sqrt(CommonConstants.NUMBER_OF_CELLS) / 2;
        centerPosition = backgroundMap.transform.position;
    }

    void InitObjectDictionary()
    {
        int count = objectTag.Count;

        for  (int i = 0; i < count; i++)
        {
            var objectsFound = GameObject.FindGameObjectsWithTag(objectTag[i]);
            
            if (objectsFound.Length > 0)
            {
                foreach (var obj in objectsFound)
                {
                    objectInfoDictionary.Add(obj.transform.position, obj);
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
              (centerPosition.x + UtilMapHelpers.GetHorizontalSign(icol,centerNumber) * (TileSize.x * (Mathf.Abs(centerNumber - icol)))) + TileSize.x / 2,
              (centerPosition.y + UtilMapHelpers.GetVerticalSign(irow,centerNumber) * (TileSize.y * Mathf.Abs((centerNumber - irow)))) - TileSize.y / 2);

                if (!objectInfoDictionary.ContainsKey(pos))
                {
                    staticAvailablePositions.Add(pos);
                }
            }
        }
    }

    public void LayoutUnitAtRandomPosition(GameObject unit, bool recycle)
    {
        Vector2 randomPos = staticAvailablePositions[Random.Range(0, staticAvailablePositions.Count)];
        if (recycle) //reuse the game object, just update the position
        {
            unit.transform.position = randomPos;
            unit.SetActive(true);

        }
        else // Create the new unit instance
            Instantiate(unit, randomPos, Quaternion.identity);

        //Make sure to remove this position
        //staticAvailablePositions.Remove(randomPos);
    }

}
