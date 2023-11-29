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

    public bool TryToUseMovement(int movement){
        if(movement > movementLeft){
            Debug.Log($"{this} cannot use {movement} movement. {movementLeft} movement left");
            return false;
        }
        
        movementLeft -= movement;

        Debug.Log($"{this} using {movement} movement. {movementLeft} movement left");

        return true;
    }

    public override string ToString(){
        return $"Player {playerNum}";
    }
}
