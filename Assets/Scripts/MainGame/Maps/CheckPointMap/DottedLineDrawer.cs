using BaseSystems.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLineDrawer : MonoBehaviour
{
    public List<CheckPoint> checkPoints = new List<CheckPoint>();
    public int CHECK_POINT_LEVEL = 0;

    private bool isfinished = false;

    private static DottedLineDrawer instance;
    public static DottedLineDrawer Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DottedLineDrawer>();
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CHECK_POINT_LEVEL = PlayerPrefs.GetInt("CHECK_POINT_LEVEL", 0);
        Debug.Log(CHECK_POINT_LEVEL);
        isfinished = false;
        if (CHECK_POINT_LEVEL < 5)
        {
            DottedLine.Instance.SetActiveAllExisting(CHECK_POINT_LEVEL);
            DottedLine.Instance.DrawDottedLine(checkPoints[CHECK_POINT_LEVEL].StartPoint.transform.position,
                checkPoints[CHECK_POINT_LEVEL + 1].EndPoint.transform.position,
                CHECK_POINT_LEVEL,
                SetDrawFinishedScene);
        }
    }

    private void SetDrawFinishedScene()
    {
        isfinished = true;
    }

    // Update is called once per frame
    void Update()
    {
        //isfinished = DottedLine.Instance.isDrawFinished;
        if (isfinished)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isfinished = false;
                CHECK_POINT_LEVEL++;
                PlayerPrefs.SetInt("CHECK_POINT_LEVEL", CHECK_POINT_LEVEL);
                SceneGameManager.Instance.LoadScene(CommonConstants.SceneName.TreasureMapScene);
            }
        }
    }
}
