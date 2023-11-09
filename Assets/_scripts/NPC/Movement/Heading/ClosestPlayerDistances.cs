using System.Collections.Generic;
using UnityEngine;
namespace GellosGames
{
    public struct ClosestPlayerDistances : IGetPlayerDistances, IComparer<ClosestPlayerDistances>
    {
        public float DistanceToNextPlayer;
        public Transform Player;
        public void GetDistanceObj(Transform player, Transform npc)
        {
            Player = player;
            DistanceToNextPlayer = Vector3.Distance(Player.position, npc.position);
        }
        public bool IsCloserTo(IGetPlayerDistances newUpdate)
        {
            return ((ClosestPlayerDistances)newUpdate).DistanceToNextPlayer < DistanceToNextPlayer;
        }
        public int Compare(ClosestPlayerDistances x, ClosestPlayerDistances y)
        {
            return x.DistanceToNextPlayer.CompareTo(y.DistanceToNextPlayer);
        }
    }
}