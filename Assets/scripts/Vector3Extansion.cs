using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gianni.Helper
{
    public static class Vector3Extansion
    {
        public static Vector2 ToVector2XZ(this Vector3 from)
        {
            return new Vector2(from.x, from.z);
        }
        public static Vector3 ToVectorXZ(this Vector2 from)
        {
            return new Vector3(from.x, 0f, from.y);
        }
    } 
}
