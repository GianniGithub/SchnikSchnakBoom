using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace GellosGames
{
    public class CinemachineInputOverride : CinemachineInputProvider
    {
        private Vector2 sensitivity;

        public override float GetAxisValue(int axis)
        {

            switch (axis)
            {
                case 0: return 1;
                case 1: return 1;
                case 2: return 0;
            }
            return 0;
        }
    }
}
