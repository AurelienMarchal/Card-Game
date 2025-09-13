
using System;
using System.Collections.Generic;

namespace GameLogic{

    using GameState;
    using GameEffect;
    [Serializable]
    public class Weapon{

        public string name{
            get;
            protected set;
        }

        public Damage atkDamage{
            get;
            protected set;
        }

        public Cost costToUse{
            get;
            protected set;
        }

        public int range{
            get;
            protected set;
        }

        public List<WeaponEffect> effects{
            get;
            protected set;
        }

        public const Weapon noWeapon = null;

        public Weapon(string name, Damage damage, Cost cost, int range, List<WeaponEffect> permanentEffects){
            this.name = name;
            atkDamage = damage;
            costToUse = cost;
            this.range = range;
            effects = new List<WeaponEffect>();
            AddEffectList(permanentEffects);
            AddDefaultPermanentEffects();
        }

    

        public void AddEffect(WeaponEffect weaponEffect){
            weaponEffect.associatedWeapon = this;
            effects.Add(weaponEffect);
        }

        public void AddEffectList(List<WeaponEffect> weaponEffects){
            foreach (var effect in weaponEffects){
                AddEffect(effect);
            }
        }

        protected void AddDefaultPermanentEffects(){

        }

        public override string ToString()
        {
            return $"Weapon {name}";
        }

        public virtual void GetTilesAndEntitiesAffectedByAtk(Tile tile, Direction direction, out Entity[] entitiesAffected, out Tile[] tilesAffected){

            var entityInFront = Game.currentGame.board.GetFirstEntityInDirectionWithRange(
                Game.currentGame.board.NextTileInDirection(tile, direction), 
                direction, 
                range,
                out tilesAffected
            );

            if(entityInFront == Entity.noEntity){
                entitiesAffected = new Entity[0];
                return;
            }
            
            entitiesAffected = new Entity[1]{entityInFront};
            

        }


        public WeaponState ToWeaponState(){
            WeaponState weaponState = new WeaponState();
            weaponState.name = name;
            weaponState.range = range;
            weaponState.costToUseState = costToUse.ToCostState();
            weaponState.atkDamageState = atkDamage.ToDamageState();
            weaponState.effectStates = new List<EffectState>();
            foreach (Effect effect in effects){
                weaponState.effectStates.Add(effect.ToEffectState());
            }

            return weaponState;
        }
    }
}