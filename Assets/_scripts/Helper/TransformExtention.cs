
using UnityEngine;

namespace Gianni.Helper
{
	public static class TransformExtention
	{

		public static void CopyToTransform(this Transform original, Transform From)
		{
			original.position = From.position;
			original.rotation = From.rotation;
			original.localScale = From.localScale;
		}
	}
}
