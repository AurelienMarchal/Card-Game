using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand 
{
    
    public List<Card> cards{
        get;
        /*private*/ set; // for setting cards from the editor
    }

    public Player player{
        get;
        private set;
    }

    public Hand(Player player){
        cards = new List<Card>();
        this.player = player;
    }

    public Hand(Player player, List<Card> cards){
        this.player = player;
        this.cards = cards;
    }
}
