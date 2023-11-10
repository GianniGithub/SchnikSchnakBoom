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
    // Get Target (pick player behaviour), update Target behaviour depending on Target and inform ITarget<TLogic> Npc member
    internal class TargetRanking<TLogic> : NPCModeBehaviour where TLogic : struct, TargetLogic<TLogic> 
    {
        public readonly List<TLogic> ranking;
        public TLogic Closest;
        public TargetRanking(NPCMode Mother) : base(Mother)
        {
            ranking = new List<TLogic>();
            var dummy = new TLogic();
            dummy.GetRanking(ranking, Npc.transform);
            Closest = ranking[0];
            ((ITarget<TLogic>)Npc).TargetUpdate(Closest);
        }
        public override void Update()
        {
            if (Closest.Update(ranking, Npc.transform))
            {
                //New Target
                ((ITarget<TLogic>)Npc).TargetUpdate(Closest);
            }
        }
    }


    public interface TargetLogic<T>
    {
        public void GetRanking(List<T> ranking, Transform npc);
        public bool Update(List<T> ranking, Transform npc);
    }
    interface ITarget<T> where T : struct, TargetLogic<T>
    {
        public void TargetUpdate(T target);
    }
}