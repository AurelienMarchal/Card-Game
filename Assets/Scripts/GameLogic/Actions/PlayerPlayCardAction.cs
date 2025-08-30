using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        public class PlayerPlayCardAction : PlayerAction
        {

            public Card card{
                get;
                private set;
            }
            
            public Tile targetTile
            {
                get;
                private set;
            }

            public Entity targetEntity
            {
                get;
                private set;
            }


            public PlayerPlayCardAction(Player player, Card card, Tile targetTile = null, Entity targetEntity = null, Action requiredAction = null) : base(player, requiredAction)
            {
                this.card = card;
                this.targetTile = targetTile;
                this.targetEntity = targetEntity;
            }

            protected override bool Perform()
            {
                return player.TryToPlayCard(card, targetTile, targetEntity);
            }

            public override ActionState ToActionState()
            {
                var actionState = new PlayerPlayCardActionState();
                actionState.playerNum = player.playerNum;
                actionState.card = card.ToCardState();
                actionState.targetTileNum = targetTile != null ? (int)targetTile.num : -1;
                actionState.targetEntityPlayerNum = targetEntity != null ? (int)targetEntity.player.playerNum : -1;
                actionState.targetEntityNum = targetEntity != null ? (int)targetEntity.num : -1;
                return actionState;
            }
        }
    }
}

