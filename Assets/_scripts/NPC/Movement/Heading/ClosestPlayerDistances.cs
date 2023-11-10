using System.Collections.Generic;
using UnityEngine;
namespace GellosGames
{
    public struct ClosestPlayerDistances : TargetLogic<ClosestPlayerDistances>, IComparer<ClosestPlayerDistances>
    {
        public float DistanceToNextPlayer;
        public Transform Player;
        public void GetDistanceObj(Transform player, Transform npc)
        {
            Player = player;
            DistanceToNextPlayer = Vector3.Distance(Player.position, npc.position);
        }
        public bool IsCloserTo(TargetLogic<ClosestPlayerDistances> newUpdate)
        {
            return ((ClosestPlayerDistances)newUpdate).DistanceToNextPlayer < DistanceToNextPlayer;
        }
        public int Compare(ClosestPlayerDistances x, ClosestPlayerDistances y)
        {
            return x.DistanceToNextPlayer.CompareTo(y.DistanceToNextPlayer);
        }
        public void GetRanking(List<ClosestPlayerDistances> ranking, Transform npc)
        {
            ranking.Clear();
            foreach (var player in PlayerEvents.GetAllActivePlayerEvents())
            {
                var dis = new ClosestPlayerDistances();
                dis.GetDistanceObj(player.PlayerObject.transform, npc);
                ranking.Add(dis);
            }
            ranking.Sort();
        }
        public bool Update(List<ClosestPlayerDistances> ranking, Transform npc)
        {
            GetRanking(ranking, npc);
            if (this.IsCloserTo(ranking[0]))
            {
                this = ranking[0];
                return true;
            }
            return false;
        }
    }
}