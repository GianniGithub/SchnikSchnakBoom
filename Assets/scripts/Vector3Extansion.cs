using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extansion
{
    public static Vector2 ToVectorXZ(this Vector3 from)
    {
        return new Vector2(from.x, from.z);
    }
}
