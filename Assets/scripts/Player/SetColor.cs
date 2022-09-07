using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GellosGames
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SetColor : PlayerEvent
    {
        public Material[] PlayerColors;
        public override void OnSpawn() 
        {
            var mesh = GetComponent<MeshRenderer>();
            mesh.material = PlayerColors[EventHandler.PlayerSlot];
        }
    }
}
