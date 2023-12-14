using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [SerializeField]
    HandManager handManager;

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
    }

    private void OnCardClicked(Card card){
        Debug.Log("On Mouse click");
        if(card.CanBeActivated()){
            TryToPlayCard(card);
        }
    }

    void UpdateAccordingToPlayer(){
        if(player != null){
            handManager.hand = player.hand;
        }
        
    }


    public bool TryToPlayCard(Card card, Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity){
        player.TryToCreatePayCostAction(card.cost, out PlayerPayCostAction playerPayCostAction);
        card.TryToCreateCardPlayedAction(playerPayCostAction, out CardPlayedAction cardPlayedAction, targetTile, targetEntity);
        return cardPlayedAction.wasPerformed;
    }
}
