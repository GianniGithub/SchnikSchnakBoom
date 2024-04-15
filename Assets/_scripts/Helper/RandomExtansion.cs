using UnityEngine;

namespace Gianni.Helper
{
	public static class RandomExtansion
	{
		public static float Angel => Random.Range(0f, 360f);
		public static Vector3 RotationAngelZ => new Vector3(0, 0, Angel);
	}
}
