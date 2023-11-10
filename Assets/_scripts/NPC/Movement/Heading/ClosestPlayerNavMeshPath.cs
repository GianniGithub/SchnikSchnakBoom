using System;
using System.Collections.Generic;
using Gianni.Helper;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
namespace GellosGames
{
    public struct ClosestPlayerNavMeshPath : TargetLogic<ClosestPlayerNavMeshPath>, IComparer<ClosestPlayerNavMeshPath>
    {
        public float DistanceToNextPlayer;
        public Transform Player;

        public void GetDistanceObj(Transform player, Transform npc)
        {
            Player = player;
            var path = new NavMeshPath();
            if (!NavMesh.CalculatePath(npc.position, player.position, NavMesh.AllAreas, path))
            {
                DistanceToNextPlayer = 0f;
                return;
            }
            var wayPoints = path.corners;
            for (int i = 0; i < wayPoints.Length + 1; i++)
            {
                DistanceToNextPlayer += Vector3.Distance(wayPoints[i], wayPoints[i + 1]);
            }
        }
        public bool IsCloserTo(TargetLogic<ClosestPlayerNavMeshPath> newUpdate)
        {
            return ((ClosestPlayerNavMeshPath)newUpdate).DistanceToNextPlayer < DistanceToNextPlayer;
        }
        public int Compare(ClosestPlayerNavMeshPath x, ClosestPlayerNavMeshPath y)
        {
            return x.DistanceToNextPlayer.CompareTo(y.DistanceToNextPlayer);
        }
        public void GetRanking(List<ClosestPlayerNavMeshPath> ranking, Transform npc)
        {
            ranking.Clear();
            foreach (var player in PlayerEvents.GetAllActivePlayerEvents())
            {
                var dis = new ClosestPlayerNavMeshPath();
                dis.GetDistanceObj(player.PlayerObject.transform, npc);
                ranking.Add(dis);
            }
            ranking.Sort();
        }
        public bool Update(List<ClosestPlayerNavMeshPath> ranking, Transform npc)
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