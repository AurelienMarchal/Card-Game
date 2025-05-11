using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic{

    using GameAction;
    using GameEffect;
    using GameState;
    public class Player{
        public List<Entity> entities{
            get;
            private set;
        }

        public uint playerNum{
            get;
            private set;
        }

        [Obsolete]
        public int manaLeft{
            get;
            private set;
        }

        [Obsolete]
        public int maxMana{
            get;
            private set;
        }

        [Obsolete]
        public const int maxManaCap = 10;

        public Hero hero{
            get;
            set;
        }

        public Hand hand{
            get;
            private set;
        }

        public List<Effect> effects{
            get;
            protected set;
        }

        public Player(uint num){
            playerNum = num;
            hand = new Hand(this);
            entities = new List<Entity>();
            effects = new List<Effect>();
            SetupPermanentEffects();
        }

        [Obsolete]
        public void ResetMana(){
            manaLeft = maxMana;
        }
        
        [Obsolete]
        public bool TryToIncreaseMaxMana(){
            var canIncreaseMaxMana = CanIncreaseMaxMana();
            if(canIncreaseMaxMana){
                IncreaseMaxMana();
            }

            return canIncreaseMaxMana;
        }

        [Obsolete]
        public bool CanIncreaseMaxMana(){
            return maxMana < maxManaCap;
        }

        [Obsolete]
        private void IncreaseMaxMana(){
            maxMana = Math.Clamp(maxMana+1, 0, maxManaCap);
        }

        public bool TryToCreateSpawnEntityAction(EntityModel model, string name, Tile startingTile, Health startingHealth, int startingMaxMovement, Direction startingDirection, List<EntityEffect> permanentEffects, Action requiredAction, out PlayerSpawnEntityAction playerSpawnEntityAction, Weapon weapon=Weapon.noWeapon){
            playerSpawnEntityAction = new PlayerSpawnEntityAction(this, model, name, startingTile, startingHealth, startingMaxMovement, permanentEffects, startingDirection,  weapon, requiredAction);
            var canSpawnEntityAt = CanSpawnEntityAt(startingTile);
            if(canSpawnEntityAt){
                Game.currentGame.PileAction(playerSpawnEntityAction);
            }

            return canSpawnEntityAt;
        }

        public bool TryToCreateSpawnEntityAction(ScriptableEntity scriptableEntity, Tile startingTile, Direction startingDirection, Action requiredAction, out PlayerSpawnEntityAction playerSpawnEntityAction){
            playerSpawnEntityAction = new PlayerSpawnEntityAction(this, scriptableEntity,  startingTile, startingDirection,  requiredAction);
            var canSpawnEntityAt = CanSpawnEntityAt(startingTile);
            if(canSpawnEntityAt){
                Game.currentGame.PileAction(playerSpawnEntityAction);
            }

            return canSpawnEntityAt;
        }

        public bool TryToSpawnEntity(Entity entity){
            var canSpawnEntity = CanSpawnEntity(entity);
            if(canSpawnEntity){
                SpawnEntity(entity);
            }
            return canSpawnEntity;
        }

        private void SpawnEntity(Entity entity){
            entities.Add(entity);

            if(entity is Hero hero){
                this.hero = hero;
            }

            //Debug.Log($"Adding {entity} to {this}");
        }

        public bool CanSpawnEntity(Entity entity){
            return Game.currentGame.board.GetEntityAtTile(entity.currentTile) == Entity.noEntity;
        }

        public bool CanSpawnEntityAt(Tile tile){
            return Game.currentGame.board.GetEntityAtTile(tile) == Entity.noEntity;
        }

        [Obsolete]
        public bool TryToCreatePlayerUseManaAction(int mana, out PlayerUseManaAction useManaAction){
            useManaAction = new PlayerUseManaAction(this, mana);
            var canUseMana =  CanUseMana(mana);
            if(canUseMana){
                Game.currentGame.PileAction(useManaAction);
            }

            return canUseMana;
        }

        [Obsolete]
        public bool TryToUseMana(int mana){
            
            var canUseMana = CanUseMana(mana);

            if(canUseMana){
                UseMana(mana);
            }

            return canUseMana;
        }

        [Obsolete]
        private bool CanUseMana(int mana){
            return mana <= manaLeft && mana >= 0;
        }

        [Obsolete]
        private void UseMana(int mana){
            manaLeft = Math.Clamp(manaLeft - mana, 0, maxMana);
            Debug.Log($"{this} using {mana} mana. {manaLeft} mana left");
        }



        private void SetupPermanentEffects(){

        }

        public override string ToString(){
            return $"Player {playerNum}";
        }

        public PlayerState ToPlayerState(){
            PlayerState playerState = new PlayerState();

            playerState.playerNum = playerNum;

            playerState.handState = hand.ToHandState();
            playerState.heroState = hero.ToHeroState();

            playerState.entityStates = new List<EntityState>();
            foreach (Entity entity in entities){
                playerState.entityStates.Add(entity.ToEntityState());
            }

            playerState.effectStates = new List<EffectState>();
            foreach (Effect effect in effects){
                playerState.effectStates.Add(effect.ToEffectState());
            }


            return playerState;
        }
    }
}