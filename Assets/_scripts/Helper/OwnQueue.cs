using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gianni.Helper
{
	public class DoubleQueue<TArray1, TArray2> where TArray1 : struct
		where TArray2 : struct
	{

		protected int MaxItemsToHold;
		protected TArray1[] Ar1;
		protected TArray2[] Ar2;
		protected int kopf;
		protected int schwanz;

		public bool IsFull => Schwanz - MaxItemsToHold >= Kopf;
		public TArray1[] Collection => Ar1;
		public int Count => schwanz;
		public int Kopf => kopf;
		public int Schwanz => schwanz;

		public DoubleQueue(int MaxItemsToHold)
		{
			this.MaxItemsToHold = MaxItemsToHold;
			Ar1 = new TArray1[MaxItemsToHold];
			Ar2 = new TArray2[MaxItemsToHold];
			kopf = 0;
			schwanz = 0;
		}

		public int Enqueue(TArray1 Value1, TArray2 Value2)
		{
#if UNITY_EDITOR
			CheckForLimitErreicht();
#endif
			var index = schwanz % MaxItemsToHold;
			Ar1[index] = Value1;
			Ar2[index] = Value2;
			return schwanz++;

		}
		public int Dequeue()
		{
#if UNITY_EDITOR
			if (kopf == schwanz)
				throw new ArgumentOutOfRangeException("Reihenfolge eingeholt, Katze beist sich in den Tail");//: Sollte nie Passieren ; )
#endif
			return kopf++ % MaxItemsToHold;
		}
		public int Peek()
		{
			return kopf % MaxItemsToHold;
		}
		public int Peek(int Offset)
		{
			return (kopf + Offset) % MaxItemsToHold;
		}
		/// <summary>
		/// Abstand zwischen Ende und Anfang der Queue
		/// </summary>
		/// <param name="differenz">Wegpunkte abstand</param>
		/// <returns>Größer als 500?</returns>
		public int GetDistanz()
		{
			return schwanz - kopf;
		}

		private void CheckForLimitErreicht()
		{
			if (Schwanz - MaxItemsToHold >= Kopf)
			{
				string name = this.GetType().Name;
				throw new ArgumentOutOfRangeException("Zu Viele Active Items(" + name + "): MaxItemsToHold wurde Erreicht (WegPunkte?)");//: Sollte nie Passieren ; )
			}
		}

	}
	/// <summary>
	/// Queue für Value Typen um Boxing zu vermeiden
	/// </summary>
	/// <typeparam name="Value"></typeparam>
	public struct StructQueue<Value> where Value : struct
	{
		private int Head;
		private int Tail;
		private int MaxItemsToHold;
		private Value[] ValueArray;
		public int Count => Tail - Head;

		public StructQueue(int MaxItemsToHold)
		{
			Head = 0;
			Tail = 0;
			this.MaxItemsToHold = MaxItemsToHold;
			ValueArray = new Value[MaxItemsToHold];

		}

		public int Enqueue(Value Wert)
		{
			//Falls Array zu klein, dann wird verdoppelt : https://referencesource.microsoft.com/#mscorlib/system/collections/generic/list.cs,aeb6ba6c11713802
			if (Tail >= MaxItemsToHold)
			{
				Debug.Log("Array zu klein gewesen, menge: " + MaxItemsToHold);
				MaxItemsToHold *= 2;
				var newItems = new Value[MaxItemsToHold];
				if (Tail > 0)
				{
					Array.Copy(ValueArray, 0, newItems, 0, Tail);
				}
				ValueArray = newItems;
			}

			var index = Tail % MaxItemsToHold;
			ValueArray[index] = Wert;
			return Tail++;

		}
		public Value Dequeue()
		{
#if UNITY_EDITOR
			if (Head == Tail)
				throw new IndexOutOfRangeException();
#endif
			return ValueArray[Head++ % MaxItemsToHold];
		}
		public void Clear()
		{
			Head = 0;
			Tail = 0;
		}
	}


	/// <summary>
	/// Queue die beim Ende wieder am anfang weiter Läuft
	/// </summary>
	/// <typeparam name="Value"></typeparam>
	public class RingCollection<Value> : IEnumerable<Value>
	{
		protected int Head;
		protected int Tail;
		readonly int MaxItemsToHold;
		protected Value[] PoolObjects;
		public Value this[int Index]
		{
			get { return PoolObjects[Index]; }
			set { PoolObjects[Index] = value; }
		}
		/// <summary>
		/// Is at the end of the collection
		/// </summary>
		public bool IsFull => Tail - MaxItemsToHold >= Head;
		public int Count => Tail - Head;
		public RingCollection(Value[] array)
		{
			PoolObjects = array;
			Head = 0;
			Tail = array.Length;
			this.MaxItemsToHold = array.Length;
		}
		public RingCollection(int MaxItemsToHold)
		{
			Head = 0;
			Tail = 0;
			this.MaxItemsToHold = MaxItemsToHold;
			PoolObjects = new Value[MaxItemsToHold];
		}
		// Kann man hier nicht platzieren, da Value Type GegnerIndex haben muss, kein Bock auf Interface, soll immer vererbt werden
		//public void Delete(ushort nr)
		//{
		//    var old = PoolObjects[Head++ oder Tail++ je nach dem % MaxItemsToHold];
		//    PoolObjects[nr] = old;
		//    old.GegnerIndex = nr;
		//}
		/// <summary>
		/// Füght in die EndlosQueue ein Parameter hinzu
		/// </summary>
		/// <param name="poolObj"></param>
		/// <returns>Index an dem es hinzugefügt wurde</returns>
		public int Add(Value poolObj)
		{
			var index = Tail++ % MaxItemsToHold;
#if UNITY_EDITOR
			if (Head == Tail)
				throw new ArgumentOutOfRangeException("Reihenfolge eingeholt, Katze beist sich in den Tail");//: Sollte nie Passieren ; )
#endif
			PoolObjects[index] = poolObj;
			return index;
		}

		public int AddNewRange(Value[] poolObj)
		{
#if UNITY_EDITOR
			if (poolObj.Length > MaxItemsToHold)
				throw new IndexOutOfRangeException("pool Obj to big");
#endif
			PoolObjects = poolObj;
			Tail += poolObj.Length;
			return Tail;
		}

		public Value previous()
		{
			if (--Head < 0) //damit nie Negativ
				Head += MaxItemsToHold;
			return PoolObjects[Head % MaxItemsToHold];
		}

		public Value Next()
		{
			// keine Tail Beiß prüfung, da RingBuffer
			return PoolObjects[Head++ % MaxItemsToHold];
		}

		public Value Dequeue()
		{
#if UNITY_EDITOR
			if (Head == Tail)
				throw new ArgumentOutOfRangeException("Reihenfolge eingeholt, Katze beist sich in den Tail");//: Sollte nie Passieren ; )
#endif
			return PoolObjects[Head++ % MaxItemsToHold];
		}

		public Value Peek()
		{
			return PoolObjects[Head % MaxItemsToHold];
		}

		public void Clear()
		{
			Head = 0;
			Tail = 0;
		}

		public void ForEach(Action<Value> ToDo)
		{
#if UNITY_EDITOR
			CheckForLimitErreicht();
#endif
			for (int i = Head; i < Tail; i++)
			{
				ToDo(PoolObjects[i % MaxItemsToHold]);
			}
		}

		public IEnumerator<Value> GetEnumerator()
		{
#if UNITY_EDITOR
			CheckForLimitErreicht();
#endif
			for (int i = Head; i < Tail; i++)
			{
				yield return PoolObjects[i % MaxItemsToHold];
			}
		}

		public Value[] CopyToArray()
		{
#if UNITY_EDITOR
			CheckForLimitErreicht();
#endif
			var retur = new Value[Tail - Head];
			int l = 0;
			for (int i = Head; i < Tail; i++)
			{
				retur[l++] = PoolObjects[i % MaxItemsToHold];
			}
			return retur;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void CheckForLimitErreicht()
		{
			if (Tail - MaxItemsToHold >= Head)
			{
				string name = PoolObjects[0].GetType().Name;
				throw new ArgumentOutOfRangeException("Zu Viele Active Items(" + name + "): MaxItemsToHold wurde Erreicht");//: Sollte nie Passieren ; )
			}
		}

		internal Value[] GetArray()
		{
			return PoolObjects;
		}
	}
}