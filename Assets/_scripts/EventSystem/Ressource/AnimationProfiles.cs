using GellosGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    [CreateAssetMenu(fileName = "TutorialTexts", menuName = "ScriptableObjects/Profiles/Animation", order = 1)]
    public class AnimationProfiles : Profile
    {
        public AnimationProfil MenueButtons;
        public AnimationProfil Tutorial;
        public AnimationProfil DailyTreeButtons;

    }
    [Serializable]
    public class AnimationProfil
	{
        public AnimationCurve curve;
        public float time;

    }
}
