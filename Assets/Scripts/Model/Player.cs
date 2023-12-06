using UnityEngine;

public class Player{

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

    public Player(int num, int color){
        playerNum = num;
        playerColor = color;
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

    public bool TryToCreateUseMovementAction(int movement, out UseMovementAction useMovementAction){
        useMovementAction = new UseMovementAction(this, movement);
        var canUseMovement =  CanUseMovement(movement);
        if(canUseMovement){
            Game.currentGame.PileAction(useMovementAction);
        }

        return canUseMovement;
    }

    public bool TryToUseMovement(int movement){
        
        var canUseMovement =  CanUseMovement(movement);

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

    public override string ToString(){
        return $"Player {playerNum}";
    }
}
