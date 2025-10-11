using System.Collections;
using System.Collections.Generic;
using GameLogic;
using GameLogic.GameEffect;
using UnityEngine;



namespace GameLogic
{
    public class HealTargetEntityCard : EffectCard
    {
        public HealTargetEntityCard(Player player) :
            base(
                1,
                player,
                new Cost(mana: 1),
                new PlayerHealsTargetEntityEffect(player, 2),
                needsEntityTarget: true)
        {

        }

        public override string GetCardName()
        {
            return "Healing Card";
        }

        public override string GetText()
        {
            return "Heal target entity for 2 hearts";
        }
    }
}

