using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    public abstract class PlayerListener : MonoBehaviour
    {
        protected PlayerEvent StateManger;
        // Start is called before the first frame update
        void Awake()
        {
            StateManger = Locator.GetService<PlayerEvents>().Player(gameObject);
            StateManger.AddActionListener(OnStateChange);
        }

        protected abstract void OnStateChange(MonoBehaviour sender, Event<PlayerActions> e);
    }
}
