namespace GameLogic{

    namespace UserAction{
        public class MoveEntityUserAction : EntityUserAction
        {
            public Direction direction { get; private set;}
            public MoveEntityUserAction(uint playerNum, uint entityNum, Direction direction) : base(playerNum, entityNum){
                this.direction = direction;
            }
        }
    }
}