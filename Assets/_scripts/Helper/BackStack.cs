using System;
using System.Collections.Generic;
using System.Collections;

namespace GellosGames{

    public class BackStack<Value> : IEnumerable<Value>
    {
        protected int Tail = 0;
        protected readonly int MaxItemsToHold;
        protected Value[] PoolObjects;
        public Value this[int Index]
        {
            get { return PoolObjects[Index]; }
            set { PoolObjects[Index] = value; }
        }
        public int Count => Tail;
        
        public BackStack(int MaxItemsToHold)
        {
            this.MaxItemsToHold = MaxItemsToHold;
            PoolObjects = new Value[MaxItemsToHold];
        }
        public int Enqueue(Value poolObj)
        {
            var index = Tail++;
#if UNITY_EDITOR
            if (Tail > MaxItemsToHold)
                throw new ArgumentOutOfRangeException("Reihenfolge eingeholt, Katze beist sich in den Tail");//: Sollte nie Passieren ; )
#endif
            PoolObjects[index] = poolObj;
            return index;
        }

        public Value Dequeue()
        {
#if UNITY_EDITOR
            if (Tail < 0)
                throw new ArgumentOutOfRangeException("Reihenfolge eingeholt, Katze beist sich in den Tail");//: Sollte nie Passieren ; )
#endif
            return PoolObjects[--Tail];
        }

        public Value Peek()
        {
            return PoolObjects[Tail];
        }

        public void Clear()
        {
            Tail = 0;
        }

        public IEnumerator<Value> GetEnumerator()
        {
            for (int i = 0; i < Tail; i++)
            {
                yield return PoolObjects[i % MaxItemsToHold];
            }
        }

        public IEnumerator<Value> GetNegativEnumerator()
        {
            for (int i = Tail - 1; i >= 0; i--)
            {
                yield return PoolObjects[i % MaxItemsToHold];
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
