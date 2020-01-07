using SevenSeas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataObjectMove
{
    public DataObjectMove(ObjectType type, Vector2 previousPosition, Quaternion previousRotation, Quaternion previousLocalRotation, Direction previousDirection)
    {
        Type = type;
        PreviousPosition = previousPosition;
        PreviousRotation = previousRotation;
        PreviousLocalRotation = previousLocalRotation;
        PreviousDirection = previousDirection;
    }

    public ObjectType Type { get; set; }
    public Vector2 PreviousPosition { get; set; }
    public Quaternion PreviousRotation { get; set; }
    public Quaternion PreviousLocalRotation { get; set; }
    public Direction PreviousDirection { get; set; }
}
