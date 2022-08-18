using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArtilleriePath : MonoBehaviour
{
    public Vector2 PowerRange;
    public PlayersControlls Controll;
    public float Power;
    [SerializeField]
    float lengthLimit;
    [SerializeField]
    int resulution;

    LineRenderer lr;
    Vector3[] points;
    Quaternion[] rotations;
    public float PointsDistanzToEacheuser => lengthLimit / resulution;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    void Start()
    {
        lr.positionCount = resulution;
        points = new Vector3[resulution];
        rotations = new Quaternion[resulution];

        Controll.OnLookStateSwitch += Controll_OnLookStateSwitch;
    }

    private void Controll_OnLookStateSwitch(bool state, Vector2 axes)
    {
        if (enabled != state)
        {
            lr.enabled = state;
            enabled = state;
        }

        var t = Mathf.Abs(axes.x) + Mathf.Abs(axes.y);
        Power = Mathf.Lerp(PowerRange.x, PowerRange.y, t);
    }

    public ArtilleriePathPointData GetArtilleriePathData()
    {
        return new ArtilleriePathPointData(points, rotations, lengthLimit / resulution);
    }
    // Update is called once per frame
    void Update()
    {
        var point = transform.position; // + new Vector3(0,2,0);
        points[0] = point;
        rotations[0] = Quaternion.LookRotation(transform.up);

        for (int i = 1; i < resulution; i++)
        {
            var t = (lengthLimit / resulution) * i;

            //Positions
            var powerStraight = transform.up * Power * t;
            var gravityLost = (Mathf.Pow(t, 2) * 9.8f) / 2;
            points[i] = new Vector3(powerStraight.x, powerStraight.y - gravityLost, powerStraight.z) + point;

            //Rotations
            var direction = points[i] - points[i - 1];
            rotations[i] = Quaternion.LookRotation(direction);
        }
        lr.SetPositions(points);
    }

}

public class ArtilleriePathPointData
{
    public Vector3[] points;
    public Quaternion[] rotations;
    public float PointsDistanzToEacheuser;

    public ArtilleriePathPointData(Vector3[] points, Quaternion[] rotations, float pointsDistanzToEacheuser)
    {
        this.points = (Vector3[])points.Clone();
        this.rotations = (Quaternion[])rotations.Clone(); ;
        PointsDistanzToEacheuser = pointsDistanzToEacheuser;
    }
}
