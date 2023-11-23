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

    public int mouvementLeft{
        get;
        private set;
    }

    public int maxMouvement{
        get;
        private set;
    }

    public Player(int num, int color){
        playerNum = num;
        playerColor = color;
    }

    private void ResetMouvement(){
        mouvementLeft = maxMouvement;
    }

    public virtual void OnStartGame(){
        
    }

    public virtual void OnStartTurn(){
    }

    public virtual void OnStartPlayerTurn(){
        maxMouvement ++;
        ResetMouvement();
    }

    public virtual void OnEndTurn(){
        
    }

    public virtual void OnEndPlayerTurn(){
        
    }

    public bool TryToUseMouvement(int mouvement){
        if(mouvement > mouvementLeft){
            Debug.Log($"{this} cannot use {mouvement} mouvement. {mouvementLeft} mouvement left");
            return false;
        }
        
        mouvementLeft -= mouvement;

        Debug.Log($"{this} using {mouvement} mouvement. {mouvementLeft} mouvement left");

        return true;
    }

    public override string ToString(){
        return $"Player {playerNum}";
    }
}
