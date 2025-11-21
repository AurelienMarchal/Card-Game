
using System;
using System.Collections.Generic;
using UnityEngine;


namespace GameLogic{

    using GameAction;
    using GameEffect;
    using GameBuff;
    using GameState;
    public class Entity
    {
        public EntityModel model{
            get;
            protected set;
        }

        public uint num{
            get;
            //Temp, num has to be set in constructor
            set;
        }

        public string name{
            get;
            protected set;
        }

        public Tile currentTile{
            get;
            protected set;
        }

        public Health health{
            get;
            protected set;
        }

        public Direction direction{
            get;
            protected set;
        }

        public Player player{
            get;
            protected set;
        }

        public int movementLeft{
            get;
            protected set;
        }

        public int range{
            get;
            protected set;
        }

        public int baseRange{
            get;
            protected set;
        }

        public Damage atkDamage
        {
            get;
            protected set;
        }

        public Damage baseAtkDamage{
            get;
            protected set;
        }
        
        public Cost costToAtk{
            get;
            protected set;
        }

        public Cost baseCostToAtk{
            get;
            protected set;
        }

        public Cost costToMove{
            get;
            protected set;
        }

        public Cost baseCostToMove{
            get;
            protected set;
        }
        

        public int maxMovement
        {
            get;
            protected set;
        }

        public Tile[] tilesToMoveTo
        {
            get
            {
                var dynamicList = new List<Tile>();
                var tileEast = Game.currentGame.board.NextTileInDirection(currentTile, Direction.East);
                if (CanMoveByChangingDirection(tileEast))
                {
                    dynamicList.Add(tileEast);
                }

                var tileWest = Game.currentGame.board.NextTileInDirection(currentTile, Direction.West);
                if (CanMoveByChangingDirection(tileWest))
                {
                    dynamicList.Add(tileWest);
                }

                var tileNorth = Game.currentGame.board.NextTileInDirection(currentTile, Direction.North);
                if (CanMoveByChangingDirection(tileNorth))
                {
                    dynamicList.Add(tileNorth);
                }

                var tileSouth = Game.currentGame.board.NextTileInDirection(currentTile, Direction.South);
                if (CanMoveByChangingDirection(tileSouth))
                {
                    dynamicList.Add(tileSouth);
                }

                return dynamicList.ToArray();
                
            }
        }

        public const Entity noEntity = null;
        public const int maxMovementCap = 10;

        public List<EntityEffect> effects{
            get;
            protected set;
        }

        public List<EntityBuff> buffs{
            get{
                var dynamicList = new List<EntityBuff>();
                if(tempBuffs != null){
                    dynamicList.AddRange(tempBuffs);
                }
                if(permanentBuffs != null){
                    dynamicList.AddRange(permanentBuffs);
                }
                return dynamicList;
            }
        }

        protected List<EntityBuff> tempBuffs{
            get;
            private set;
        }

        protected List<EntityBuff> permanentBuffs{
            get;
            private set;
        }

        public Entity(
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
            Direction startingDirection = Direction.North){
            this.player = player;
            this.model = model;
            this.name = name;
            currentTile = startingTile;
            health = startingHealth.Clone() as Health;
            maxMovement = startingMaxMovement;
            movementLeft = 0;
            direction = startingDirection;
            
            
            this.baseRange = baseRange;
            range = baseRange;
            this.baseAtkDamage = baseAtkDamage;
            atkDamage = new Damage(baseAtkDamage.amount);
            this.baseCostToAtk = baseCostToAtk;
            costToAtk = new Cost(baseCostToAtk.heartCost, baseCostToAtk.mouvementCost, baseCostToAtk.manaCost);
            this.baseCostToMove = baseCostToMove;
            costToMove = new Cost(baseCostToMove.heartCost, baseCostToMove.mouvementCost, baseCostToMove.manaCost);

            effects = new List<EntityEffect>();
            tempBuffs = new List<EntityBuff>();
            permanentBuffs = new List<EntityBuff>();
            AddDefaultPermanentEffects();
        }

        [Obsolete]
        protected virtual Cost CalculateCostToMove()
        {
            var weightedDownBuffCount = NumberOfBuffs<WeightedDownBuff>();
            return new Cost(mouvement: 1 + weightedDownBuffCount);
        }

        internal void TryToSetStartingTile(Tile tile)
        {
            if (currentTile == Tile.noTile)
            {
                currentTile = tile;
            }
        }

