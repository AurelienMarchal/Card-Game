using System.Collections.Generic;

public class Hero : Entity{

    public Hero(Player player, EntityModel model, string name, Tile startingTile, Health startingHealth, Damage starttingAtkDamage, int startingMaxMovement, List<EntityEffect> permanentEffects, Direction startingDirection = Direction.North) : base(player, model, name, startingTile, startingHealth,  starttingAtkDamage, startingMaxMovement, permanentEffects, startingDirection){
    }

    public Hero(Player player, ScriptableHero scriptableHero, Tile startingTile, Direction startingDirection = Direction.North) : base(player, scriptableHero, startingTile, startingDirection){

    }

    public override bool TryToMove(Tile tile){

        var result = base.TryToMove(tile);

        return result;
        
    }

}

