using GrandDevs.ExtremeScooling.Common;
using UnityEngine;
using UnityEngine.UI;

namespace GrandDevs.ExtremeScooling
{
    public class MainPage : IUIElement
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
            _selfPage = MonoBehaviour.Instantiate(_loadObjectsManager.GetObjectByPath<GameObject>("Prefabs/UI/MainPage"));
            _selfPage.transform.SetParent(_uiManager.Canvas.transform, false);
            _uiManager.ApplyTopBorderOffsetToUIElement(_selfPage);


            UpdateLocalization();
            _localizationManager.LanguageWasChangedEvent += _localizationManager_LanguageWasChangedEvent;

			Hide();
		}

		private void _localizationManager_LanguageWasChangedEvent(Enumerators.Language obj)
        {
            UpdateLocalization();

        }        
        
        public void OnClickRegisterHandler()
        {

        }

        public void OnClickRestoreHandler()
        {

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

        private void UpdateLocalization()
        {

        }

        public void Dispose()
        {

        }
    }
}