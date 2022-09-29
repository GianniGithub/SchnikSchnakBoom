using GrandDevs.ExtremeScooling.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDevs.ExtremeScooling
{
    public interface ILocalizationManager
    {
        event Action<Enumerators.Language> LanguageWasChangedEvent;


        Dictionary<SystemLanguage, Enumerators.Language> SupportedLanguages { get; }
        Enumerators.Language CurrentLanguage { get; }
        Enumerators.Language DefaultLanguage { get; }

        void ApplyLocalization();

        void SetLanguage(Enumerators.Language language, bool forceUpdate = false);

        string GetUITranslation(string key);
    }
}