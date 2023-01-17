using System;
using GellosGames;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GellosGames
{
    public enum GradiantType
    {
        Bright,
        Dark
    }
    [CreateAssetMenu(fileName = "TutorialTexts", menuName = "ScriptableObjects/Profiles/Color", order = 1)]
    public class ColorProfile : Profile
    {
        public Gradient Scalar;
        public Gradient WeeklyScalar;
        public Color PipeColor;

        public Gradient GetColorType(GradiantType colorType) => colorType switch
        {

            GradiantType.Bright => Scalar,
            GradiantType.Dark => WeeklyScalar,
            _ => throw new ArgumentOutOfRangeException(nameof(colorType), colorType, null)
        };
    }
}
