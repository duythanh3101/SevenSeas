using BaseSystems.Observer;
using BaseSystems.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLineDrawer : Singleton<DottedLineDrawer>
{
    public List<CheckPoint> checkPoints = new List<CheckPoint>();
    public int CHECK_POINT_LEVEL = 0;

    // Start is called before the first frame update
    void Start()
    {
        DottedLine.Instance.DrawDottedLine(checkPoints[CHECK_POINT_LEVEL].StartPoint.transform.position, checkPoints[CHECK_POINT_LEVEL + 1].EndPoint.transform.position);
        this.PostEvent(ObserverEventID.OnCheckPointMapStarted);
    }


    // Update is called once per frame
    void Update()
    {
        if (DottedLine.Instance.isDrawFinished && CHECK_POINT_LEVEL < 5)
        {
            CHECK_POINT_LEVEL++;
            var start = checkPoints[CHECK_POINT_LEVEL].StartPoint.transform.position;
            var end = checkPoints[CHECK_POINT_LEVEL + 1].EndPoint.transform.position;

            Debug.Log(start + "  - -- - " + end + " -- " + CHECK_POINT_LEVEL);
            DottedLine.Instance.DrawDottedLine(start, end);
        }
    }
}
