
using System.Collections.Generic;
using UnityEngine;


namespace GameLogic{


    using GameEffect;
    using GameAction;
    using UserAction;
    using GameState;

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

        private List<Action> actionPile;

        public List<Action> depiledActionQueue{
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

        public void SetUpGame(int numberOfPlayer, int boardHeight, int boardWidth)
        {
            board = new Board(boardHeight, boardWidth);
            players = new Player[numberOfPlayer];
            actionPile = new List<Action>();
            depiledActionQueue = new List<Action>();
            random = new System.Random(0);
            effects = new List<Effect>();
            SetupPermanentEffects();
            depileStarted = false;
            for (uint i = 0; i < numberOfPlayer; i++)
            {
                players[i] = new Player(i);
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
            var nextPlayerIndex = currentPlayer.playerNum + 1;
            
            if (nextPlayerIndex >= players.Length)
            {
                currentPlayer = players[0];
                Debug.Log($"Current Player : {currentPlayer}");
                return true;
            }
            currentPlayer = players[nextPlayerIndex];
            Debug.Log($"Current Player : {currentPlayer}");
            return false;
        }

        private Player GetPlayerByPlayerNum(uint playerNum)
        {
            if (players == null)
            {
                return null;
            }

            if (playerNum > players.Length)
            {
                return null;
            }

            return players[playerNum];
        }

        private Entity GetEntityByEntityNumAndPlayerNum(uint playerNum, uint entityNum)
        {
            Debug.Log($"Getting Entity {entityNum} of Player {playerNum}");

            Player player = GetPlayerByPlayerNum(playerNum);

            if (player == null)
            {
                return Entity.noEntity;
            }

            if (player.entities == null)
            {
                return Entity.noEntity;
            }

            if (entityNum >= player.entities.Count)
            {
                return Entity.noEntity;
            }

            return player.entities[(int)entityNum];
        }

        public bool ReceiveUserAction(UserAction.UserAction userAction)
        {

            if (userAction.playerNum != currentPlayer.playerNum)
            {
                return false;
            }

            switch (userAction)
            {
                case EndTurnUserAction endTurnUserAction:
                    return HandleUserAction(endTurnUserAction);

                case MoveEntityUserAction moveEntityUserAction:
                    return HandleUserAction(moveEntityUserAction);

                case AtkWithEntityUserAction atkWithEntityUserAction:
                    return HandleUserAction(atkWithEntityUserAction);

                default:
                    break;
            }


            return false;
        }


        public bool HandleUserAction(EndTurnUserAction endTurnUserAction)
        {
            var endPlayerTurnAction = new PlayerEndTurnAction(currentPlayer, null);
            PileAction(endPlayerTurnAction);
            return endPlayerTurnAction.wasPerformed;
        }


        public bool HandleUserAction(MoveEntityUserAction moveEntityUserAction)
        {
            Entity entity = GetEntityByEntityNumAndPlayerNum(
                moveEntityUserAction.playerNum,
                moveEntityUserAction.entityNum
            );

            if (entity == Entity.noEntity)
            {
                return false;
            }

            Tile tile = board.NextTileInDirection(entity.currentTile, moveEntityUserAction.direction);

            if (tile == Tile.noTile)
            {
                return false;
            }

            if (!entity.CanMoveByChangingDirection(tile))
            {
                return false;
            }

            entity.TryToCreateEntityUseMovementAction(
                tile.Distance(entity.currentTile) * entity.costToMove.mouvementCost,
                out EntityUseMovementAction useMovementAction);

            entity.TryToCreateEntityMoveAction(tile, useMovementAction, out EntityMoveAction entityMoveAction);

            return entityMoveAction.wasPerformed;
        }


        public bool HandleUserAction(AtkWithEntityUserAction atkWithEntityUserAction)
        {
            Entity entity = GetEntityByEntityNumAndPlayerNum(
                atkWithEntityUserAction.playerNum,
                atkWithEntityUserAction.entityNum
            );

            if (entity == Entity.noEntity)
            {
                return false;
            }

            entity.GetTilesAndEntitiesAffectedByAtk(
                out Entity[] attackedEntities,
                out Tile[] _
            );

            if (attackedEntities.Length < 1)
            {
                return false;
            }

            //Temp attackedEntities[0] maybe some attacks can hit multiple entities
            // remove CanAttackByChangingDirection just need CanAttack
            if (!entity.CanAttackByChangingDirection(attackedEntities[0]))
            {
                return false;
            }

            entity.TryToCreateEntityUseMovementAction(entity.costToAtk.mouvementCost, out EntityUseMovementAction entityUseMovementAction);
            entity.TryToCreateEntityPayHeartCostAction(entity.costToAtk.heartCost, out EntityPayHeartCostAction entityPayHeartCostAction);

            if (!entityPayHeartCostAction.wasPerformed || !entityUseMovementAction.wasPerformed)
            {
                return false;
            }

            //Temp attackedEntities[0] maybe some attacks can hit multiple entities
            entity.TryToCreateEntityAttackAction(attackedEntities[0], out EntityAttackAction entityAttackAction, entityPayHeartCostAction);

            return entityAttackAction.wasPerformed;
        }

        public void PileActions(GameAction.Action[] actions)
        {
            foreach (var action in actions)
            {
                if (actionPile.Count < maxPileCount)
                {
                    actionPile.Add(action);
                    Debug.Log($"Piling {action}");
                }
                else
                {
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

        private Action DequeueDepiledActionQueue(){
            if(depiledActionQueue.Count == 0){
                return null;
            }
            var action = depiledActionQueue[0];
            depiledActionQueue.RemoveAt(0);

            return action;
        }

        public ActionState DequeueDepiledActionQueueAndGetActionState()
        {
            var action = DequeueDepiledActionQueue();
            if (action != null)
            {
                return action.ToActionState();
            }
            return null;
            
        }

        private void SetupPermanentEffects()
        {

        }

        public GameState.GameState ToGameState(){
            //Only send what has changed. If reco send everything

            GameState.GameState gameState = new GameState.GameState();

            gameState.boardState = board.ToBoardState();
            gameState.turn = turn;
            gameState.currentPlayerNum = currentPlayer.playerNum;

            gameState.playerStates = new List<PlayerState>();
            foreach (Player player in players){
                gameState.playerStates.Add(player.ToPlayerState());
            }

            gameState.effectStates = new List<EffectState>();
            foreach (Effect effect in effects){
                gameState.effectStates.Add(effect.ToEffectState());
            }


            return gameState;
        }
    }
}
