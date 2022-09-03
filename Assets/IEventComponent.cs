using System;
using UnityEngine;

namespace GellosGames
{
    public interface IEventComponent<IEventLeaf>
    {
        void RegisterListener(IEventLeaf listener);
        void UnregisterListener(IEventLeaf listener);
    }
    public interface IEventLeaf<IArgs> where IArgs : EventArgs
    {
        IEventComponent<IEventLeaf<IArgs>> EventParent { get; }
        void Initialisation();
        void OnEvent(MonoBehaviour sender, IArgs e);

        // Alles in einer Componete lösen, mit childs, so das immer jedes Event erweitert werden kann und besser getestet. für Parents muss man immer ein Mockup schreiben, daher besser immer alles in einer Componente
        // Also jede Compnente hat eine Liste an möglichen Listener
    }
}