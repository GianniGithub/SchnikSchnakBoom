using UnityEngine;

namespace Gianni.Helper
{
	public static class ColorExtension
	{
		public static void InvertColores(this Color Farbe, ref Color Target)
		{
			Target.r = 1 - Farbe.r;
			Target.g = 1 - Farbe.g;
			Target.b = 1 - Farbe.b;
		}
		public static Color InvertColores(this Color Farbe)
		{
			Farbe.r = 1 - Farbe.r;
			Farbe.g = 1 - Farbe.g;
			Farbe.b = 1 - Farbe.b;
			return Farbe;
		}
	}
}
