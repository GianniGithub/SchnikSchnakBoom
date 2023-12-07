using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace GellosGames
{
    public enum NPCModeState
    {
        idle,
        chasing,
        hidde,
        retreat,
        patrol,
        waitOfPlayer,
        playerSelection,
        followPath,
        attack,
        protecting
    }
    public abstract class NPCMode : NPCEvent
    {
        [SerializeField][ReadOnly]
        protected NPCModeState ActionState;
        [SerializeField][ReadOnly]
        protected NPCModeState MovementState;
        [SerializeField][ReadOnly]
        protected NPCModeState BonusState;
        
        private Mode m_CurrentActionMode ;
        private Mode m_CurrentMovementMode ;
        private Mode m_CurrentBonusMode ;
        protected Mode CurrentActionMode
        {
            get => m_CurrentActionMode;
            set => m_CurrentActionMode = value;
        }
        protected Mode CurrentMovementMode
        {
            get => m_CurrentMovementMode;
            set => m_CurrentMovementMode = value;
        }
        protected Mode CurrentBonusMode
        {
            get => m_CurrentBonusMode;
            set => m_CurrentBonusMode = value;
        }
        protected void Update()
        {
            m_CurrentActionMode.Update();
        }
        protected void FixedUpdate()
        {
            m_CurrentMovementMode.Update();
        }
    }
    public abstract class Mode
    {
        [NonSerialized]
        protected readonly MonoBehaviour Mother;
        public abstract void Update();
        protected Mode(MonoBehaviour Mother)
        {
            this.Mother = Mother;
        }
    }
    public abstract class ScheduleUpdate : Mode
    {
        public float ActionUpdateRate;
        private float timeLeft;
        protected ScheduleUpdate(MonoBehaviour mother, float actionUpdateRate = 0f) : base(mother)
        {
            ActionUpdateRate = actionUpdateRate;
        }
        public override void Update()
        {
            timeLeft += Time.deltaTime;
            if (timeLeft > ActionUpdateRate)
            {
                timeLeft -= ActionUpdateRate;
                ScheduledUpdate();
            }
        }
        protected abstract void ScheduledUpdate();
    }
}
