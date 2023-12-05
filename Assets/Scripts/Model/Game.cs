using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Game{

    public const int maxPileCount = 100;

    public Board board{
        get;
        private set;
    }

    public Player[] players{
        get;
        private set;
    }

    public Player currentPlayer{
        get;
        private set;
    }

    public int turn{
        get;
        private set;
    }

    private List<Action> actionPile;

    private List<Action> actionPerformedPile;

    private Game(){
        
    }

    static Game currentGame_;

    public static Game currentGame{
        get{
            if(currentGame_ == null){
                currentGame_ = new Game();
            }
            return currentGame_;
        }
    }

    public void SetUpGame(int numberOfPlayer, int boardHeight, int boardWidth){
        board = new Board(boardHeight, boardWidth);
        players = new Player[2];
        actionPile = new List<Action>();
        actionPerformedPile = actionPile;
        for(var i = 0; i < numberOfPlayer; i++){
            players[i] = new Player(i + 1, i);
        }
    }

    public void StartGame(){
        Debug.Log("Starting Game");
        currentPlayer = players[0];
        turn = 0;
        foreach(Player player in players){
            player.OnStartGame();
            foreach(Entity entity in board.entities){
                if(entity.player == player){
                    entity.OnStartGame();
                    //OnEntityMoving(entity);
                }
            }
        }
        
        StartTurn();
    }

    public void StartTurn(){
        Debug.Log($"Starting turn {turn}");

        foreach(Player player in players){
            player.OnStartTurn();
            foreach(Entity entity in board.entities){
                if(entity.player == player){
                    entity.OnStartTurn();
                }
            }
        }

        StartPlayerTurn();
    }

    public void StartPlayerTurn(){
        Debug.Log($"Starting player turn {currentPlayer}");

        currentPlayer.OnStartPlayerTurn();
        foreach(Entity entity in board.entities){
            if(entity.player == currentPlayer){
                entity.OnStartPlayerTurn();
            }
        }
    }

    public void EndPlayerTurn(){
        Debug.Log($"Ending player turn {currentPlayer}");
        
        currentPlayer.OnEndPlayerTurn();
        foreach(Entity entity in board.entities){
            if(entity.player == currentPlayer){
                entity.OnEndPlayerTurn();
            }
        }

        var changeTurn = GoToNextPlayer();
        if(changeTurn){
            turn ++;
            StartTurn();
        }
        else{
            StartPlayerTurn();
        }
    }


    private bool GoToNextPlayer(){
        var nextPlayerIndex = currentPlayer.playerNum;
        if(nextPlayerIndex >= players.Length){
            currentPlayer = players[0];
            return true;
        }
        currentPlayer = players[nextPlayerIndex];
        return false;
    }

    

    public void PileAction(Action action, bool depile = true){
        if(actionPile.Count < maxPileCount){
            actionPile.Add(action);
            Debug.Log($"Piling {action}");
        }
        else{
            Debug.Log($"Reached pile action maximum");
        }

        if(depile){
            DepileActionPile();
        }
    }

    private void DepileActionPile(){
        var c = 0;

        while(actionPile.Count > 0 && c < 1000){
            var action = actionPile[^1];

            CheckTriggers(action);
            

            var newAction = actionPile[^1];

            if(action == newAction){
                action.Perform();
                actionPerformedPile.Add(action);
                actionPile.Remove(action);
                CheckTriggers(action);
            }
            else{
                c ++;
            }
        }

        Debug.Log(actionPerformedPile);
    }

    void CheckTriggers(Action action){

        foreach(Player player in players){

            }

            foreach(Entity entity in board.entities){
                foreach(Effect effect in entity.effects){
                    if(effect.Trigger(action)){
                        effect.Activate(false);
                    }
                }
            }

            foreach(Tile tile in board.tiles){
                foreach(Effect effect in tile.effects){
                    if(effect.Trigger(action)){
                        effect.Activate(false);
                    }
                }
            }

    }

    
}
