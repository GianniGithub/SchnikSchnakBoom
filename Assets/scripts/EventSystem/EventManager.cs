using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GellosGames;


namespace GellosGames
{

    public class EventManager : MonoBehaviour
    {
        public UIPageStage CurrentStage;
        [Tooltip("For Debug Reasons you can start where ever you want")]
        public UIPageStage StartStage = UIPageStage.Start_Menue;

        // Is loaded in EventManerEditor
        public List<PageEventListener> AllStageListener;

		void Awake()
		{

            Locator.AddService(new PageEvents());
            Locator.AddService(new UserAction());

            foreach (var listener in AllStageListener)
            {
                listener.DisableAwake();
            }
        }
		void Start()
        {

            // AllStageListener will be updated in EventManagerEditor each time
            foreach (var listener in AllStageListener)
            {
                listener.RegisterThisListener();
                listener.DisableStart();
            }
#if UNITY_EDITOR
            // Just for Debug
            Locator.GetService<PageEvents>().broadcastEventChangedListener += (s,e) => CurrentStage = e.StageNow;
#endif
            // First Stage
            Locator.GetService<PageEvents>().RunEvent(this, new Event<UIPageStage>(StartStage));
        }
        private void OnDestroy()
        {

            Locator.RemoveService<PageEvents>();
            Locator.RemoveService<UserAction>();
        }

    }





}
