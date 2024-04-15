using System;
using System.Collections.Generic;

namespace Gianni.Helper
{
	public class ThreadSafeList<T> : IList<T>
	{
		protected List<T> _interalList = new List<T>();

		// Other Elements of IList implementation

		public IEnumerator<T> GetEnumerator()
		{
			return Clone().GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return Clone().GetEnumerator();
		}

		protected static readonly object _lock = new object();

		public int Count => throw new NotImplementedException();

		public bool IsReadOnly => throw new NotImplementedException();

		public T this[int index]
		{
			get => throw new NotImplementedException();

			set => throw new NotImplementedException();
		}

		public List<T> Clone()
		{
			var newList = new List<T>();

			lock (_lock)
			{
				_interalList.ForEach(x => newList.Add(x));
			}

			return newList;
		}

		public int IndexOf(T item)
		{
			throw new NotImplementedException();
		}

		public void Insert(int index, T item)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		public void Add(T item)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(T item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public bool Remove(T item)
		{
			throw new NotImplementedException();
		}
	}
}

