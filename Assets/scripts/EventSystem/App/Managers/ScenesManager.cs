﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using GrandDevs.ExtremeScooling.Common;
using System;

namespace GrandDevs.ExtremeScooling
{
    public sealed class ScenesManager : IService, IScenesManager
    {
        public event Action<Enumerators.AppState> SceneForAppStateWasLoadedEvent;

        private IUIManager _uIManager;
        private IAppStateManager _appStateManager;

        private bool _isLoadingScenesAsync = true;

        public static string CurrentSceneName { get; private set; }
        public int SceneLoadingProgress { get; private set; } 

        public bool IsLoadedScene { get; set; }

        public void Dispose()
        {
            MainApp.Instance.OnLevelWasLoadedEvent -= OnLevelWasLoadedHandler;
        }

        public void Init()
        {
            MainApp.Instance.OnLevelWasLoadedEvent += OnLevelWasLoadedHandler;

            _uIManager = GameClient.Get<IUIManager>();
            _appStateManager = GameClient.Get<IAppStateManager>();

            OnLevelWasLoadedHandler(null);
        }

        public void Update()
        {
        }

        public void ChangeScene(Enumerators.AppState appState)
        {
            IsLoadedScene = false;

            string sceneName = string.Empty;
            switch (appState)
            {
                case Enumerators.AppState.AppStart:
                    sceneName = appState.ToString();
                    break;
            }

            if (CurrentSceneName == sceneName)
            {
                SceneForAppStateWasLoadedEvent?.Invoke(_appStateManager.AppState);
                return;
            }

            if (!_isLoadingScenesAsync)
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                MainApp.Instance.StartCoroutine(LoadLevelAsync(sceneName));
            }
        }

        private void OnLevelWasLoadedHandler(object param)
        {
            CurrentSceneName = SceneManager.GetActiveScene().name;

            IsLoadedScene = true;
            SceneLoadingProgress = 0;

            SceneForAppStateWasLoadedEvent?.Invoke(_appStateManager.AppState);
        }

        private IEnumerator LoadLevelAsync(string levelName)
        {
            yield return new WaitForSeconds(0.25f);

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelName);

            while (!asyncOperation.isDone)
            {
                SceneLoadingProgress = Mathf.RoundToInt(asyncOperation.progress * 100f);
                yield return null;
            }
        }
    }
}