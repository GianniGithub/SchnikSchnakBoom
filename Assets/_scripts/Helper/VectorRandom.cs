using UnityEngine;



namespace Gianni.Helper
{
	public class VectorRandom : System.Random
	{
		public VectorRandom() : base()
		{
		}

		//public VectorRandom(int Seed) : base(Seed)
		//{
		//}
		public Vector2 NextVector(float OffsetRange)
		{
			return new Vector2(Next(-OffsetRange, OffsetRange), Next(-OffsetRange, OffsetRange));
		}
		public int Next(Vector2Int range)
		{
			return base.Next(range.x, range.y);
		}
		public float Next(Vector2 range)
		{
			return (float)NextDouble() * (range.y - range.x) + range.x;
		}
		public virtual float Next(float maxValue)
		{
			return (float)NextDouble() * maxValue;
		}
		public virtual float Next(float minValue, float maxValue)
		{
			return (float)NextDouble() * (maxValue - minValue) + minValue;
		}
	}
}
