using System.Collections.Generic;

namespace Gianni.Helper
{
	/// <summary>
	/// Tests for equality based solely on if the references are equal. This is useful for UnityEngine.Objects that overrides the default Equals 
	/// operator returning false if it's been destroyed.
	/// <see cref="https://forum.unity.com/threads/getinstanceid-what-is-it-really-for.293849/"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ObjectInstanceIDEqualityComparer<T> : EqualityComparer<T> where T : UnityEngine.Object
	{
		private static IEqualityComparer<T> _defaultComparer;
		public new static IEqualityComparer<T> Default
		{
			get { return _defaultComparer ?? (_defaultComparer = new ObjectInstanceIDEqualityComparer<T>()); }
		}
		#region IEqualityComparer<T> Members
		public override bool Equals(T x, T y)
		{
			return x.GetInstanceID() == y.GetInstanceID();
		}

		public override int GetHashCode(T obj)
		{ // If null Prüfung wird nicht benötigt, lieber mit Exaption
			return obj.GetInstanceID();
		}
		#endregion
	}
}
