using System.Collections.Generic;
using UnityEngine;

public class Player{

    public List<Entity> entities{
        get;
        private set;
    }

    public int playerNum{
        get;
        private set;
    }

    public int playerColor{
        get;
        private set;
    }

    public int movementLeft{
        get;
        private set;
    }

    public int maxMovement{
        get;
        private set;
    }

    public Hero hero{
        get;
        set;
    }

    public Hand hand{
        get;
        private set;
    }

    public Player(int num, int color){
        playerNum = num;
        playerColor = color;
        hand = new Hand(this);
        entities = new List<Entity>();
    }

    private void ResetMovement(){
        movementLeft = maxMovement;
    }

    public virtual void OnStartGame(){
        
    }

    public virtual void OnStartTurn(){
    }

    public virtual void OnStartPlayerTurn(){
        maxMovement ++;
        ResetMovement();
    }

    public virtual void OnEndTurn(){
        
    }

    public virtual void OnEndPlayerTurn(){
        
    }

    public bool TryToCreatePayCostAction(Cost cost, out PlayerPayCostAction payCostAction){
        payCostAction = new PlayerPayCostAction(this, cost);
        var canPayHeartCost = CanPayCost(cost);
        if(canPayHeartCost){

            var useMovementAction = new PlayerUseMovementAction(this, cost.movementCost);
            var payHeartCostAction = new PlayerPayHeartCostAction(this, cost.heartCost);

            payCostAction = new PlayerPayCostAction(this, cost, payHeartCostAction);

            Game.currentGame.PileAction(payCostAction, false);
            Game.currentGame.PileAction(useMovementAction, false);
            Game.currentGame.PileAction(payHeartCostAction, true);
            
        }

        return canPayHeartCost;
    }

    private bool CanPayCost(Cost cost){
        //TODO
        return CanPayHeartCost(cost.heartCost) && CanUseMovement(cost.movementCost);
    }

    public bool TryToCreatePlayerUseMovementAction(int movement, out PlayerUseMovementAction useMovementAction){
        useMovementAction = new PlayerUseMovementAction(this, movement);
        var canUseMovement =  CanUseMovement(movement);
        if(canUseMovement){
            Game.currentGame.PileAction(useMovementAction);
        }

        return canUseMovement;
    }

    public bool TryToUseMovement(int movement){
        
        var canUseMovement = CanUseMovement(movement);

        if(canUseMovement){
            UseMouvement(movement);
        }

        return canUseMovement;
    }

    private bool CanUseMovement(int movement){
        if(movement > movementLeft){
            Debug.Log($"{this} cannot use {movement} movement. {movementLeft} movement left");
            return false;
        }

        return true;
    }

    private void UseMouvement(int movement){
        movementLeft -= movement;
        Debug.Log($"{this} using {movement} movement. {movementLeft} movement left");
    }

    public bool TryToCreatePayHeartCostAction(Heart[] hearts, out PlayerPayHeartCostAction payHeartCostAction){
        payHeartCostAction = new PlayerPayHeartCostAction(this, hearts);
        var canPayHeartCost = CanPayHeartCost(hearts);
        if(canPayHeartCost){
            Game.currentGame.PileAction(payHeartCostAction);
        }

        return canPayHeartCost;
    }

    public bool TryToPayHeartCost(Heart[] hearts){
        
        var canPayHeartCost = CanPayHeartCost(hearts);

        if(canPayHeartCost){
            PayHeartCost(hearts);
        }

        return canPayHeartCost;
    }

    private bool CanPayHeartCost(Heart[] hearts){
        //TODO
        return true;
    }

    private void PayHeartCost(Heart[] hearts){
        Debug.Log($"{this} paying {hearts}");
    }

    public override string ToString(){
        return $"Player {playerNum}";
    }
}
