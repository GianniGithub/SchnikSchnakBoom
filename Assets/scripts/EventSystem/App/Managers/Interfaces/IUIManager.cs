using GrandDevs.ExtremeScooling.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace GrandDevs.ExtremeScooling
{
    public interface IUIManager
    {
        GameObject Canvas { get; set; }
        CanvasScaler CanvasScaler { get; set; }
        bool HasTopBorder { get; }
        IUIElement CurrentPage { get; set; }
        void SetPage<T>(bool hideAll = false) where T : IUIElement;
        void DrawPopup<T>(object message = null, bool setMainPriority = false) where T : IUIPopup;
        void HidePopup<T>() where T : IUIPopup;
        IUIPopup GetPopup<T>() where T : IUIPopup;
        IUIElement GetPage<T>() where T : IUIElement;

        void HideAllPages();
        void HideAllPopups();

        void ApplyTopBorderOffsetToUIElement(GameObject uiElement, bool force = false);
    }
}