using System;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    public struct ClosestPlayerDistances : TargetingLogic<ClosestPlayerDistances>, IComparable<ClosestPlayerDistances>
    {
        public float DistanceToNextPlayer;
        public Transform Player;
        void getDistanceObj(Transform player, Transform npc)
        {
            Player = player;
            DistanceToNextPlayer = Vector3.Distance(Player.position, npc.position);
        }
        bool isCloserTo(ClosestPlayerDistances newUpdate)
        {
            if (newUpdate.Player == Player)
            {
                return false;
            }
            return newUpdate.DistanceToNextPlayer < DistanceToNextPlayer;
        }
        public void GetRanking(List<ClosestPlayerDistances> ranking, Transform npc)
        {
            ranking.Clear();
            foreach (var player in PlayerEvents.GetAllActivePlayerEvents())
            {
                var dis = new ClosestPlayerDistances();
                dis.getDistanceObj(player.PlayerObject.transform, npc);
                ranking.Add(dis);
            }
            ranking.Sort();
        }
        public bool CheckForTarget(List<ClosestPlayerDistances> ranking, Transform npc)
        {
            GetRanking(ranking, npc);
            if (!isCloserTo(ranking[0]))
                return false;
            this = ranking[0];
            return true;
        }
        public int CompareTo(ClosestPlayerDistances other)
        {
            return DistanceToNextPlayer.CompareTo(other.DistanceToNextPlayer);
        }
    }
}