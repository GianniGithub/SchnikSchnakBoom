using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    public class CamaraBrainInputHandling : MonoBehaviour
    {
        private CinemachineBrain brain;
        public string axisName;

        void Start()
        {
            CinemachineCore.GetInputAxis = GetAxisCustom;
            brain = Camera.main.GetComponent<CinemachineBrain>();
        }

        private float GetAxisCustom(string axisName)
        {
            this.axisName = axisName;
            return Input.GetAxis(axisName);
        }

    }
}
