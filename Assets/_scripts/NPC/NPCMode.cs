using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace GellosGames
{
    public enum NPCMoveState
    {
        idle,
        chasing,
        hidde,
        retreat,
        patrol
    }
    public enum NPCActionState
    {
        waitOfPlayer,
        attack,
        protecting
    }
    public abstract class NPCMode : NPCEvent
    {
        protected float ActionUpdateRate = 0f;
        private float timeLeft;
        private Mode m_CurrentActionMode ;
        private Mode m_CurrentMovementMode ;
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
        protected void Update()
        {
            timeLeft += Time.deltaTime;
            if (timeLeft > ActionUpdateRate)
            {
                timeLeft -= ActionUpdateRate;
                m_CurrentActionMode.Update();
            }
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
        protected Mode()
        {
        }
    }
}
