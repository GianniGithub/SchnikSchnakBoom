using GrandDevs.ExtremeScooling.Common;
using GrandDevs.ExtremeScooling.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GrandDevs.ExtremeScooling
{
    public class UIManager : IService, IUIManager
    {
        private const float TopBorderOffset = 100f;
        private const float FadeInOutDuration = 1f;

        private List<IUIElement> _uiPages;
        private List<IUIPopup> _uiPopups;

        public IUIElement CurrentPage { get; set; }

        public CanvasScaler CanvasScaler { get; set; }
        public GameObject Canvas { get; set; }

        public bool HasTopBorder { get; private set; }

        private ILoadObjectsManager _loadObjectsManager;

        public void Dispose()
        {
            foreach (var page in _uiPages)
                page.Dispose();

            foreach (var popup in _uiPopups)
                popup.Dispose();
        }

        public void Init()
        {
            Canvas = GameObject.Find("Canvas");
            CanvasScaler = Canvas.GetComponent<CanvasScaler>();

            _loadObjectsManager = GameClient.Get<ILoadObjectsManager>();

#if UNITY_IOS
             HasTopBorder = UnityEngine.iOS.Device.generation >= UnityEngine.iOS.DeviceGeneration.iPhoneX;
#endif

            _uiPages = new List<IUIElement>();
            _uiPages.Add(new MainPage());
            _uiPages.Add(new GamePage());    

            foreach (var page in _uiPages)
                page.Init();

            _uiPopups = new List<IUIPopup>();
            _uiPopups.Add(new QuestionPopup());

            foreach (var popup in _uiPopups)
                popup.Init();
        }

        public void Update()
        {
            foreach (var page in _uiPages)
                page.Update();

            foreach (var popup in _uiPopups)
                popup.Update();
        }

        public void HideAllPages()
        {
            foreach (var _page in _uiPages)
            {
                _page.Hide();
            }
        }

        public void HideAllPopups()
        {
            foreach (var _popup in _uiPopups)
            {
                _popup.Hide();
            }
        }

        public void SetPage<T>(bool hideAll = false) where T : IUIElement
        {
            if (hideAll)
            {
                HideAllPages();
            }
            else
            {
                if (CurrentPage != null)
                    CurrentPage.Hide();
            }

            foreach (var _page in _uiPages)
            {
                if (_page is T)
                {
                    CurrentPage = _page;
                    break;
                }
            }
            CurrentPage.Show();
        }

        public void DrawPopup<T>(object message = null, bool setMainPriority = false) where T : IUIPopup
        {
            IUIPopup popup = null;
            foreach (var _popup in _uiPopups)
            {
                if (_popup is T)
                {
                    popup = _popup;
                    break;
                }
            }

            if (setMainPriority)
                popup.SetMainPriority();

            if (message == null)
                popup.Show();
            else
                popup.Show(message);
        }

        public void HidePopup<T>() where T : IUIPopup
        {
            foreach (var _popup in _uiPopups)
            {
                if (_popup is T)
                {
                    _popup.Hide();
                    break;
                }
            }
        }

        public IUIPopup GetPopup<T>() where T : IUIPopup
        {
            IUIPopup popup = null;
            foreach (var _popup in _uiPopups)
            {
                if (_popup is T)
                {
                    popup = _popup;
                    break;
                }
            }

            return popup;
        }

        public IUIElement GetPage<T>() where T : IUIElement
        {
            IUIElement page = null;
            foreach (var _page in _uiPages)
            {
                if (_page is T)
                {
                    page = _page;
                    break;
                }
            }

            return page;
        }

        public void ApplyTopBorderOffsetToUIElement(GameObject uiElement, bool force = false)
        {
            if (!HasTopBorder && !force)
                return;

            RectTransform transform = uiElement.GetComponent<RectTransform>();
            transform.offsetMax = new Vector2(transform.offsetMax.x, -TopBorderOffset);

            // todo implement vertical offset
        }
    }
}