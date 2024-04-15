using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Gianni.Helper
{
	public sealed class IdentifieComparerGameobject<Ttransform> : IEqualityComparer<Ttransform>
		where Ttransform : Transform
	{
		public int GetHashCode(Ttransform value)
		{
			return RuntimeHelpers.GetHashCode(value);
		}

		public bool Equals(Ttransform left, Ttransform right)
		{
			return left.GetInstanceID() == right.GetInstanceID(); // Reference identity comparison
		}
	}
}

