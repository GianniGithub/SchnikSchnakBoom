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
        protecting,
        PathError
    }
    public abstract class NPCMode : NPCEvent
    {
        //Movement is controlled by Mother Obj, it does not need a update Methode
        [SerializeField][ReadOnly]
        protected NPCModeState ActionState;
        [SerializeField][ReadOnly]
        protected NPCModeState RotationState;
        [SerializeField][ReadOnly]
        protected NPCModeState BonusState;
        
        private Mode m_CurrentActionMode ;
        private Mode m_CurrentRotationMode ;
        private Mode m_CurrentBonusMode ;
        protected Mode CurrentActionMode
        {
            get => m_CurrentActionMode;
            set => m_CurrentActionMode = value;
        }
        protected Mode CurrentRotationMode
        {
            get => m_CurrentRotationMode;
            set => m_CurrentRotationMode = value;
        }
        protected Mode CurrentBonusMode
        {
            get => m_CurrentBonusMode;
            set => m_CurrentBonusMode = value;
        }
        protected void Update()
        {
            m_CurrentActionMode.Update();
            m_CurrentBonusMode.Update();
        }
        protected void FixedUpdate()
        {
            m_CurrentRotationMode.Update();
        }
    }
    [Serializable]
    public abstract class Mode
    {
        MonoBehaviour mother;
        public MonoBehaviour Mother
        {
            get => mother;
            set => mother = value;
        }
        public abstract void Update();
        protected Mode(MonoBehaviour Mother)
        {
            this.Mother = Mother;
        }
        protected Mode()
        {
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
