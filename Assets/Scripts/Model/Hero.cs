public class Hero : Entity{

    public Hero(EntityModel model, string name, Tile startingTile, Health startingHealth, Player player, int starttingAtk = 0, Direction startingDirection = Direction.North) : base(model, name, startingTile, startingHealth, player, starttingAtk, startingDirection){
    }

    public override bool TryToMove(Tile tile){

        var result = base.TryToMove(tile);

        return result;
        
    }

}

