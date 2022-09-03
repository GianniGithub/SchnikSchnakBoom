using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerController;

namespace GellosGames
{
    public enum Weapen
    {
        unknown = 0,
        none = 1,
        Gun,
        Artillery,
        Rocket,
    }
    public class PlayersControlls : PlayerEvent, IPlayer1Actions
    {
        public GameObject[] Weapons;
        public event Action<bool, Vector2> OnLookStateSwitch;
        public float Speed;
        Vector3 nextMove;
        Rigidbody rb;
        public PlayerController ControllEvents;
        public int PlayerNum;
        private InputAction moveAction;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            ControllEvents = new PlayerController();

            moveAction = ControllEvents.Player1.movment;

            try
            {
                ControllEvents.devices = new[] { Gamepad.all[PlayerNum] };
            }
            catch (Exception)
            {
                return;
            }
            ControllEvents.Player1.looking.performed += OnLooking;
            ControllEvents.Player1.Artillery.performed += OnArtillery;
            ControllEvents.Player1.MiniGun.performed += OnMiniGun;
            ControllEvents.Player1.Rocket.performed += OnRocket;
            ControllEvents.Player1.Enable();

            PlayerEvents.CallPlayerAction(this, PlayerActions.PlayerControllerEventsRegisterd, PlayerEventInfos.general);

        }

        void FixedUpdate()
        {
            var moveInput = moveAction.ReadValue<Vector2>();
            nextMove = new Vector3(moveInput.x, 0, moveInput.y);
            rb.AddForce(nextMove * Speed, ForceMode.Force);
            nextMove = Vector3.zero;
        }

        public void OnLooking(InputAction.CallbackContext context)
        {
            var dPad = context.ReadValue<Vector2>();
            if (dPad == Vector2.zero)
            {
                OnLookStateSwitch?.Invoke(false, dPad);
                return;
            }
            else
            {
                OnLookStateSwitch?.Invoke(true, dPad);
            }

            float heading = Mathf.Atan2(dPad.x, dPad.y);
            transform.rotation = Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0f);


        }

        public void OnMainShoot(InputAction.CallbackContext context)
        {

        }

        public void OnMovment(InputAction.CallbackContext context)
        {

        }

        public void OnMiniGun(InputAction.CallbackContext context)
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                Weapons[i].SetActive(false);
            }
            Weapons[0].SetActive(true);

            var e = PlayerEventInfos.general;
            e.Current = Weapen.Gun;
            PlayerEvents.CallPlayerAction(this, PlayerActions.WeapenSwitch, e);
        }

        public void OnArtillery(InputAction.CallbackContext context)
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                Weapons[i].SetActive(false);
            }
            Weapons[1].SetActive(true);

            var e = PlayerEventInfos.general;
            e.Current = Weapen.Artillery;
            PlayerEvents.CallPlayerAction(this, PlayerActions.WeapenSwitch, e);
        }

        public void OnRocket(InputAction.CallbackContext context)
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                Weapons[i].SetActive(false);
            }
            Weapons[2].SetActive(true);

            var e = PlayerEventInfos.general;
            e.Current = Weapen.Rocket;
            PlayerEvents.CallPlayerAction(this, PlayerActions.WeapenSwitch, e);
        }

        public override void OnPlayerActionEvent(MonoBehaviour sender, PlayerActions action, PlayerEventInfos e)
        {

        }
    } 

}
