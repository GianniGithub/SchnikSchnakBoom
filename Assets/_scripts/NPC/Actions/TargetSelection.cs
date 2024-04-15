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
    internal class TargetSelection<TLogic> : ScheduleUpdate where TLogic : struct, TargetingLogic<TLogic> 
    {
        public readonly List<TLogic> ranking;
        public TLogic Closest;
        public TargetSelection(MonoBehaviour Mother, float actionUpdateRate) : base(Mother, actionUpdateRate)
        {
            ranking = new List<TLogic>();
            var dummy = new TLogic();
            //TODO Ugly to work here with dummy
            dummy.GetRanking(ranking, base.Mother.transform);

            try
            {
                Closest = ranking[0];
            }
            catch (Exception e)
            {
                Debug.LogWarning("No Player in Scene, No Target to Attack");
                throw;
            }
            
            ((ITarget<TLogic>)base.Mother).TargetUpdate(Closest);
        }
        protected override void ScheduledUpdate()
        {
            if (Closest.CheckForTarget(ranking, Mother.transform))
            {
                //New Target (Player, NPC...)
                ((ITarget<TLogic>)Mother).TargetUpdate(Closest);
            }
        }
    }


    public interface TargetingLogic<T>
    {
        public void GetRanking(List<T> ranking, Transform npc);
        public bool CheckForTarget(List<T> ranking, Transform npc);
    }
    interface ITarget<T> where T : struct, TargetingLogic<T>
    {
        public void TargetUpdate(T target);
    }
}