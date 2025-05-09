
using System.Collections.Generic;
using UnityEngine;


namespace GameLogic{


    using GameEffect;
    using GameLogic.GameAction;
    using UserAction;

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

        public System.Random random{
            get;
            private set;
        }

        public List<Effect> effects{
            get;
            private set;
        }

        private List<GameAction.Action> actionPile;

        public List<GameAction.Action> depiledActionQueue{
            get;
            private set;
        }

        private bool depileStarted;

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
            actionPile = new List<GameAction.Action>();
            depiledActionQueue = new List<GameAction.Action>();
            random = new System.Random(0);
            effects = new List<Effect>();
            SetupPermanentEffects();
            depileStarted = false;
            for(uint i = 0; i < numberOfPlayer; i++){
                players[i] = new Player(i + 1);
            }
        }

        public void StartGame(){
            Debug.Log("Starting Game");
            currentPlayer = players[0];
            turn = 0;
        }

        public void StartTurn(){
            Debug.Log($"Starting turn {turn}");
        }

        public void StartPlayerTurn(){
            Debug.Log($"Starting player turn {currentPlayer}");
        }

        public bool EndPlayerTurn(){
            Debug.Log($"Ending player turn {currentPlayer}");

            var changeTurn = GoToNextPlayer();
            if(changeTurn){
                turn ++;
            }
            return changeTurn;
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

        public bool ReceiveUserAction(UserAction.UserAction userAction){

            if(userAction.playerNum != currentPlayer.playerNum){
                return false;
            }

            switch (userAction){
                case EndTurnUserAction endTurnUserAction:
                    PileAction(new EndPlayerTurnAction(currentPlayer, null));
                    break;
                default:
                    break;
            }

            return false;
        }

        public void PileActions(GameAction.Action[] actions){
            foreach(var action in actions){
                if(actionPile.Count < maxPileCount){
                actionPile.Add(action);
                Debug.Log($"Piling {action}");
            }
                else{
                    Debug.Log($"Reached pile action maximum");
                }
            }

            TryToDepileActionPile();
        }

        
        public void PileAction(GameAction.Action action){
            if(actionPile.Count < maxPileCount){
                actionPile.Add(action);
                Debug.Log($"Piling {action}");
            }
            else{
                Debug.Log($"Reached pile action maximum");
            }

            TryToDepileActionPile();
        }

        public bool TryToDepileActionPile(){
            var canDepile = !depileStarted;
            if(canDepile){
                DepileActionPile();
            }

            return canDepile;
        }

        private void DepileActionPile(){
            var c = 0;
            depileStarted = true;
            while(actionPile.Count > 0 && c < 1000){
                var action = actionPile[^1];

                Debug.Log("Depile start");
                Debug.Log($"action : {action}");

                //Debug.Log("Checking Trigger");

                CheckTriggers(action);

                //Debug.Log($"GameAction.Action pile : {string.Join( ",", actionPile)}");

                var newAction = actionPile[^1];

                //Debug.Log($"newAction : {newAction}");

                if(action == newAction){

                    Debug.Log("Trying to perform action");
                    var wasPerformed = action.TryToPerform();

                    if(wasPerformed){
                        Debug.Log($"{action} was performed");
                    }
                    else{
                        Debug.Log($"{action} was not performed");
                    }

                    depiledActionQueue.Add(action);
                    actionPile.Remove(action);

                    if(wasPerformed){
                        Debug.Log("Checking Trigger");
                        CheckTriggers(action);
                    }
                }

                else{
                    c++;
                }

                //Debug.Log($"GameAction.Action pile : {string.Join( ",", actionPile)}");
            }

            //Debug.Log($"GameAction.Action performed pile : {string.Join( ",", depiledActionQueue)}");

            depileStarted = false;
        }

        void CheckTriggers(GameAction.Action action){

            

            foreach(Effect effect in currentGame.effects){
                if(effect.Trigger(action)){
                    effect.TryToCreateEffectActivatedAction(action, out _);
                }
            }

            foreach(Effect effect in currentGame.board.effects){
                if(effect.Trigger(action)){
                    effect.TryToCreateEffectActivatedAction(action, out _);
                }
            }

            foreach(Player player in players){
                foreach(Effect effect in player.effects){
                    if(effect.Trigger(action)){
                        effect.TryToCreateEffectActivatedAction(action, out _);
                    }
                }
            }

            foreach(Tile tile in currentGame.board.tiles){
                foreach(Effect effect in tile.effects){
                    if(effect.Trigger(action)){
                        effect.TryToCreateEffectActivatedAction(action, out _);
                    }
                }
            }

            //Debug.Log($"Cheking triggers for board entities : [{String.Join(", ", currentGame.board.entities)}]");

            foreach(Entity entity in currentGame.board.entities){
                foreach(Effect effect in entity.effects){
                    //Debug.Log($"Checking Trigger for {entity} for effect {effect} with action {action}");
                    if(effect.Trigger(action)){
                        
                        effect.TryToCreateEffectActivatedAction(action, out _);
                    }
                }
            }
        }

        public GameAction.Action DequeueDepiledActionQueue(){
            if(depiledActionQueue.Count == 0){
                return null;
            }
            var action = depiledActionQueue[0];
            depiledActionQueue.RemoveAt(0);

            return action;
        }

        private void SetupPermanentEffects(){

        }
    }
}
