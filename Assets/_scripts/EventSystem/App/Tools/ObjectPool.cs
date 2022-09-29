using System;
using System.Collections.Generic;

namespace GrandDevs.ExtremeScooling.Gameplay
{
    public class ObjectPool<T>
    {
        private object _lock = new object();
        private List<T> _poolObjects;
        private List<T> _poolObjectsInUse;
        private List<T> _poolObjectsAll;

        public ObjectPool()
        {
            _poolObjects = new List<T>();
            _poolObjectsInUse = new List<T>();
            _poolObjectsAll = new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
		{
            return _poolObjectsAll.GetEnumerator();
        }

        public void AddObjectToPool(T poolObject)
        {
            lock (_lock)
            {
                _poolObjects.Add(poolObject);
                _poolObjectsAll.Add(poolObject);
            }
        }

        public void ClearObjectPool()
        {
            lock (_lock)
            {
                //TODO: IMPLEMENT INTERFACE WITH DISPOSE METHOD IF NEED AND ADD WHERE T INTERFACE_NAME

                //foreach (T element in _poolObjects)
                //{
                //    UnityEngine.Object.Destroy(element);
                //}

                //foreach (T element in _poolObjectsInUse)
                //{
                //    UnityEngine.Object.Destroy(element);
                //}

                _poolObjects.Clear();
                _poolObjectsInUse.Clear();
            }
        }

        public T GetObjectFromPool()
        {
            lock (_lock)
            {
                var poolObject = RemoveObject();
                _poolObjectsInUse.Add(poolObject);

                return poolObject;
            }
        }

        public void ReturnObjectToPool(T poolObject)
        {
            lock (_lock)
            {
                _poolObjects.Add(poolObject);
                _poolObjectsInUse.Remove(poolObject);
            }
        }

        private void ReturnFirstObjectInUseToPool()
        {
            lock (_lock)
            {
                var poolObject = _poolObjectsInUse[0];
                _poolObjects.Add(poolObject);
                _poolObjectsInUse.Remove(poolObject);
            }
        }

        private T RemoveObject()
        {
            if (_poolObjects.Count == 0)
            {
                ReturnFirstObjectInUseToPool();

                throw new Exception("All objects from object pool in use!");
            }

            var poolObject = _poolObjects[0];
            _poolObjects.RemoveAt(0);
            return poolObject;
        }
    }
}