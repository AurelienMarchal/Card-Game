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

    void UpdateAccordingToPlayer(){
        if(player != null){
            handManager.hand = player.hand;
        }
        
    }


    public bool TryToPlayCard(Card card){
        
        player.TryToCreatePayCostAction(card.cost, out PlayerPayCostAction playerPayCostAction);

        //Card Played action
        
        

        return playerPayCostAction.wasPerformed;

    }
}
