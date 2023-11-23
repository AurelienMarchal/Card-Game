using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game{

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

    public Game(int numberOfPlayer, int boardHeight, int boardWidth){
        board = new Board(boardHeight, boardWidth);
        players = new Player[numberOfPlayer];
        for(var i = 0; i < numberOfPlayer; i++){
            players[i] = new Player(i, i);
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
        var nextPlayerIndex = currentPlayer.playerNum + 1;
        if(nextPlayerIndex >= players.Length){
            currentPlayer = players[0];
            return true;
        }
        currentPlayer = players[nextPlayerIndex];
        return false;
    }

    
}
