using GellosGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    public enum GameActions
    {
        None = 0,
        OnGamePadStateChange = 202,
        OnPlayerAdded = 302,
        OnNPCAdded = 702,
    }
    public class GameEvents : EventManager<GameActions, GameEventArgs>
    {
        public static GameEvents Instance { get { return instance; } }
        static GameEvents instance;
        public GameEvents()
        {
            instance = this;
        }
        public override void TriggerEvent(MonoBehaviour sender, GameEventArgs listener)
        {
            if (EventDictionary.TryGetValue(listener.Action, out var thisEvent))
            {
                thisEvent.Invoke(sender, listener);
            }
        }
    }
    public abstract class GameEvent : MonoBehaviour
    {
    }
    public struct GameEventArgs
    {
        public readonly GameActions Action { get; }
        public System.EventArgs EventInfos { get; }

        public GameEventArgs(GameActions action, EventArgs e) : this(action)
        {
            EventInfos = e;
        }

        public GameEventArgs(GameActions action) : this()
        {
            this.Action = action;
        }
    }
}
