using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gianni.Helper
{
	/// <summary>
	/// Dicitionary mit begrenzten RAM Allocationen, für bessere Cache zugriffe und GC Vermeidungen (Boxing)
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	/// <remarks https://social.msdn.microsoft.com/Forums/vstudio/en-US/789c37ea-b9bf-4512-a418-f4f9532c59bf/dictionary-with-limited-size?forum=csharpgeneral></remarks>
	public class SizeLimitedOrderdDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
		/// <summary>
		/// Sammelt alle Grid Nummern Reihenfolge mit Key
		/// </summary>
		Dictionary<TKey, int> AllCreatetdGridNr;
		public int MaxItemsToHold;
		TKey[] ownQueue;
		int kopf = 0;
		int schwanz = 0;

		/// <summary>
		/// Begrenzt die zugriffe auf Size Limt
		/// </summary>
		/// <param name="SizeLimit">Maximale Dict Groesse</param>
		/// <param name="IEquality"></param>
		public SizeLimitedOrderdDictionary(int SizeLimit, IEqualityComparer<TKey> IEquality) : base(SizeLimit + 1, IEquality)
		{
			AllCreatetdGridNr = new Dictionary<TKey, int>(SizeLimit * 3, IEquality);
			MaxItemsToHold = SizeLimit;
			initKonstruktors();
		}
		public SizeLimitedOrderdDictionary(int SizeLimit) : base(SizeLimit + 1)
		{
			AllCreatetdGridNr = new Dictionary<TKey, int>();
			MaxItemsToHold = SizeLimit;
			initKonstruktors();
		}
		void initKonstruktors()
		{

			ownQueue = new TKey[MaxItemsToHold];
		}
		void Enqueue(TKey key)
		{
			ownQueue[schwanz++ % MaxItemsToHold] = key;
		}
		TKey Dequeue()
		{
#if UNITY_EDITOR
			if (kopf == schwanz)
				throw new ArgumentOutOfRangeException($"Reihenfolge eingeholt, Katze beist sich in den Tail");//: Sollte nie Passieren ; )
#endif
			return ownQueue[kopf++ % MaxItemsToHold];
		}
		public void Add(TKey key, TValue value, int Nr)
		{
			Add(key, value);
			AllCreatetdGridNr.Add(key, Nr);
		}
		public new void Add(TKey key, TValue value)
		{
			if (this.MaxItemsToHold != 0 && this.Count >= MaxItemsToHold)
			{
				base.Remove(Dequeue());
			}
			Enqueue(key);
			base.Add(key, value);
		}
		public int TryGetNr(TKey key)
		{
			return AllCreatetdGridNr[key];
		}
		public new bool Remove(TKey key)
		{
			throw new Exception("Automatisch Loeschendes Dictionary!");
		}
		public TValue PeekKopf()
		{
			return this[ownQueue[kopf % MaxItemsToHold]];
		}
		/// <summary>
		/// zum wiederverwerten alter Einträge
		/// </summary>
		/// <returns>Letzter Eintrag in der Schlange</returns>
		public TValue PeekSchwanz()
		{
			return this[ownQueue[schwanz % MaxItemsToHold]];
		}
		/// <summary>
		/// Gibt Grid zurück nach Indexierter Reihenfolge mit GridNr
		/// </summary>
		/// <param name="GridNr">Raster Index nach Erscheinung</param>
		/// <returns></returns>
		public TValue GetValueWithIndex(int GridNr)
		{
#if UNITY_EDITOR// Grid Index startet bei 1 nicht bei 0
			if (kopf == schwanz)
				Debug.LogWarning("Katze beist sich in den schwanz");
#endif
			var index = ownQueue[(GridNr - 1) % MaxItemsToHold];
			return this[index];
		}
		/// <summary>
		/// Iteration von Collection 0 bis int Parameter Bis
		/// </summary>
		/// <param name="Bis"></param>
		/// <returns></returns>
		public IEnumerable<TValue> GetValuesNachErstellung(int Bis)
		{
#if UNITY_EDITOR
			if (Bis >= schwanz)
				throw new ArgumentOutOfRangeException($"Abfrage ausserhalb der verfügbaren Grids");//: Sollte nie Passieren ; )
#endif
			for (var i = 0; i < Bis; i++)
			{
				yield return this[ownQueue[i % MaxItemsToHold]];
			}

		}

	}
}

