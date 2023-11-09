using UnityEngine;
namespace GellosGames
{
    public class HeadToPlayer : ForceMovement
    {
        public Transform Player;
        public HeadToPlayer(ConstantForce forceMover, NPCMode mother) : base(forceMover, mother)
        {
        }
        public override void Update()
        {
            var npcTransform = Npc.transform;
            var dir = (Player.position - npcTransform.position);
            dir.y = npcTransform.forward.y;
            npcTransform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }
}