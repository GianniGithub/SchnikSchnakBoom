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
        
        private Node _mCurrentActionNode ;
        private Node _mCurrentRotationNode ;
        private Node _mCurrentTargetNode ;
        protected Node CurrentActionNode
        {
            get => _mCurrentActionNode;
            set => _mCurrentActionNode = value;
        }
        protected Node CurrentRotationNode
        {
            get => _mCurrentRotationNode;
            set => _mCurrentRotationNode = value;
        }
        protected Node CurrentTargetNode
        {
            get => _mCurrentTargetNode;
            set => _mCurrentTargetNode = value;
        }
        protected void Update()
        {
            _mCurrentActionNode.Update();
            _mCurrentTargetNode.Update();
        }
        protected void FixedUpdate()
        {
            _mCurrentRotationNode.Update();
        }
    }
    [Serializable]
    public abstract class Node
    {
        MonoBehaviour mother;
        public MonoBehaviour Mother
        {
            get => mother;
            set => mother = value;
        }
        public abstract void Update();
        protected Node(MonoBehaviour Mother)
        {
            this.Mother = Mother;
        }
        protected Node()
        {
        }
    }
    public abstract class ScheduleUpdate : Node
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
