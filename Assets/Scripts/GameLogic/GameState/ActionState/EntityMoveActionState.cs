namespace GameLogic
{
    namespace GameState
    {
        public class EntityMoveActionState : EntityActionState
        {
            public uint startTileNum { get; set; }
            public uint endTileNum { get; set; }


            public EntityMoveActionState()
            {
                
            }
        }
    }
}