using SevenSeas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataObjectMove
{
    public ObjectType Type { get; set; }
    public Vector2 PreviousPosition { get; set; }
    public Quaternion PreviousQuaternion { get; set; }
    public Direction PreviousDirection { get; set; }

    public DataObjectMove(ObjectType type, Vector2 previousPosition, Quaternion previousQuaternion, Direction previousDirection)
    {
        Type = type;
        PreviousPosition = previousPosition;
        PreviousQuaternion = previousQuaternion;
        PreviousDirection = previousDirection;
    }

}
