namespace GrandDevs.ExtremeScooling
{
    public class GameClient : ServiceLocatorBase
    {
        private static object _sync = new object();

        private static GameClient _Instance;
        public static GameClient Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_sync)
                    {
                        _Instance = new GameClient();
                    }
                }
                return _Instance;
            }
        }

        public static bool IsDebugMode = false; //change to 'false' for release

        /// <summary>
        /// Initializes a new instance of the <see cref="GameClient"/> class.
        /// </summary>
        internal GameClient()
            : base()
        {
#if UNITY_EDITOR
            IsDebugMode = true;
#endif
            //AddService<IDataManager>(new DataManager());
            AddService<ISoundManager>(new SoundManager());
            AddService<IInputManager>(new InputManager());
            AddService<ILoadObjectsManager>(new LoadObjectsManager());
            AddService<IAppStateManager>(new AppStateManager());
            AddService<IScenesManager>(new ScenesManager());
            AddService<ILocalizationManager>(new LocalizationManager());
            AddService<IUIManager>(new UIManager());
            AddService<IGameplayManager>(new GameplayManager());
            AddService<INetworkManager>(new NetworkManager());
        }

        public static T Get<T>()
        {
            return Instance.GetService<T>();
        }
    }
}