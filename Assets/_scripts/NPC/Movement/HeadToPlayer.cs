using UnityEngine;
namespace GellosGames
{
    public class HeadToPlayer: ForceMovement
    {

        public HeadToPlayer(ConstantForce forceMover, NPCMode mother) : base(forceMover, mother)
        {
        }
        public override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}