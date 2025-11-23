
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

        [System.Obsolete]
        public List<Action> depiledActionQueue
        {
            get;
            private set;
        }

        public List<ActionState> actionStatesToSendQueue{
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

        public void SetUpGame(int numberOfPlayer, int boardHeight, int boardWidth /*ADD decklists*/ )
        {
            board = new Board(boardHeight, boardWidth);
            players = new Player[numberOfPlayer];
            actionPile = new List<Action>();
            
            //depiledActionQueue = new List<Action>();
            actionStatesToSendQueue = new List<ActionState>();
            random = new System.Random(0);
            effects = new List<Effect>();
            SetupPermanentEffects();
            depileStarted = false;
            for (uint i = 0; i < numberOfPlayer; i++)
            {
                players[i] = new Player(i, new uint[]{0, 0, 0, 0, 0, 1, 2}, random);
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

                case PlayCardFromHandUserAction playCardFromHandUserAction:
                    return HandleUserAction(playCardFromHandUserAction);

                case ActivateEntityEffectUserAction activateEntityEffectUserAction:
                    return HandleUserAction(activateEntityEffectUserAction);

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

            entity.TryToCreateEntityUseMovementAction(entity.costToMove.mouvementCost, out EntityUseMovementAction entityUseMovementAction);
            entity.TryToCreateEntityPayHeartCostAction(entity.costToMove.heartCost, out EntityPayHeartCostAction entityPayHeartCostAction);

            if (!entityPayHeartCostAction.wasPerformed || !entityUseMovementAction.wasPerformed)
            {
                return false;
            }

            entity.TryToCreateEntityMoveAction(tile, entityUseMovementAction, out EntityMoveAction entityMoveAction);

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
            if (!entity.CanAttack(attackedEntities[0]))
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


        public bool HandleUserAction(PlayCardFromHandUserAction playCardFromHandUserAction)
        {
            var player = GetPlayerByPlayerNum(playCardFromHandUserAction.playerNum);

            if (player == null)
            {
                return false;
            }

            var card = player.hand.GetCardInPosition(playCardFromHandUserAction.cardPositionInHand);

            if (card == null)
            {
                return false;
            }

            //TODO: Targets

            var targetTile = Tile.noTile;

            if (playCardFromHandUserAction.tileTargetNum >= 0)
            {
                targetTile = board.GetTileWithNum((uint)playCardFromHandUserAction.tileTargetNum);
            }

            var targetEntity = Entity.noEntity;

            if (playCardFromHandUserAction.entityTargetNum >= 0 && playCardFromHandUserAction.entityTargetPlayerNum >= 0)
            {
                targetEntity = GetEntityByEntityNumAndPlayerNum(
                    (uint)playCardFromHandUserAction.entityTargetPlayerNum,
                    (uint)playCardFromHandUserAction.entityTargetNum
                );
            }


            Debug.Log($"Card : {card}, Target Entity : {targetEntity}, Traget Tile : {targetTile}");

            if (!player.CanUseMana(card.cost.manaCost))
            {
                return false;
            }

            if (!player.CanPlayCard(card,
                targetTile: targetTile,
                targetEntity: targetEntity))
            {
                return false;
            }

            player.TryToCreatePlayerUseManaAction(card.cost.manaCost, out PlayerUseManaAction playerUseManaAction);

            if (!playerUseManaAction.wasPerformed)
            {
                return false;
            }
            player.TryToCreatePlayerPlayCardAction(
                card,
                out PlayerPlayCardAction playerPlayCardAction,
                costAction: playerUseManaAction,
                targetTile: targetTile,
                targetEntity: targetEntity
            );

            return playerPlayCardAction.wasPerformed;
        }
        
        public bool HandleUserAction(ActivateEntityEffectUserAction activateEntityEffectUserAction)
        {

            Entity entity = GetEntityByEntityNumAndPlayerNum(
                activateEntityEffectUserAction.playerNum,
                activateEntityEffectUserAction.entityNum
            );

            if (entity == Entity.noEntity)
            {
                return false;
            }

            Debug.Log($"Trying to activate effect {activateEntityEffectUserAction.effectId} of entity {entity}");

            EntityEffect foundEffect = null;

            //temp
            foreach (var entityEffect in entity.effects)
            {
                if (entityEffect.id.ToString() == activateEntityEffectUserAction.effectId)
                {
                    foundEffect = entityEffect;
                    break;
                }
            }

            if (foundEffect == null)
            {
                return false;
            }

            if (foundEffect is HasCostInterface hasCostEffect)
            {
                var cost = hasCostEffect.GetCost();

                if (!entity.CanPayCost(cost))
                {
                    return false;
                }

                entity.TryToCreateEntityUseMovementAction(cost.mouvementCost, out EntityUseMovementAction entityUseMovementAction);
                entity.TryToCreateEntityPayHeartCostAction(cost.heartCost, out EntityPayHeartCostAction entityPayHeartCostAction);

                if (!entityUseMovementAction.wasPerformed || !entityPayHeartCostAction.wasPerformed)
                {
                    return false;
                }
            }
            
            if(foundEffect is CanBeActivatedInterface canBeActivatedEffect)
            {
                if (!canBeActivatedEffect.CanBeActivated())
                {
                    return false;
                }

                var effectActivateAction = new EffectActivatesAction(foundEffect);
                PileAction(effectActivateAction);

                return effectActivateAction.wasPerformed;
            }

            return false;
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

        private void DepileActionPile()
        {
            var c = 0;
            depileStarted = true;
            while (actionPile.Count > 0 && c < 1000)
            {
                var action = actionPile[^1];

                Debug.Log("Depile start");
                Debug.Log($"action : {action}");

                //Debug.Log("Checking Trigger");

                CheckTriggers(action);

                //Debug.Log($"GameAction.Action pile : {string.Join( ",", actionPile)}");

                var newAction = actionPile[^1];

                //Debug.Log($"newAction : {newAction}");

                if (action == newAction)
                {

                    Debug.Log("Trying to perform action");
                    var wasPerformed = action.TryToPerform();

                    if (wasPerformed)
                    {
                        Debug.Log($"{action} was performed");
                    }
                    else
                    {
                        Debug.Log($"{action} was not performed");
                    }

                    //depiledActionQueue.Add(action);

                    actionPile.Remove(action);

                    if (action.wasPerformed || action.wasCancelled)
                    {
                        //Converting action to actionState the moment they were activated
                        try
                        {
                            actionStatesToSendQueue.Add(action.ToActionState());
                        }
                        catch (System.NotImplementedException exeption)
                        {
                            Debug.LogError(exeption);
                        }

                    }

                    if (wasPerformed)
                    {
                        Debug.Log("Checking Trigger");
                        CheckTriggers(action);
                    }
                }

                else
                {
                    c++;
                }

                //Debug.Log($"GameAction.Action pile : {string.Join( ",", actionPile)}");
            }

            //Debug.Log($"GameAction.Action performed pile : {string.Join( ",", depiledActionQueue)}");

            depileStarted = false;

            
        }

        //TODO Put effects in a Dictionnary<ActionType, List<Effect>> 
        // where all the effects that are triggered by the same type of action are in the same list 
        // in order to not check every effects when checking triggers but only the ones that are 
        // triggerd by the action

        void CheckTriggers(Action action){



            foreach (Effect effect in currentGame.effects)
            {
                CheckTriggers(effect, action);
            }

            foreach(Effect effect in currentGame.board.effects){
                CheckTriggers(effect, action);
            }
            
            foreach(Tile tile in currentGame.board.tiles){
                foreach(Effect effect in tile.effects){
                    CheckTriggers(effect, action);
                }
            }

            foreach (Player player in players)
            {
                foreach (Effect effect in player.effects)
                {
                    CheckTriggers(effect, action);
                }

                foreach (Entity entity in player.entities)
                {
                    foreach (Effect effect in entity.effects)
                    {
                        CheckTriggers(effect, action);
                    }
                }
            }
        }

        void CheckTriggers(Effect effect, Action action)
        {


            if (effect is CanBeActivatedInterface canBeActivatedEffect)
            {
                if (canBeActivatedEffect.CheckTriggerToActivate(action))
                {
                    if (canBeActivatedEffect.CanBeActivated())
                    {
                        PileAction(new EffectActivatesAction(effect, action));
                    }
                }
            }
            
            if (effect is AffectsEntitiesInterface affectsEntitiesEffect)
            {
                if (affectsEntitiesEffect.CheckTriggerToUpdateEntitiesAffected(action))
                {
                    PileAction(new EffectUpdatesEntitiesAffectedAction(effect, action));   
                }
            }

            if (effect is AffectsTilesInterface affectsTilesEffect)
            {
                if (affectsTilesEffect.CheckTriggerToUpdateTilesAffected(action))
                {
                    PileAction(new EffectUpdatesTilesAffectedAction(effect, action));
                }
            }

            if (effect is GivesTempEntityBuffInterface givesTempBuffEffect)
            {
                if (givesTempBuffEffect.CheckTriggerToUpdateTempEntityBuffs(action))
                {
                    PileAction(new EffectUpdatesTempBuffsAction(effect, action));
                }
            }
        }

        public ActionState DequeueActionStateToSendQueue()
        {
            if (actionStatesToSendQueue.Count == 0)
            {
                return null;
            }
            var actionState = actionStatesToSendQueue[0];
            actionStatesToSendQueue.RemoveAt(0);

            return actionState;
        }

        [System.Obsolete]
        private Action DequeueDepiledActionQueue()
        {
            if (depiledActionQueue.Count == 0)
            {
                return null;
            }
            var action = depiledActionQueue[0];
            depiledActionQueue.RemoveAt(0);

            return action;
        }

        [System.Obsolete]
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


        //Big TODO
        public void FromGameState()
        {
            throw new System.NotImplementedException();
        }

        public GameState.GameState ToGameState()
        {

            GameState.GameState gameState = new GameState.GameState();

            gameState.boardState = board.ToBoardState();
            gameState.turn = turn;
            gameState.currentPlayerNum = currentPlayer.playerNum;

            gameState.playerStates = new List<PlayerState>();
            foreach (Player player in players)
            {
                gameState.playerStates.Add(player.ToPlayerState());
            }

            gameState.effectStates = new List<EffectState>();
            foreach (Effect effect in effects)
            {
                //gameState.effectStates.Add(EffectStateGenerator.GenerateEffectState(effect));
            }


            return gameState;
        }
    }
}
