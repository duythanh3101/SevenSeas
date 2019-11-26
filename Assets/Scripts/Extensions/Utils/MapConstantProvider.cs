using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Extensions.Utils;

public class MapConstantProvider : MonoBehaviour
{
    public static MapConstantProvider Instance = null;

    [SerializeField]
    private GameObject backgroundMap;

    //Properties
    public Vector2 TileSize { get; private set; }
    public Vector2 BackgroundSize { get; private set; }


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
        BackgroundSize = UtilMapHelpers.CalculateBackgroundSize(backgroundSR, backgroundMap.transform.lossyScale);
        TileSize = UtilMapHelpers.CalculateCellSize(BackgroundSize);
    }

}
