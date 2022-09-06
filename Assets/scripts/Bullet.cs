using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Gianni.Helper;

namespace GellosGames
{

    public interface Bullet
    {
        public PlayerID OwnerId { get; set; }
        public Transform ExplosionPrefap { get; }
        public AnimationCurve ExplosionAnimation { get; }
    }

}