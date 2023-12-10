using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand 
{
    
    public List<Card> cards{
        get;
        private set;
    }

    public Hand(){
        
    }

    public Hand(List<Card> cards){
        this.cards = cards;
    }
}
