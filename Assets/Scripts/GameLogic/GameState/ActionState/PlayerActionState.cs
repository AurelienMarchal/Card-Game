namespace GameLogic
{
    namespace GameState
    {



        public class PlayerActionState : ActionState
        {
            public uint playerNum { get; set; }

            public PlayerActionState(uint playerNum)
            {
                this.playerNum = playerNum;
            }

            public PlayerActionState()
            {
                
            }
        }
    }
}