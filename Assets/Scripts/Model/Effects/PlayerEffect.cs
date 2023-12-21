using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : Effect
{
    
    public Player associatedPlayer{
        get;
        protected set;
    }

    public PlayerEffect(Player player){
        associatedPlayer = player;
    }
}
