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
        Debug.Log($"Clicked on {card}");
        if(card.CanBeActivated()){
            player.TryToPlayCard(card);
        }
    }

    void UpdateAccordingToPlayer(){
        if(player != null){
            handManager.hand = player.hand;
        }
        
    }
}
