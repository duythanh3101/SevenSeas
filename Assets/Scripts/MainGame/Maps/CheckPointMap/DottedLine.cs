using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLine : MonoBehaviour
{
    // Inspector fields
    public Sprite Dot;
    [Range(0.01f, 1f)]
    public float Size;
    [Range(0.1f, 2f)]
    public float Delta;

    [HideInInspector]
    public bool isDrawFinished = false;
    //Static Property with backing field
    private static DottedLine instance;
    public static DottedLine Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DottedLine>();
            return instance;
        }
    }

    //Utility fields
    List<GameObject> dots = new List<GameObject>();

    private void DestroyAllDots()
    {
        foreach (var dot in dots)
        {
            Destroy(dot);
        }
        dots.Clear();
    }

    GameObject GetOneDot()
    {
        var gameObject = new GameObject();
        gameObject.transform.localScale = Vector3.one * Size;
        gameObject.transform.parent = transform;

        var sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Dot;
        return gameObject;
    }

    public bool DrawDottedLine(Vector3 start, Vector3 end)
    {
        isDrawFinished = false;
        List<Vector2> positions = new List<Vector2>();

        int lineSteps = GetLineSteps(start, end);
        Vector3 lineStart = GetPoint(0f, start, end);
        for (int i = 1; i <= lineSteps; i++)
        {
            Vector3 lineEnd = GetPoint(i / (float)lineSteps, start, end);
            positions.Add(lineEnd);
            lineStart = lineEnd;
        }

        StartCoroutine("Render", positions);
        return isDrawFinished;
    }

    private int GetLineSteps(Vector3 start, Vector3 end)
    {
        var distance = (int)(end - start).magnitude;
        return distance != 0 ? distance * 5 : 2;
    }

    public Vector3 GetPoint(float t, Vector3 startPoint, Vector3 endPoint)
    {
        var middleCurvePosition = new Vector3((startPoint.x + endPoint.x)/2 + 0.5f,
                                            (startPoint.y + endPoint.y) / 2 + 0.5f,
                                            0f);
        return transform.TransformPoint(Bezier.GetPoint(startPoint,
           middleCurvePosition,
           endPoint, t));
    }

    private IEnumerator Render(List<Vector2> positionList)
    {
        if (DottedLineDrawer.Instance.checkPoints[DottedLineDrawer.Instance.CHECK_POINT_LEVEL].Parent.gameObject == null
            || DottedLineDrawer.Instance.checkPoints[DottedLineDrawer.Instance.CHECK_POINT_LEVEL + 1].Parent.gameObject == null)
            yield return null;
        DottedLineDrawer.Instance.checkPoints[DottedLineDrawer.Instance.CHECK_POINT_LEVEL].Parent.gameObject.SetActive(true);
        foreach (var position in positionList)
        {
            yield return new WaitForSeconds(0.5f);
            var g = GetOneDot();
            g.transform.position = position;
            dots.Add(g);
        }
        DottedLineDrawer.Instance.checkPoints[DottedLineDrawer.Instance.CHECK_POINT_LEVEL + 1].Parent.gameObject.SetActive(true);
        isDrawFinished = true;
        yield return null;
    }
}
