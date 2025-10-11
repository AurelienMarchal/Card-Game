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

        public int manaLeft{
            get;
            private set;
        }

        public int maxMana{
            get;
            private set;
        }

        public int maxManaCap {
            get;
            private set;
        }
        
        public Deck deck
        {
            get;
            private set;
        }

        public Hero hero
        {
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

        public Player(uint num, uint[] deckList, System.Random random){
            //temp
            maxMana = 0;
            //temp
            maxManaCap = 10;
            playerNum = num;
            deck = new Deck(deckList, this, random);
            hand = new Hand(this);
            entities = new List<Entity>();
            effects = new List<Effect>();
            SetupPermanentEffects();
        }


        public void ResetMana(){
            manaLeft = maxMana;
        }
        

        public bool TryToIncreaseMaxMana(){
            var canIncreaseMaxMana = CanIncreaseMaxMana();
            if(canIncreaseMaxMana){
                IncreaseMaxMana();
            }

            return canIncreaseMaxMana;
        }

        public bool CanIncreaseMaxMana(){
            return maxMana < maxManaCap;
        }

        private void IncreaseMaxMana(){
            maxMana = Math.Clamp(maxMana+1, 0, maxManaCap);
        }
        
        public bool TryToCreatePlayerUseManaAction(int mana, out PlayerUseManaAction useManaAction){
            useManaAction = new PlayerUseManaAction(this, mana);
            var canUseMana =  CanUseMana(mana);
            if(canUseMana){
                Game.currentGame.PileAction(useManaAction);
            }

            return canUseMana;
        }

        
        public bool TryToUseMana(int mana){
            
            var canUseMana = CanUseMana(mana);

            if(canUseMana){
                UseMana(mana);
            }

            return canUseMana;
        }

        
        public bool CanUseMana(int mana){
            return mana <= manaLeft && mana >= 0;
        }

        
        private void UseMana(int mana){
            manaLeft = Math.Clamp(manaLeft - mana, 0, maxMana);
            Debug.Log($"{this} using {mana} mana. {manaLeft} mana left");
        }

        public bool TryToCreatePlayerPlayCardAction(Card card, out PlayerPlayCardAction playerPlayCardAction, Action costAction, Tile targetTile = null, Entity targetEntity = null)
        {

            playerPlayCardAction = new PlayerPlayCardAction(this, card, targetTile, targetEntity, requiredAction: costAction);
            var canPlayCard = CanPlayCard(card, targetTile, targetEntity);
            if (canPlayCard)
            {
                Game.currentGame.PileAction(playerPlayCardAction);
            }

            return canPlayCard;
        }

        public bool CanPlayCard(Card card, Tile targetTile = null, Entity targetEntity = null)
        {
            return card.CanBeActivated(targetTile, targetEntity);
        }

        public bool TryToPlayCard(Card card, Tile targetTile = null, Entity targetEntity = null)
        {
            return card.TryToActivate(targetTile, targetEntity);
        }
        
        public bool TryToCreateSpawnEntityAction(Entity entity, Tile startingTile, out PlayerSpawnEntityAction playerSpawnEntityAction, Action requiredAction = null)
        {
            playerSpawnEntityAction = new PlayerSpawnEntityAction(this, entity, startingTile, requiredAction);
            var canSpawnEntityAt = CanSpawnEntityAt(startingTile);
            if (canSpawnEntityAt)
            {
                Game.currentGame.PileAction(playerSpawnEntityAction);
            }

            return canSpawnEntityAt;
        }

        

        public bool TryToSpawnEntity(Entity entity)
        {
            var canSpawnEntity = CanSpawnEntity(entity);
            if (canSpawnEntity)
            {
                SpawnEntity(entity);
            }
            return canSpawnEntity;
        }

        private void SpawnEntity(Entity entity){
            //Temp
            entity.num = (uint)entities.Count;
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

        public Card TryToDraw()
        {
            var canDraw = CanDraw();
            if (canDraw)
            {
                return deck.TryToDraw();
            }

            return null;
        }

        public bool CanDraw()
        {
            return deck.CanDraw();
        }

        private void SetupPermanentEffects()
        {   
            effects.Add(new PlayerResetManaAtTurnStartPlayerEffect(this));
            effects.Add(new PlayerIncreaseMaxManaAtTurnStartPlayerEffect(this));
            
            effects.Add(new DrawCardAtTurnStartPlayerEffect(this));
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

            playerState.manaLeft = manaLeft;
            playerState.maxMana = maxMana;
            playerState.maxManaCap = maxManaCap;

            playerState.effectStates = new List<EffectState>();
            foreach (Effect effect in effects){
                playerState.effectStates.Add(effect.ToEffectState());
            }


            return playerState;
        }
    }
}