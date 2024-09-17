using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [SerializeField]
    HandManager handManager;

    public CardEvent cardHoverEnterEvent = new CardEvent();

    public CardEvent cardHoverExitEvent = new CardEvent();

    private Player player_;

    public Player player{
        get{
            return player_;
        }

        set{
            player_ = value;
            UpdateAccordingToPlayer();
        }
    }

    void Start(){
        handManager.cardClickedEvent.AddListener(OnCardClicked);
        handManager.cardHoverEnterEvent.AddListener(
            (card) => {
                card.activableEffect.associatedEntity = player.hero;
                cardHoverEnterEvent.Invoke(card);
                card.activableEffect.associatedEntity = Entity.noEntity;
            });
        handManager.cardHoverExitEvent.AddListener(
            (card) => {
                cardHoverExitEvent.Invoke(card);
                
            });
    }

    private void OnCardClicked(Card card){
        Debug.Log($"Clicked on {card}");
        if(!player.hero.CanPlayCard(card)){
            return;
        }

        player.hero.TryToCreateEntityUseMovementAction(card.activableEffect.cost.mouvementCost, out EntityUseMovementAction entityUseMovementAction);
        player.hero.TryToCreateEntityPayHeartCostAction(card.activableEffect.cost.heartCost, out EntityPayHeartCostAction entityPayHeartCostAction);

        if(!entityPayHeartCostAction.wasPerformed || !entityUseMovementAction.wasPerformed){
            return;
        }

        player.hero.TryToCreateEntityPlayCardAction(card, out EntityPlayCardAction entityPlayCardAction, entityPayHeartCostAction);
    }

    void UpdateAccordingToPlayer(){
        if(player != null){
            handManager.hand = player.hand;
        }
    }
}
