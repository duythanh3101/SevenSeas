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


    //Cache value
    private SpriteRenderer backgroundSR;


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

    // Start is called before the first frame update
    void Start()
    {
        InitValues();
        InitObjectDictionary();

    }
    void InitValues()
    {
        BackgroundSize = UtilMapHelpers.CalculateBackgroundSize(backgroundSR, backgroundMap.transform.lossyScale);
        TileSize = UtilMapHelpers.CalculateCellSize(BackgroundSize);
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




    public void LayoutUnitAtRandom()
    {

    }

}
