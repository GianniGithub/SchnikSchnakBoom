//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/input/PlayerController.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerController : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerController"",
    ""maps"": [
        {
            ""name"": ""Player1"",
            ""id"": ""18b2d364-6e65-40b8-932b-ecfa53e4b677"",
            ""actions"": [
                {
                    ""name"": ""MainShoot"",
                    ""type"": ""Button"",
                    ""id"": ""9f38fb21-947d-465c-8523-32919f290173"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""movment"",
                    ""type"": ""Value"",
                    ""id"": ""6b95cfbc-5f51-43e3-8efa-3fcf00f85f50"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a63b33c2-6688-437e-90d9-1b926965b635"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MainShoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""858ed4bc-da8c-4b9d-98c4-8829d2d5483b"",
                    ""path"": ""<XInputController>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movment"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player1
        m_Player1 = asset.FindActionMap("Player1", throwIfNotFound: true);
        m_Player1_MainShoot = m_Player1.FindAction("MainShoot", throwIfNotFound: true);
        m_Player1_movment = m_Player1.FindAction("movment", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player1
    private readonly InputActionMap m_Player1;
    private IPlayer1Actions m_Player1ActionsCallbackInterface;
    private readonly InputAction m_Player1_MainShoot;
    private readonly InputAction m_Player1_movment;
    public struct Player1Actions
    {
        private @PlayerController m_Wrapper;
        public Player1Actions(@PlayerController wrapper) { m_Wrapper = wrapper; }
        public InputAction @MainShoot => m_Wrapper.m_Player1_MainShoot;
        public InputAction @movment => m_Wrapper.m_Player1_movment;
        public InputActionMap Get() { return m_Wrapper.m_Player1; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Player1Actions set) { return set.Get(); }
        public void SetCallbacks(IPlayer1Actions instance)
        {
            if (m_Wrapper.m_Player1ActionsCallbackInterface != null)
            {
                @MainShoot.started -= m_Wrapper.m_Player1ActionsCallbackInterface.OnMainShoot;
                @MainShoot.performed -= m_Wrapper.m_Player1ActionsCallbackInterface.OnMainShoot;
                @MainShoot.canceled -= m_Wrapper.m_Player1ActionsCallbackInterface.OnMainShoot;
                @movment.started -= m_Wrapper.m_Player1ActionsCallbackInterface.OnMovment;
                @movment.performed -= m_Wrapper.m_Player1ActionsCallbackInterface.OnMovment;
                @movment.canceled -= m_Wrapper.m_Player1ActionsCallbackInterface.OnMovment;
            }
            m_Wrapper.m_Player1ActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MainShoot.started += instance.OnMainShoot;
                @MainShoot.performed += instance.OnMainShoot;
                @MainShoot.canceled += instance.OnMainShoot;
                @movment.started += instance.OnMovment;
                @movment.performed += instance.OnMovment;
                @movment.canceled += instance.OnMovment;
            }
        }
    }
    public Player1Actions @Player1 => new Player1Actions(this);
    public interface IPlayer1Actions
    {
        void OnMainShoot(InputAction.CallbackContext context);
        void OnMovment(InputAction.CallbackContext context);
    }
}