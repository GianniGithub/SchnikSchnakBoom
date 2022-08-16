using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleriePath : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float lengthLimit;
    [SerializeField]
    int resulution;

    LineRenderer lr;
    Vector3[] points;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    void Start()
    {
        lr.positionCount = resulution;
        points = new Vector3[resulution];
    }

    // Update is called once per frame
    void Update()
    {
        var point = transform.position; // + new Vector3(0,2,0);
     

        for (int i = 0; i < resulution; i++)
        {
            var t = (lengthLimit / resulution) * i;
            var powerStraight = transform.up * speed * t;
            var gravityLost = (Mathf.Pow(t, 2) * 9.8f) / 2;
            points[i] = new Vector3(powerStraight.x, powerStraight.y - gravityLost, powerStraight.z) + point;
        }
        lr.SetPositions(points);
    }
}
