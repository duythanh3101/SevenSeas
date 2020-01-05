using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataTransform
{

    public Vector3 position;
    public Quaternion modelRotation;
    public Direction boatDirection;

    public void SetData(Vector3 pos, Quaternion modelRot, Direction boatDir)
    {
        position = pos;
        modelRotation = modelRot;
        boatDirection = boatDir;
    }

}
