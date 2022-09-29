using GrandDevs.ExtremeScooling.Common;
using UnityEngine;


namespace GrandDevs.ExtremeScooling
{
    public sealed class AppStateManager : IService, IAppStateManager
    {
        private IUIManager _uiManager;

        public Enumerators.AppState AppState { get; set; } = Enumerators.AppState.Undefined;

        public void Dispose()
        {

        }

        public void Init()
        {
            _uiManager = GameClient.Get<IUIManager>();
        }

        public void Update()
        {

        }

        public void ChangeAppState(Enumerators.AppState stateTo)
        {
            if (AppState == stateTo)
                return;

            AppState = stateTo;

            switch (stateTo)
            {
                case Enumerators.AppState.Game:
                    _uiManager.SetPage<GamePage>();
                    GameClient.Get<IGameplayManager>().StartGameplay();
                    break;
                case Enumerators.AppState.Main:
                    GameClient.Get<IGameplayManager>().StopGameplay();
                    _uiManager.SetPage<MainPage>();
                    break;
            }
        }

        public void PauseGame(bool enablePause)
        {
            if (enablePause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
}