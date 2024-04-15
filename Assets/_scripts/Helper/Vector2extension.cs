using UnityEngine;

namespace Gianni.Helper
{
	public static class Vector2extension
	{
		/// <summary>
		/// Sind beid Vector ints in Range von 0 bis List.Count oder Array.Length
		/// </summary>
		/// <param name="V2int"></param>
		/// <param name="ListCount">Mas length of Collection</param>
		/// <returns></returns>
		public static bool IsInCount(this Vector2Int V2int, int ListCount)
		{
			if (ListCount > V2int.x && ListCount > V2int.y &&
				V2int.x >= 0 && V2int.y >= 0)
				return true;

			Debug.LogWarning("Out Of Range");
			return false;
		}
		public static bool IsInRange(this Vector2Int V2int, int toCheck)
		{
			return toCheck >= V2int.x && toCheck <= V2int.y;
		}
		public static bool IsInRange(this Vector2 V2int, float toCheck)
		{
			return toCheck >= V2int.x && toCheck <= V2int.y;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="u"></param>
		/// <param name="v"></param>
		/// <returns></returns>
		/// <remarks>https://matheguru.com/lineare-algebra/winkel-zwischen-zwei-vektoren.html</remarks>
		public static float Winkel(this Vector2 u, Vector2 v)
		{
			var Betrag = Vector2.Dot(u, v) / (u.magnitude * v.magnitude);
			return Mathf.Atan(Betrag);
		}
		/// <summary>
		/// Auch Dot Produkt
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float ScalarProdukt(this Vector2 a, Vector2 b)
		{
			return (a.x * b.x) + (a.y * b.y);
		}
		public static Vector2 SwitchXY(this Vector2 a)
		{
			return new Vector2(a.y,a.x);
		}
	}
}
