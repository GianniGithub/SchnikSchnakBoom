using GrandDevs.ExtremeScooling.Common;
using System;

namespace GrandDevs.ExtremeScooling
{
    public interface IScenesManager
    {
        event Action<Enumerators.AppState> SceneForAppStateWasLoadedEvent;

        bool IsLoadedScene { get; set; }

        void ChangeScene(Enumerators.AppState appState); 
    }
}