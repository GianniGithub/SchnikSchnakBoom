using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    public class GameEventHandler : MonoBehaviour
    {
        public List<GameObject> Listener;

        private void Start()
        {
            List<GameEvent> coll = new List<GameEvent>();
            var gem = new GameEventManager();

            GetComponentsInParent(true, coll);
            SetUpEvents(coll, gem);

            foreach (var item in Listener)
            {
                if (item == null)
                    continue;

                item.GetComponentsInParent(true, coll);
                SetUpEvents(coll, gem);
            }

        }

        private static void SetUpEvents(List<GameEvent> coll, GameEventManager gem)
        {
            foreach (var ge in coll)
            {
                ge.EventHandler = gem;
                ge.DisableStart();
            }

            coll.Clear();
        }
    }
}
