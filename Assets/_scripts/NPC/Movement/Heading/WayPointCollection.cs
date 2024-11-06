using System;
using System.Collections;
using System.Collections.Generic;
using Gianni.Helper;
using UnityEngine;

namespace GellosGames
{
    [Serializable]
    public class WayPointCollection : Node
    {
        [SerializeField]
        bool Loop;
        [SerializeField]
        public Transform[] Waypoints;
        
        RingCollection<Transform> WayPointPool;
        
        public WayPointCollection(MonoBehaviour mother) : base(mother)
        {
            WayPointPool = new RingCollection<Transform>(Waypoints);
        }
        public bool NextWayPoint(out Transform nextItem)
        {
            nextItem = WayPointPool.Next();
            if (Loop)
                return true;
            return !WayPointPool.IsFull;
        }
        public override void Update()
        {
            throw new NotImplementedException();
        }
    }

}
