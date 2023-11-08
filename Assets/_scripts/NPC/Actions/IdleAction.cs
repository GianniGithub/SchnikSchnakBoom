namespace GellosGames
{
    public class IdleAction : NPCModeBehaviour
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
        public IdleAction(NPCMode mother) : base(mother)
        {
        }
    }
}