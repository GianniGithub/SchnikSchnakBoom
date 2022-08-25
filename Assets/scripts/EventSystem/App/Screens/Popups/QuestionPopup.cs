using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDevs.ExtremeScooling
{
    public class QuestionPopup : IUIPopup
    {
        private GameObject _selfPage;

        private IUIManager _uiManager;

        private ILoadObjectsManager _loadObjectsManager;
        public GameObject Self => _selfPage;

        public void Init()
        {
            _uiManager = GameClient.Get<IUIManager>();
            _loadObjectsManager = GameClient.Get<ILoadObjectsManager>();

            _selfPage = MonoBehaviour.Instantiate(_loadObjectsManager.GetObjectByPath<GameObject>("Prefabs/UI/Popups/QuestionPopup"));
            _selfPage.transform.SetParent(_uiManager.Canvas.transform, false);
            _uiManager.ApplyTopBorderOffsetToUIElement(_selfPage);

            Hide();
        }

        public void Hide()
        {
            _selfPage.SetActive(false);
        }
        
        public void Show(object data)
        {
            _selfPage.SetActive(true);
        }

        public void Show()
        {
            _selfPage.SetActive(true);
        }

        public void Update()
        {
        }

        public void Dispose()
        {
        }

        public void SetMainPriority()
        {
        }
    }
}