public class Hero : Entity{

    public Hero(Player player, EntityModel model, string name, Tile startingTile, Health startingHealth, Damage starttingAtkDamage, Direction startingDirection = Direction.North) : base(player, model, name, startingTile, startingHealth,  starttingAtkDamage, startingDirection){
    }

    public override bool TryToMove(Tile tile){

        var result = base.TryToMove(tile);

        return result;
        
    }

}

