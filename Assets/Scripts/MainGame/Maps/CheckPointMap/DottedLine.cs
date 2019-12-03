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

    [SerializeField] private List<CheckPoint> checkPoints = new List<CheckPoint>();

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

    public void DrawDottedLine(Vector2 start, Vector2 end)
    {
        //DestroyAllDots();
        List<Vector2> positions = new List<Vector2>();

        Vector2 point = start;
        Vector2 direction = (end - start).normalized;
        int lineSteps = 15;
        Vector3 lineStart = GetPoint(0f);
        for (int i = 1; i <= lineSteps; i++)
        {
            Vector3 lineEnd = GetPoint(i / (float)lineSteps);
            positions.Add(lineEnd);
            lineStart = lineEnd;
        }

        //while ((end - start).magnitude > (point - start).magnitude)
        //{
        //    positions.Add(point);
        //    point += (direction * Delta);
        //}

        StartCoroutine("Render", positions);
    }

    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(checkPoints[0].StartPoint.transform.position,
            new Vector3(2f, 2.4f, 0f), 
            checkPoints[1].EndPoint.transform.position, t));
    }

    private IEnumerator Render(List<Vector2> positionList)
    {
        checkPoints[0].Parent.gameObject.SetActive(true);
        foreach (var position in positionList)
        {
            yield return new WaitForSeconds(0.25f);
            var g = GetOneDot();
            g.transform.position = position;
            dots.Add(g);
        }
        checkPoints[1].Parent.gameObject.SetActive(true);
        yield return null;
    }
}
