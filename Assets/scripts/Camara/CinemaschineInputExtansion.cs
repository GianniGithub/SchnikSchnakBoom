using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace GellosGames
{
    public class CinemaschineInputExtansion : CinemachineExtension
    {
        public Vector3 Rotation;
        public float AxeA, AxeB;

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            Rotation = state.RawOrientation.eulerAngles;
            state.RawOrientation = Quaternion.identity;
            var input = vcam.GetInputAxisProvider();
            AxeA = input.GetAxisValue(0);
            AxeB = input.GetAxisValue(1);
        }
        protected override void Awake()
        {
            base.Awake();
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
