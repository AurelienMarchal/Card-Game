using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{


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

    }


    public bool TryToPlayCard(Card card){
        
        player.TryToCreatePayCostAction(card.cost, out PlayerPayCostAction playerPayCostAction);

        //Card Played action
        
        

        return playerPayCostAction.wasPerformed;

    }
}
