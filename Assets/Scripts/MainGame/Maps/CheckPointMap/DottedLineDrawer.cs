using BaseSystems.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLineDrawer : Singleton<DottedLineDrawer>
{
    public List<CheckPoint> checkPoints = new List<CheckPoint>();
    public int CHECK_POINT_LEVEL = 0;
    bool isfinished = false;

    // Start is called before the first frame update
    void Start()
    {
        isfinished = DottedLine.Instance.DrawDottedLine(checkPoints[CHECK_POINT_LEVEL].StartPoint.transform.position, checkPoints[CHECK_POINT_LEVEL + 1].EndPoint.transform.position);
        //CHECK_POINT_LEVEL++;
    }


    // Update is called once per frame
    void Update()
    {
        if (isfinished)
        {
            isfinished = false;
            SceneGameManager.Instance.LoadScene(CommonConstants.SceneName.TreasureMapScene);
        }
        //if (DottedLine.Instance.isDrawFinished && CHECK_POINT_LEVEL < 5)
        //{
        //    CHECK_POINT_LEVEL++;
        //    var start = checkPoints[CHECK_POINT_LEVEL].StartPoint.transform.position;
        //    var end = checkPoints[CHECK_POINT_LEVEL + 1].EndPoint.transform.position;

        //    DottedLine.Instance.DrawDottedLine(start, end);
        //}
    }

    public void Draw()
    {

    }
}
