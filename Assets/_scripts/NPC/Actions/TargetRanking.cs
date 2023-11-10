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
    internal class TargetRanking<TLogic> : Mode where TLogic : struct, TargetLogic<TLogic> 
    {
        public readonly List<TLogic> ranking;
        public TLogic Closest;
        public TargetRanking(MonoBehaviour Mother) : base(Mother)
        {
            ranking = new List<TLogic>();
            var dummy = new TLogic();
            dummy.GetRanking(ranking, base.Mother.transform);
            Closest = ranking[0];
            ((ITarget<TLogic>)base.Mother).TargetUpdate(Closest);
        }
        public override void Update()
        {
            if (Closest.Update(ranking, Mother.transform))
            {
                //New Target
                ((ITarget<TLogic>)Mother).TargetUpdate(Closest);
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