using System.Collections.Generic;
using UnityEngine;

namespace GameLogic{

    using GameAction;
    using GameEffect;
    using GameBuff;
    using GameState;
    public class Hero : Entity{

        public Hero(
            Player player,
            EntityModel model,
            string name,
            Tile startingTile,
            Health startingHealth,
            int startingMaxMovement,
            Damage baseAtkDamage,
            int baseRange,
            Cost baseCostToAtk,
            Cost baseCostToMove,
            Direction startingDirection = Direction.North,
            Weapon weapon = Weapon.noWeapon) :
            base(player, model, name, startingTile, startingHealth, startingMaxMovement, baseAtkDamage, baseRange, baseCostToAtk, baseCostToMove, startingDirection)
        {
            movementLeft = maxMovement;
            this.weapon = weapon;
        }

        public Weapon weapon{
            get;
            protected set;
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

        protected override Cost CalculateCostToAtk(){
            return weapon != null ? weapon.costToUse : Cost.noCost;
        }

        protected override int CalculateRange(){
            return (weapon != null ? weapon.range : 0) + base.CalculateRange();
        }

        protected override Damage CalculateAtkDamage(){
            return (weapon != null ? weapon.atkDamage : new Damage(0)) + base.CalculateAtkDamage();
        }

        /*
        public override bool CanAttack(Entity entity)
        {

            if (entity == this)
            {
                return false;
            }

            if (weapon == Weapon.noWeapon)
            {
                return false;
            }

            if (entity.currentTile.gridX != currentTile.gridX && entity.currentTile.gridY != currentTile.gridY)
            {
                return false;
            }

            if (entity.currentTile.Distance(currentTile) > range)
            {
                return false;
            }

            if (DirectionsExtensions.FromCoordinateDifference(
                entity.currentTile.gridX - currentTile.gridX,
                entity.currentTile.gridY - currentTile.gridY
                ) != direction)
            {
                return false;
            }

            return true;
        }

        public override bool CanAttackByChangingDirection(Entity entity){

            if(entity == this){
                return false;
            }

            if(weapon == Weapon.noWeapon){
                return false;
            }

            if(entity.currentTile.gridX != currentTile.gridX && entity.currentTile.gridY != currentTile.gridY){
                return false;
            }

            if(entity.currentTile.Distance(currentTile) > weapon.range){
                return false;
            }

            if(!CanChangeDirection(DirectionsExtensions.FromCoordinateDifference(
                entity.currentTile.gridX - currentTile.gridX,
                entity.currentTile.gridY - currentTile.gridY
                ))){
                    return false;
            }

            return true;
        }

        public override bool CanPayAtkCost(){
            if(weapon == null){
                return false;
            }

            return CanPayHeartCost(weapon.costToUse.heartCost) && CanUseMovement(weapon.costToUse.mouvementCost);
        }

        
        public override void GetTilesAndEntitiesAffectedByAtk(out Entity[] entitiesAffected, out Tile[] tilesAffected)
        {
            if (weapon == Weapon.noWeapon)
            {
                entitiesAffected = new Entity[0];
                tilesAffected = new Tile[0];
                return;
            }

            weapon.GetTilesAndEntitiesAffectedByAtk(currentTile, direction, out entitiesAffected, out tilesAffected);

        }
        */


        public HeroState ToHeroState(){
            HeroState heroState = new HeroState();
            heroState.num = num;
            heroState.model = model;
            heroState.name = name;
            heroState.currentTileNum = currentTile.num;
            heroState.healthState = health.ToHealthState();
            heroState.direction = direction;
            heroState.movementLeft = movementLeft;
            heroState.costToAtkState = costToAtk.ToCostState();
            heroState.baseCostToAtkState = baseCostToAtk.ToCostState();
            heroState.range = range;
            heroState.baseRange = baseRange;
            heroState.atkDamageState = atkDamage.ToDamageState();
            heroState.baseAtkDamageState = baseAtkDamage.ToDamageState();
            heroState.maxMovement = maxMovement;
            heroState.costToMoveState = costToMove.ToCostState();

            if (weapon != Weapon.noWeapon)
            {
                heroState.weaponState = weapon.ToWeaponState();
            }
            

            heroState.effectStates = new List<EffectState>();
            heroState.buffStates = new List<BuffState>();

            foreach (Effect effect in effects){
                heroState.effectStates.Add(effect.ToEffectState());
            }

            foreach (Buff buff in buffs){
                heroState.buffStates.Add(buff.ToBuffState());
            }


            return heroState;
        }


    }

}