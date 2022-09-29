using System;
using System.Collections;
using System.Collections.Generic;
using GellosGames;
using UnityEngine;

namespace GellosGames
{

    public abstract class PageEventListener : MonoBehaviour
    {
        public virtual void RegisterThisListener()
        {
            Locator.GetService<PageEvents>().broadcastEventChangedListener += OnPageChanged;
        }

        public virtual void UnRegisterThisListener()
        {
            Locator.GetService<PageEvents>().broadcastEventChangedListener -= OnPageChanged;
        }

        protected virtual void OnDestroy()
        {
            try
            {
                UnRegisterThisListener();
            }
            catch (Exception e)
            {
                if(!(e is KeyNotFoundException))
                    throw;
            }
        }
        /// <summary>
        /// Becarefull with this event
        /// </summary>
        public virtual void DisableAwake() { }

        /// <summary>
        /// Load all Listener, On Start also if Disabled, is called after RegisterThisListener
        /// </summary>
        public virtual void DisableStart() { }

        /// <summary>
        /// General Event what is raised on each User Action or state changes 
        /// </summary>
        /// <param name="sender">Origne Caller</param>
        /// <param name="e">Sender Meta Data</param>
        protected abstract void OnPageChanged(MonoBehaviour sender, Event<UIPageStage> e);


    }
}

