using UnityEngine;
namespace GellosGames
{
    public class IdleAction : Mode
    {
        public static IdleAction Universal
        {
            get
            {
                if (instance == null)
                {
                    instance = new IdleAction(null);
                }
                return instance;
            }
        }
        private static IdleAction instance;
        public override void Update()
        {
            // Do nothing
        }
        public IdleAction(MonoBehaviour mother) : base(mother)
        {
        }
    }
}