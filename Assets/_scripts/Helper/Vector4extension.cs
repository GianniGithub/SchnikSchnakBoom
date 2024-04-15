using System;
using UnityEngine;

namespace Gianni.Helper
{
	public static class Vector4extension
	{
		public static side IsOutOfBorderSide(this Vector4 sides, Vector2 target)
		{
			//        left = PivotMitte.x - (size.x / 2);
			//        rigth = PivotMitte.x + (size.x / 2);
			//        up = PivotMitte.y + (size.y / 2);
			//        down = PivotMitte.y - (size.y / 2);
			side s = 0;
			if (sides.x > target.x)
				s |= side.left;
			if (sides.y < target.x)
				s |= side.right;
			if (sides.z < target.y)
				s |= side.up;
			if (sides.w > target.y)
				s |= side.down;
			return s;
		}


	}

	//    public bool withinOfGridCeck(Vector2 outofGridPosition)
	//    {
	//        return MatrixAnfang >= outofGridPosition && MatrixEnde >= outofGridPosition;
	//    }
	//}
	[Flags]
	public enum side { inside = 0, up = 1, down = 2, left = 4, right = 8 }
}