        public virtual bool TryToCreateEntityMoveAction(Tile tile, Action requiredAction, out EntityMoveAction entityMoveAction)
        {
            var newdirection = DirectionsExtensions.FromCoordinateDifference(tile.gridX - currentTile.gridX, tile.gridY - currentTile.gridY);
            TryToCreateEntityChangeDirectionAction(newdirection, requiredAction, out EntityChangeDirectionAction entityChangeDirectionAction);
            entityMoveAction = new EntityMoveAction(this, currentTile, tile, entityChangeDirectionAction);
            var result = CanMove(tile);
            if (result)
            {
                Game.currentGame.PileAction(entityMoveAction);
            }

            return result;
        }

        public virtual bool TryToMove(Tile tile){

            var result = CanMove(tile);
            if(result){
                Move(tile);
            }

            return result;

        }

        public virtual bool CanMove(Tile tile){
            if(tile == currentTile){
                return false;
            }

            if(NumberOfBuffs<EntityCannotMoveBuff>() > 0){
                return false;
            }

            if(Game.currentGame.board.GetEntityAtTile(tile) != Entity.noEntity){
                return false;
            }

            //Check move condition
            if(tile.gridX != currentTile.gridX && tile.gridY != currentTile.gridY){
                return false;
            }

            if(DirectionsExtensions.FromCoordinateDifference(tile.gridX - currentTile.gridX, tile.gridY - currentTile.gridY) != direction){
                return false;
            }

            var distance = tile.Distance(currentTile);

            if(distance > 1){
                return false;
            }

            return true;
        }

        public virtual bool CanMoveByChangingDirection(Tile tile){
            if (tile == Tile.noTile)
            {
                return false;
            }
            if (tile == currentTile)
            {
                return false;
            }

            if(Game.currentGame.board.GetEntityAtTile(tile) != Entity.noEntity){
                return false;
            }

            //Check move condition
            if(tile.gridX != currentTile.gridX && tile.gridY != currentTile.gridY){
                return false;
            }

            if(!CanChangeDirection(DirectionsExtensions.FromCoordinateDifference(tile.gridX - currentTile.gridX, tile.gridY - currentTile.gridY))){
                return false;
            }

            var distance = tile.Distance(currentTile);

            if(distance > 1){
                return false;
            }

            return true;
        }

        protected virtual void Move(Tile tile){
            currentTile = tile;
        }

        public virtual bool TryToCreateEntityChangeDirectionAction(Direction newDirection, Action requiredAction, out EntityChangeDirectionAction entityChangeDirectionAction){
            entityChangeDirectionAction = new EntityChangeDirectionAction(this, newDirection, requiredAction);
            var result = CanChangeDirection(newDirection);
            if(result){
                Game.currentGame.PileAction(entityChangeDirectionAction);
            }

            return result;
        }

        public virtual bool TryToChangeDirection(Direction newDirection){

            var result = CanChangeDirection(newDirection);
            if(result){
                ChangeDirection(newDirection);
            }

            return result;

        }

        public virtual bool CanChangeDirection(Direction newDirection)
        {
            var relativeDirection = DirectionsExtensions.RelativeDirectionBetweenDirections(direction, newDirection);
            //Temp
            return true;
        }

        protected virtual void ChangeDirection(Direction newDirection){
            direction = newDirection;
        }

        public virtual bool TryToTeleport(Tile tile){

            var result = CanTeleport(tile);
            if(result){
                Teleport(tile);
            }

            return result;

        }

        public virtual bool CanTeleport(Tile tile){
            if(tile == currentTile){
                return false;
            }

            if(Game.currentGame.board.GetEntityAtTile(tile) != Entity.noEntity){
                return false;
            }

            return true;
        }

        protected virtual void Teleport(Tile tile){
            currentTile = tile;
        }

        public bool TryToCreateEntityAttackAction(Entity entity,  out EntityAttackAction entityAttackAction, Action requiredAction = null){
            var newdirection = DirectionsExtensions.FromCoordinateDifference(entity.currentTile.gridX - currentTile.gridX, entity.currentTile.gridY - currentTile.gridY);
            TryToCreateEntityChangeDirectionAction(newdirection, requiredAction, out EntityChangeDirectionAction entityChangeDirectionAction);
            entityAttackAction = new EntityAttackAction(this, entity, requiredAction:entityChangeDirectionAction);
            var canAttack = CanAttack(entity);
            if(canAttack){
                Game.currentGame.PileAction(entityAttackAction);
            }

            return canAttack;
        }

        protected virtual Cost CalculateCostToAtk(){
            return baseCostToAtk;
        }

