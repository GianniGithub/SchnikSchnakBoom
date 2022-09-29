using System.Collections;
using System.Collections.Generic;
using GellosGames;
using UnityEngine;

namespace GellosGames
{
    public abstract class SingelStageListener : PageEventListener
    {
        protected abstract UIPageStage[] OnStages { get; }
        public override void RegisterThisListener()
        {
            Locator.GetService<PageEvents>().AddActionListener(OnPageChanged, OnStages);
        }
        public override void UnRegisterThisListener()
        {
            Locator.GetService<PageEvents>().RemoveActionListener(OnPageChanged, OnStages);
        }
    }
}
