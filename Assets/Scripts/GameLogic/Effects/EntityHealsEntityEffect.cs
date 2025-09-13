using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameLogic{

    using GameAction;

    namespace GameEffect{
        public class EntityHealsEntityEffect : EntityEffect{
            public int numberOfHeartsHealed{
                get;
                private set;
            }

            public EntityHealsEntityEffect(int numberOfHeartsHealed, Entity entity) : base(entity){
                this.numberOfHeartsHealed = numberOfHeartsHealed;
            }

            public override bool CanBeActivated()
            {
                return base.CanBeActivated() && numberOfHeartsHealed > 0;
            }

            protected override void Activate(){
                Game.currentGame.PileAction(new EntityHealsAction(numberOfHeartsHealed, associatedEntity));
            }


            public override string GetEffectText(){
                return $"{associatedEntity} heals for {numberOfHeartsHealed}";
            }
        }
    }
}
