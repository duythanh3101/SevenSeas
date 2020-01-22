using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasHelper 
{
    public static Vector2 WorldPointToCanvasPoint(Camera camera, Canvas canvas, Vector3 worldPosition)
    {
        Vector2 canvasPoint = Vector2.zero;
        Vector2 viewportPoint = camera.WorldToViewportPoint(worldPosition);
        RectTransform canvasRectTrans = canvas.GetComponent<RectTransform>();

        canvasPoint.Set(viewportPoint.x * canvasRectTrans.sizeDelta.x - canvasRectTrans.sizeDelta.x / 2,
            viewportPoint.y * canvasRectTrans.sizeDelta.y - canvasRectTrans.sizeDelta.y / 2);

        return canvasPoint;

    }
}
