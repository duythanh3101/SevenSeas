using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLineDrawer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DottedLine.Instance.DrawDottedLine(new Vector3(0.35f, 4f, 0f), new Vector3(2.35f, 0.5f, 0f));
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
