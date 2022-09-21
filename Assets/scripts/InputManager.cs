using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace GellosGames
{
    public class InputManager : GameEvent
    {
        public static PlayerController AllController { get; private set; }
        // Start is called before the first frame update
        void Start()
        {
            AllController = new PlayerController();
            AllController.devices = new[] { (InputDevice)Keyboard.current, Mouse.current };
            AllController.Player1.Enable();

            var e = new InputManagerArgs() { AllController = AllController, Enabled = true };
            GameEvents.Instance.TriggerEvent(this, new GameEventArgs(GameActions.OnGamePadStateChange, e));
            //ControllEvents.devices = (ReadOnlyArray<InputDevice>)Gamepad.all ;
        }

    }
    public class InputManagerArgs : EventArgs
    {
        public PlayerController AllController { get; set; }
        public bool Enabled { get; set; }
    }
}
