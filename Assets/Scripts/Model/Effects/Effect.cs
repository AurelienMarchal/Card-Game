using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect{

    public Player player{
        get;
        protected set;
    }

    public Effect(Player player){
        this.player = player;
    }

    public virtual void OnActivate(){

    }

    public virtual void OnStartGame(){
        
    }

    public virtual void OnStartTurn(){
    }

    public virtual void OnStartPlayerTurn(Player player){

    }

    public virtual void OnEndTurn(){
        
    }

    public virtual void OnEndPlayerTurn(Player player){
        
    }

    public virtual void OnEntityMoving(Entity entity){

    }

}
