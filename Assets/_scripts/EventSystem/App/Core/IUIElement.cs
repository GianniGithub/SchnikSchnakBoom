using UnityEngine;

namespace GrandDevs.ExtremeScooling
{
    public interface IUIElement
    {
        void Init();
        void Show();
        void Hide();
        void Update();
        void Dispose();
    }
}