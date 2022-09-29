using GrandDevs.ExtremeScooling.Common;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDevs.ExtremeScooling
{
    public class GamePage : IUIElement
    {
        private GameObject _selfPage;

        private IUIManager _uiManager;

        private ILoadObjectsManager _loadObjectsManager;
        private ILocalizationManager _localizationManager;
        private IDataManager _dataManager;


        public void Init()
        {
            _uiManager = GameClient.Get<IUIManager>();
            _loadObjectsManager = GameClient.Get<ILoadObjectsManager>();
            _localizationManager = GameClient.Get<ILocalizationManager>();
            _dataManager = GameClient.Get<IDataManager>();
            _selfPage = MonoBehaviour.Instantiate(_loadObjectsManager.GetObjectByPath<GameObject>("Prefabs/UI/GamePage"));
            _selfPage.transform.SetParent(_uiManager.Canvas.transform, false);
            _uiManager.ApplyTopBorderOffsetToUIElement(_selfPage);

            Hide();
        }

        public void Hide()
        {
            _selfPage.SetActive(false);
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
    }
}
