using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace GellosGames
{
    public enum TargetingState
    {
        newTarget,
        same,
    }
    // Get Route to Target, update Route behaviour depending on Target and inform IRouted<TLogic> Npc member
    internal class GetNextPlayer<TLogic> :  NPCModeBehaviour where TLogic : IGetPlayerDistances, new()
    {
        private readonly List<TLogic> playerDistances;
        private TLogic ClosestPlayer;
        public GetNextPlayer(NPCMode Mother) : base(Mother)
        {
            playerDistances = new List<TLogic>();
            GetPlayerDistances();
            ClosestPlayer = playerDistances[0];
            ((IRouted<TLogic>)Npc).RoutUpdate(ClosestPlayer);
        }
        public override void Update()
        {
            GetPlayerDistances();
            if (ClosestPlayer.IsCloserTo(playerDistances[0]))
            {
                ClosestPlayer = playerDistances[0];
                //New Target
                ((IRouted<TLogic>)Npc).RoutUpdate(ClosestPlayer);
            }
        }
        private void GetPlayerDistances()
        {
            playerDistances.Clear();
            foreach (var player in PlayerEvents.GetAllActivePlayerEvents())
            {
                var dis = new TLogic();
                dis.GetDistanceObj(player.PlayerObject.transform, Npc.transform);
                playerDistances.Add(dis);
            }
            playerDistances.Sort();
        }
    }

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
    public interface IGetPlayerDistances
    {
        public void GetDistanceObj(Transform player, Transform npc);
        public bool IsCloserTo(IGetPlayerDistances newUpdate);
    }
    interface IRouted<T> where T : IGetPlayerDistances
    {
        public void RoutUpdate(T rout);
    }
}