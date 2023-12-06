using UnityEngine;
namespace GellosGames
{
    public class Idle : Mode
    {
        public static Idle Universal
        {
            get
            {
                if (instance == null)
                {
                    instance = new Idle(null);
                }
                return instance;
            }
        }
        private static Idle instance;
        public override void Update()
        {
            // Do nothing
        }
        public Idle(MonoBehaviour mother) : base(mother)
        {
        }
    }
}