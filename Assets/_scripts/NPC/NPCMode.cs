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
        private NPCModeBehaviour m_CurrentActionMode ;
        private NPCModeBehaviour m_CurrentMovementMode ;
        protected NPCModeBehaviour CurrentActionMode
        {
            get => m_CurrentActionMode;
            set => m_CurrentActionMode = value;
        }
        protected NPCModeBehaviour CurrentMovementMode
        {
            get => m_CurrentMovementMode;
            set => m_CurrentMovementMode = value;
        }
        protected void Update()
        {
            CurrentMovementMode.Update();
            CurrentActionMode.Update();
        }
        public abstract void StateChanged<TAction>(TAction newState) where TAction : Enum;
    }
    public abstract class NPCModeBehaviour
    {
        public NPCMode Npc;
        public abstract void Update();
        protected NPCModeBehaviour(NPCMode Mother)
        {
            Npc = Mother;
        }
    }
}
