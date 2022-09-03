using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    public class Artillery : MonoBehaviour
    {
        public PlayersControlls Controlls;
        ArtillerieShoot shooter;
        void Awake()
        {
            shooter = GetComponent<ArtillerieShoot>();
        }
        private void OnEnable()
        {
            Controlls.OnLookStateSwitch += shooter.Controlls_OnLookStateSwitch;
            Controlls.ControllEvents.Player1.MainShoot.performed += shooter.OnShootBullet;
            Debug.Log("Artill");
        }
        private void OnDisable()
        {
            Controlls.OnLookStateSwitch += shooter.Controlls_OnLookStateSwitch;
            Controlls.ControllEvents.Player1.MainShoot.performed -= shooter.OnShootBullet;
        }
    } 
}
