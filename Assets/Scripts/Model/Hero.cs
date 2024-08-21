using System.Collections.Generic;

public class Hero : Entity{

    public Hero(Player player, EntityModel model, string name, Tile startingTile, Health startingHealth, Damage starttingAtkDamage, int startingMaxMovement, List<EntityEffect> permanentEffects, Direction startingDirection = Direction.North, Weapon weapon = Weapon.noWeapon) : base(player, model, name, startingTile, startingHealth, startingMaxMovement, permanentEffects, startingDirection, weapon){
        movementLeft = maxMovement;
    }

    public Hero(Player player, ScriptableHero scriptableHero, Tile startingTile, Direction startingDirection = Direction.North, Weapon weapon = Weapon.noWeapon) : base(player, scriptableHero, startingTile, startingDirection){
        movementLeft = maxMovement;
    }

    public override bool TryToMove(Tile tile){

        var result = base.TryToMove(tile);

        return result;
        
    }

    public override bool TryToCreateEntityPayHeartCostAction(HeartType[] heartCost, out EntityPayHeartCostAction entityPayHeartCostAction, Action requiredAction = null){
        entityPayHeartCostAction = new EntityPayHeartCostAction(this, heartCost, requiredAction);
        var canPayHeartCost = CanPayHeartCost(heartCost);
        if(canPayHeartCost){
            Game.currentGame.PileAction(entityPayHeartCostAction);
        }

        return canPayHeartCost;
    }

    public override bool TryToPayHeartCost(HeartType[] heartCost){
        return health.TryToPayHeartCost(heartCost, false);
    }

    public override bool CanPayHeartCost(HeartType[] heartCost){
        var result = health.CanPayHeartCost(heartCost, out bool willDie);
        return result && !willDie;
    }

}

