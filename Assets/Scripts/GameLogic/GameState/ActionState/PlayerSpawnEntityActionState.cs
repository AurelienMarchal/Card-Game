namespace GameLogic
{
    namespace GameState
    {
        public class PlayerSpawnEntityActionState : PlayerActionState
        {
            public EntityState entitySpawned
            {
                get;
                set;
            }
            
            public uint tileNum{
                get;
                set;
            }
        }
    }
}