        protected virtual int CalculateRange(){
            var total = baseRange;

            foreach(var buff in buffs){
                if(buff is RangeBuff rangeBuff){
                    total += rangeBuff.amount;
                }
            }

            return total;
        }

        protected virtual Damage CalculateAtkDamage(){

            var total = baseAtkDamage.amount;

            foreach(var buff in buffs){
                if(buff is AtkBuff atkBuff){
                    total += atkBuff.amount;
                }
            }


            return new Damage(total);
        }

        public virtual bool CanAttack(Entity entity){

            if(entity == this){
                
                return false;
            }

            if(entity.currentTile.gridX != currentTile.gridX && entity.currentTile.gridY != currentTile.gridY){
                
                return false;
            }

            if(entity.currentTile.Distance(currentTile) > range){
                
                return false;
            }

            if(DirectionsExtensions.FromCoordinateDifference(
                entity.currentTile.gridX - currentTile.gridX,
                entity.currentTile.gridY - currentTile.gridY
                ) != direction){
                    
                    return false;
            }

            return true;
        }

        public virtual bool CanAttackByChangingDirection(Entity entity){

            if(entity == this){
                return false;
            }

            if(entity.currentTile.gridX != currentTile.gridX && entity.currentTile.gridY != currentTile.gridY){
                return false;
            }

            if(entity.currentTile.Distance(currentTile) > range){
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

        

        public virtual void GetTilesAndEntitiesAffectedByAtk(out Entity[] entitiesAffected, out Tile[] tilesAffected){
            
            var entityInFront = Game.currentGame.board.GetFirstEntityInDirectionWithRange(
                Game.currentGame.board.NextTileInDirection(currentTile, direction), 
                direction, 
                range,
                out tilesAffected
            );

            if (entityInFront == Entity.noEntity)
            {
                entitiesAffected = new Entity[0];
                return;
            }
            
            entitiesAffected = new Entity[1]{entityInFront};

        }

        public bool TakeDamage(Damage damage)
        {
            Debug.Log($"{this} taking {damage} damage");
            var isDead = health.TakeDamage(damage);
            Debug.Log($"{this} current {health}");
            return isDead;
        }
        
        public virtual bool CanPayCost(Cost cost){
            return CanPayHeartCost(cost.heartCost) && CanUseMovement(cost.mouvementCost);
        }

        public bool TryToCreateEntityUseMovementAction(int movement,  out EntityUseMovementAction entityUseMovementAction, Action requiredAction = null){
            entityUseMovementAction = new EntityUseMovementAction(movement, this, requiredAction);
            var canUseMovement = CanUseMovement(movement);
            if(canUseMovement){
                Game.currentGame.PileAction(entityUseMovementAction);
            }

            return canUseMovement;
        }

        public bool TryToUseMovement(int movement){
            var canUseMovement = CanUseMovement(movement);
            if(canUseMovement){
                UseMovement(movement);
            }
            return canUseMovement;
        }

        public bool CanUseMovement(int movement){
            return movement <= movementLeft && movement >= 0;
        }

        private void UseMovement(int movement){
            movementLeft = Math.Clamp(movementLeft - movement, 0, maxMovement);
        }

        public virtual bool TryToCreateEntityPayHeartCostAction(HeartType[] heartCost, out EntityPayHeartCostAction entityPayHeartCostAction, Action requiredAction = null){
            entityPayHeartCostAction = new EntityPayHeartCostAction(this, heartCost, requiredAction);
            var canPayHeartCost = CanPayHeartCost(heartCost);
            if(canPayHeartCost){
                Game.currentGame.PileAction(entityPayHeartCostAction);
            }

            return canPayHeartCost;
        }

        public virtual bool TryToPayHeartCost(HeartType[] heartCost){
            return health.TryToPayHeartCost(heartCost, true);
        }

        public virtual bool CanPayHeartCost(HeartType[] heartCost){
            return health.CanPayHeartCost(heartCost, out _);
        }

        public bool TryToIncreaseMaxMovement(){
            var canIncreaseMaxMovement = CanIncreaseMaxMovement();
            if(canIncreaseMaxMovement){
                IncreaseMaxMovement();
            }
            return canIncreaseMaxMovement;
        }

        public bool CanIncreaseMaxMovement(){
            return maxMovement < maxMovementCap;
        }

        private void IncreaseMaxMovement(){
            maxMovement = Math.Clamp(maxMovement + 1, 0, maxMovementCap);
        }

        public void ResetMovement(){
            movementLeft = maxMovement;
        }

        //same with baseAtk

        public bool TryToIncreaseAtkDamage(int atkIncrease)
        {
            var canIncreaseAtkDamage = CanIncreaseAtkDamage(atkIncrease);

            if (canIncreaseAtkDamage)
            {
                IncreaseAtkDamage(atkIncrease);
            }

            return canIncreaseAtkDamage;
        }

        public bool CanIncreaseAtkDamage(int atkIncrease)
        {
            return atkIncrease != 0;
        }

        protected void IncreaseAtkDamage(int atkIncrease)
        {
            atkDamage = new Damage(Math.Clamp(atkDamage.amount + atkIncrease, 0, 10));
        }

        //same with baseRange

        public bool TryToIncreaseRange(int increase)
        {
            var canIncreaseRange = CanIncreaseRange(increase);

            if (canIncreaseRange)
            {
                IncreaseRange(increase);
            }

            return canIncreaseRange;
        }

        public bool CanIncreaseRange(int increase)
        {
            return increase != 0;
        }

        protected void IncreaseRange(int increase)
        {
            range = Math.Clamp(range + increase, 0, 10);
        }

        //same with baseCostToAtk

        public bool TryToIncreaseCostToAtk(int mouvementCostIncrease, int manaCostIncrease, HeartType[] heartCostIncrease)
        {
            var canIncreaseCostToAtk = CanIncreaseCostToAtk(mouvementCostIncrease, manaCostIncrease, heartCostIncrease);

            if (canIncreaseCostToAtk)
            {
                IncreaseCostToAtk(mouvementCostIncrease, manaCostIncrease, heartCostIncrease);
            }

            return canIncreaseCostToAtk;
        }

        public bool CanIncreaseCostToAtk(int mouvementCostIncrease, int manaCostIncrease, HeartType[] heartCostIncrease)
        {
            return mouvementCostIncrease != 0 || manaCostIncrease != 0 || (heartCostIncrease != null && heartCostIncrease.Length > 0);
        }

        protected void IncreaseCostToAtk(int mouvementCostIncrease, int manaCostIncrease, HeartType[] heartCostIncrease)
        {
            costToAtk = new Cost(
                heartCostIncrease == null || heartCostIncrease.Length == 0 ? costToAtk.heartCost : heartCostIncrease, //TODO
                Math.Clamp(costToMove.mouvementCost + mouvementCostIncrease, 0, 12),
                Math.Clamp(costToMove.manaCost + manaCostIncrease, 0, 10)
            );
        }

        //same for costToMove

        //same for baseCostToMove

        public bool TryToIncreaseCostToMove(int mouvementCostIncrease, int manaCostIncrease, HeartType[] heartCostIncrease)
        {
            var canIncreaseCostToMove = CanIncreaseCostToMove(mouvementCostIncrease, manaCostIncrease, heartCostIncrease);

            if (canIncreaseCostToMove)
            {
                IncreaseCostToMove(mouvementCostIncrease, manaCostIncrease, heartCostIncrease);
            }

            return canIncreaseCostToMove;
        }

        public bool CanIncreaseCostToMove(int mouvementCostIncrease, int manaCostIncrease, HeartType[] heartCostIncrease)
        {
            return mouvementCostIncrease != 0 || manaCostIncrease != 0 || (heartCostIncrease != null && heartCostIncrease.Length > 0);
        }

        protected void IncreaseCostToMove(int mouvementCostIncrease, int manaCostIncrease, HeartType[] heartCostIncrease)
        {
            //Debug.Log($"Increasing Cost To Move by {mouvementCostIncrease} mouvement, {manaCostIncrease} mouvement and {heartCostIncrease} hearts");
            //Debug.Log($"Previous Cost to Move : {costToMove}");
            costToMove = new Cost(
                heartCostIncrease == null || heartCostIncrease.Length == 0 ? costToMove.heartCost : heartCostIncrease, //TODO
                Math.Clamp(costToMove.mouvementCost + mouvementCostIncrease, 0, 12),
                Math.Clamp(costToMove.manaCost + manaCostIncrease, 0, 10)
            );

            //Debug.Log($"New Cost to Move : {costToMove}");
        }


        public int GetAtkIncreaseAccordingToBuffs()
        {
            var atkIncreaseAccordingToBuffs = 0;
            foreach (var buff in buffs)
            {
                if (buff is AtkBuff atkBuff)
                {
                    atkIncreaseAccordingToBuffs += atkBuff.amount;
                }
            }

            return atkIncreaseAccordingToBuffs;
        }

        public int GetRangeIncreaseAccordingToBuffs()
        {
            var rangeIncreaseAccordingToBuffs = 0;
            foreach (var buff in buffs)
            {
                if (buff is RangeBuff rangeBuff)
                {
                    rangeIncreaseAccordingToBuffs += rangeBuff.amount;
                }
            }

            return rangeIncreaseAccordingToBuffs;
        }

        public int GetMouvementCostToMoveIncreaseAccordingToBuffs()
        {
            var mouvementCostIncreaseAccordingToBuffs = 0;
            foreach (var buff in buffs)
            {
                if (buff is WeightedDownBuff weightedDownBuff)
                {
                    mouvementCostIncreaseAccordingToBuffs++;
                }
            }

            return mouvementCostIncreaseAccordingToBuffs;
        }

        public Heart[] GetHeartCostToMoveIncreaseAccordingToBuffs()
        {
            return null;
        }

        public int GetMouvementCostToAtkIncreaseAccordingToBuffs()
        {
            return 0;
        }

        public Heart[] GetHeartCostToAtkIncreaseAccordingToBuffs()
        {
            return null;
        }
        
        

        public void AddPermanentBuff(EntityBuff entityBuff)
        {
            permanentBuffs.Add(entityBuff);
        }

        public void AddTempBuff(EntityBuff entityBuff)
        {
            tempBuffs.Add(entityBuff);
        }
        
        public void AddTempBuffs(List<EntityBuff> entityBuffs)
        {
            tempBuffs.AddRange(entityBuffs);
        }

        public void RemoveTempBuff(EntityBuff entityBuff)
        {
            if (tempBuffs.Contains(entityBuff))
            {
                tempBuffs.Remove(entityBuff);
            }
        }

        public void RemoveTempBuffByEffectId(string effectId)
        {
            var i = 0;
            while (i < tempBuffs.Count)
            {
                var buff = tempBuffs[i];
                if (buff.assiociatedEffectId == effectId)
                {
                    tempBuffs.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        public void RemoveAllPermanentBuff()
        {
            permanentBuffs.Clear();
        }


        public void AddEffect(EntityEffect entityEffect)
        {
            effects.Add(entityEffect);
            //UpdateTempBuffsAccordingToEffects();
        }

        public void RemoveEffect(EntityEffect entityEffect){
            if(effects.Contains(entityEffect)){
                effects.Remove(entityEffect);
                //UpdateTempBuffsAccordingToEffects();
            }
        }

        public void AddEffectList(List<EntityEffect> entityEffects){
            foreach (var effect in entityEffects){
                AddEffect(effect);
            }
        }

        protected void AddDefaultPermanentEffects(){
            //EntityRestoreMovementAtTurnStart
            effects.Add(new EntityDiesWhenHealthIsEmpty(this));
            effects.Add(new EntityIsWeightedDownByStoneHeartEffect(this));
        }

        public override string ToString()
        {
            return $"Entity {name}";
        }

        private int NumberOfBuffs<B>() where B : EntityBuff{
            var count = 0;
            foreach(var buff in buffs){
                if(buff is B b){
                    count++;
                }
            }

            return count;
        }

        public EntityState ToEntityState(){
            EntityState entityState = new EntityState();
            entityState.num = num;
            entityState.playerNum = player.playerNum;
            entityState.model = model;
            entityState.name = name;
            entityState.currentTileNum = currentTile.num;
            entityState.healthState = health.ToHealthState();
            entityState.direction = direction;
            entityState.movementLeft = movementLeft;
            entityState.costToAtkState = costToAtk.ToCostState();
            entityState.baseCostToAtkState = baseCostToAtk.ToCostState();
            entityState.range = range;
            entityState.baseRange = baseRange;
            entityState.atkDamageState = atkDamage.ToDamageState();
            entityState.baseAtkDamageState = baseAtkDamage.ToDamageState();
            entityState.maxMovement = maxMovement;
            entityState.costToMoveState = costToMove.ToCostState();

            entityState.tileNumsToMoveTo = new List<uint>();
            foreach (var tile in tilesToMoveTo)
            {
                entityState.tileNumsToMoveTo.Add(tile.num);
            }

            entityState.effectStates = new List<EffectState>();
            entityState.buffStates = new List<BuffState>();

            foreach (Effect effect in effects)
            {
                entityState.effectStates.Add(EffectStateGenerator.GenerateEffectState(effect));
            }

            foreach (Buff buff in buffs){
                entityState.buffStates.Add(buff.ToBuffState());
            }


            return entityState;
        }
    }
}