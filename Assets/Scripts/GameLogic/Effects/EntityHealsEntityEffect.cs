using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameLogic{

    using GameAction;

    namespace GameEffect{
        public class PlayerHealEntityEffect : PlayerEffect, CanBeActivatedWithEntityTargetInterface{
            public int numberOfHeartsHealed{
                get;
                private set;
            }


            public PlayerHealEntityEffect(Player player, int numberOfHeartsHealed) : base(player)
            {
                this.numberOfHeartsHealed = numberOfHeartsHealed;
            }

            public bool CanBeActivated()
            {
                return numberOfHeartsHealed > 0;
            }

            public override string GetEffectText(){
                return $"Heals for {numberOfHeartsHealed}";
            }

            public List<Entity> PossibleEntityTargets()
            {
                return new List<Entity> (associatedPlayer.entities);
            }

            public bool CanBeActivatedWithEntityTarget(Entity entity)
            {
                return entity != Entity.noEntity;
            }

            void CanBeActivatedWithEntityTargetInterface.ActivateWithEntityTarget(Entity entity)
            {
                Game.currentGame.PileAction(new EntityHealsAction(numberOfHeartsHealed, entity));
            }
        }
    }
}
