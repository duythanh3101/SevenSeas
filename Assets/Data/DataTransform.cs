using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataTransform
{

    public Vector3 position;
    public Quaternion modelRotation;

    public void SetData(Vector3 pos, Quaternion rot)
    {
        position = pos;
        modelRotation = rot;
    }

}
