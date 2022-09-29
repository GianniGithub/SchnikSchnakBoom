using System.Collections;
using System;
using UnityEngine;

namespace GellosGames
{

    internal interface IInputDataCollector : IInputData
    {
        float UserInput { get; set; }
    }
    internal interface IInputDataCollector<TData> : IInputData
    {
        public TData GetUserInput();
    }
    internal interface IInputData
    {
        void GetUserInputData(DataCollector inputs);
        void SetUserInputData(DataCollector inputs);
    }
    [Serializable]
    public abstract class DataCollector { }
}
