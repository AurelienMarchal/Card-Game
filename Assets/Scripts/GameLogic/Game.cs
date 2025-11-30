
using System.Collections.Generic;
using UnityEngine;
using System;


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

        //Dict Effect by effect id
        public Dictionary<Guid, Effect> effectsById{
            get;
            private set;
        }

        public Dictionary<Type, List<CanBeActivatedInterface>> canActivateByTriggerEffects{
            get;
            private set;
        }

        //------- Effects by Interface -------

        public Dictionary<Type, List<AffectsEntitiesInterface>> canUpdateEntitiesAffectedByTriggerEffects{
            get;
            private set;
        }

        public Dictionary<Type, List<AffectsTilesInterface>> canUpdateTilesAffectedByTriggerEffects{
            get;
            private set;
        }

        public Dictionary<Type, List<GivesTempBuffInterface>> canUpdateTempBuffsByTriggerEffects{
            get;
            private set;
        }

        public Dictionary<Type, List<DealsDamageInterface>> canUpdateDamageByTriggerEffects{
            get;
            private set;
        }

        public Dictionary<Type, List<HasRangeInterface>> canUpdateRangeByTriggerEffects{
            get;
            private set;
        }

        public Dictionary<Type, List<HasCostInterface>> canUpdateCostByTriggerEffects{
            get;
            private set;
        }

        //------- Effects by Game Object -------

        public Dictionary<Entity, List<EntityEffect>> entityEffects{
            get;
            private set;
        }

        public Dictionary<Player, List<PlayerEffect>> playerEffects{
            get;
            private set;
        }

        public Dictionary<Tile, List<TileEffect>> tilesEffects{
            get;
            private set;
        }

        public List<BoardEffect> boardEffects
        {
            get;
            private set;
        }

        //Same with the rest

        //Dict Effect by entity

        //Dict Effect by tile

        

        private List<Action> actionPile;

        [Obsolete]
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


            effectsById = new Dictionary<Guid, Effect>();

            entityEffects = new Dictionary<Entity, List<EntityEffect>>();
            playerEffects = new Dictionary<Player, List<PlayerEffect>>();
            tilesEffects = new Dictionary<Tile, List<TileEffect>>();
            boardEffects = new List<BoardEffect>();

            canActivateByTriggerEffects = new Dictionary<Type, List<CanBeActivatedInterface>>();
            canUpdateEntitiesAffectedByTriggerEffects = new Dictionary<Type, List<AffectsEntitiesInterface>>();
            canUpdateTilesAffectedByTriggerEffects = new Dictionary<Type, List<AffectsTilesInterface>>();
            canUpdateTempBuffsByTriggerEffects = new Dictionary<Type, List<GivesTempBuffInterface>>();
            canUpdateDamageByTriggerEffects = new Dictionary<Type, List<DealsDamageInterface>>();
            canUpdateRangeByTriggerEffects = new Dictionary<Type, List<HasRangeInterface>>();
            canUpdateCostByTriggerEffects = new Dictionary<Type, List<HasCostInterface>>();
            
            //depiledActionQueue = new List<Action>();
            actionStatesToSendQueue = new List<ActionState>();
            random = new System.Random(0);
            


            
            depileStarted = false;
            for (uint i = 0; i < numberOfPlayer; i++)
            {
                players[i] = new Player(i, new uint[]{0, 0, 0, 0, 0, 1, 2}, random);
            }

            SetupPermanentGameEffects();
            SetupPermanentPlayerEffects();
            SetupPermanentBoardEffects();
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

            Effect foundEffect = null;

            //temp
            var guid = new Guid(activateEntityEffectUserAction.effectId);

            if (effectsById.ContainsKey(guid))
            {
                //Hum
                foundEffect = effectsById[guid];
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
                        CheckForEffectsToAdd(action);
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
                        catch (NotImplementedException exeption)
                        {
                            Debug.LogError(exeption);
                        }

                    }

                    if (wasPerformed)
                    {
                        //Debug.Log("Checking Trigger");
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

            Type actionType = action.GetType();

            if (canActivateByTriggerEffects.ContainsKey(actionType)){
                List<CanBeActivatedInterface> canActivateByActionTypeTriggerEffects = canActivateByTriggerEffects[actionType];

                foreach (var canActivateByActionTypeTriggerEffect in canActivateByActionTypeTriggerEffects)
                {
                    if (canActivateByActionTypeTriggerEffect.CheckTriggerToActivate(action))
                    {
                        //hum
                        PileAction(new EffectActivatesAction((Effect)canActivateByActionTypeTriggerEffect, action));
                    }
                }

            }

            if (canUpdateEntitiesAffectedByTriggerEffects.ContainsKey(actionType)){
                List<AffectsEntitiesInterface> canUpdateEntitiesAffectedByActionTypeTriggerEffects = canUpdateEntitiesAffectedByTriggerEffects[actionType];

                foreach (var canUpdateEntitiesAffectedByActionTypeTriggerEffect in canUpdateEntitiesAffectedByActionTypeTriggerEffects)
                {
                    if (canUpdateEntitiesAffectedByActionTypeTriggerEffect.CheckTriggerToUpdateEntitiesAffected(action))
                    {
                        //hum
                        PileAction(new EffectUpdatesEntitiesAffectedAction((Effect)canUpdateEntitiesAffectedByActionTypeTriggerEffect, action));
                    }
                }

            }

            if (canUpdateTilesAffectedByTriggerEffects.ContainsKey(actionType)){
                List<AffectsTilesInterface> canUpdateTilesAffectedByActionTypeTriggerEffects = canUpdateTilesAffectedByTriggerEffects[actionType];

                foreach (var canUpdateTilesAffectedByActionTypeTriggerEffect in canUpdateTilesAffectedByActionTypeTriggerEffects)
                {
                    if (canUpdateTilesAffectedByActionTypeTriggerEffect.CheckTriggerToUpdateTilesAffected(action))
                    {
                        //hum
                        PileAction(new EffectUpdatesTilesAffectedAction((Effect)canUpdateTilesAffectedByActionTypeTriggerEffect, action));
                    }
                }

            }

            if (canUpdateTempBuffsByTriggerEffects.ContainsKey(actionType)){
                List<GivesTempBuffInterface> canUpdateTempBuffsByActionTypeTriggerEffects = canUpdateTempBuffsByTriggerEffects[actionType];

                foreach (var canUpdateTempBuffsByActionTypeTriggerEffect in canUpdateTempBuffsByActionTypeTriggerEffects)
                {
                    if (canUpdateTempBuffsByActionTypeTriggerEffect.CheckTriggerToUpdateTempBuffs(action))
                    {
                        //hum
                        PileAction(new EffectUpdatesTempBuffsAction((Effect)canUpdateTempBuffsByActionTypeTriggerEffect, action));
                    }
                }

            }

            if (canUpdateDamageByTriggerEffects.ContainsKey(actionType)){
                List<DealsDamageInterface> canUpdateDamageByActionTypeTriggerEffects = canUpdateDamageByTriggerEffects[actionType];

                foreach (var canUpdateDamageByActionTypeTriggerEffect in canUpdateDamageByActionTypeTriggerEffects)
                {
                    if (canUpdateDamageByActionTypeTriggerEffect.CheckTriggerToUpdateDamage(action))
                    {
                        //hum
                        //PileAction(new EffectUpdateDamage((Effect)canUpdateDamageByActionTypeTriggerEffect, action));
                    }
                }

            }

            if (canUpdateRangeByTriggerEffects.ContainsKey(actionType)){
                List<HasRangeInterface> canUpdateRangeByActionTypeTriggerEffects = canUpdateRangeByTriggerEffects[actionType];

                foreach (var canUpdateRangeByActionTypeTriggerEffect in canUpdateRangeByActionTypeTriggerEffects)
                {
                    if (canUpdateRangeByActionTypeTriggerEffect.CheckTriggerToUpdateRange(action))
                    {
                        //hum
                        //PileAction(new EffectUpdateRange((Effect)canUpdateRangeByActionTypeTriggerEffect, action));
                    }
                }

            }

            if (canUpdateCostByTriggerEffects.ContainsKey(actionType)){
                List<HasCostInterface> canUpdateCostByActionTypeTriggerEffects = canUpdateCostByTriggerEffects[actionType];

                foreach (var canUpdateCostByActionTypeTriggerEffect in canUpdateCostByActionTypeTriggerEffects)
                {
                    if (canUpdateCostByActionTypeTriggerEffect.CheckTriggerToUpdateCost(action))
                    {
                        //hum
                        //PileAction(new EffectUpdateCost((Effect)canUpdateCostByActionTypeTriggerEffect, action));
                    }
                }

            }

            /*
            foreach (Effect effect in currentGame.effectsById.Values)
            {
                CheckTriggers(effect, action);
            }
            */
        }

        [Obsolete]
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

            if (effect is GivesTempBuffInterface givesTempBuffEffect)
            {
                if (givesTempBuffEffect.CheckTriggerToUpdateTempBuffs(action))
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

        [Obsolete]
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

        [Obsolete]
        public ActionState DequeueDepiledActionQueueAndGetActionState()
        {
            var action = DequeueDepiledActionQueue();
            if (action != null)
            {
                return action.ToActionState();
            }
            return null;

        }

        private void CheckForEffectsToAdd(Action action)
        {
            switch (action)
            {
                case PlayerSpawnEntityAction playerSpawnEntityAction:
                    SetupPermanentEntityEffects(playerSpawnEntityAction.entity);
                    break;

                //Maybe add effects when adding cards to the deck
                case PlayerPlayCardAction playerPlayCardAction:
                    
                    AddEffects(playerPlayCardAction.card.GetEffects());
                    break;

                //case AddEffectAction
                
            }
        }
        
        public void AddEffect(Effect effect)
        {
            if (effectsById.ContainsKey(effect.id))
            {
                return;
            }

            effectsById.Add(effect.id, effect);

            

            if(effect is CanBeActivatedInterface canBeActivatedByTriggerEffect)
            {
                var actionTypeTriggersToActivate = canBeActivatedByTriggerEffect.ActionTypeTriggersToActivate();
                if(actionTypeTriggersToActivate != null)
                {
                    foreach (var actionTypeTrigger in actionTypeTriggersToActivate)
                    {
                        if (!canActivateByTriggerEffects.ContainsKey(actionTypeTrigger))
                        {
                            canActivateByTriggerEffects.Add(actionTypeTrigger, new List<CanBeActivatedInterface>());
                        }
                        canActivateByTriggerEffects[actionTypeTrigger].Add(canBeActivatedByTriggerEffect);
                    }
                }
            }

            if(effect is AffectsEntitiesInterface canUpdateEntitiesAffectedByTriggerEffect)
            {
                var actionTypeTriggersToActivate = canUpdateEntitiesAffectedByTriggerEffect.ActionTypeTriggersToUpdateEntitiesAffected();
                if(actionTypeTriggersToActivate != null)
                {
                    foreach (var actionTypeTrigger in actionTypeTriggersToActivate)
                    {
                        if (!canUpdateEntitiesAffectedByTriggerEffects.ContainsKey(actionTypeTrigger))
                        {
                            canUpdateEntitiesAffectedByTriggerEffects.Add(actionTypeTrigger, new List<AffectsEntitiesInterface>());
                        }
                        canUpdateEntitiesAffectedByTriggerEffects[actionTypeTrigger].Add(canUpdateEntitiesAffectedByTriggerEffect);
                    }
                }
            }

            if(effect is AffectsTilesInterface canUpdateTilesAffectedByTriggerEffect)
            {
                var actionTypeTriggersToActivate = canUpdateTilesAffectedByTriggerEffect.ActionTypeTriggersToUpdateTilesAffected();
                if(actionTypeTriggersToActivate != null)
                {
                    foreach (var actionTypeTrigger in actionTypeTriggersToActivate)
                    {
                        if (!canUpdateTilesAffectedByTriggerEffects.ContainsKey(actionTypeTrigger))
                        {
                            canUpdateTilesAffectedByTriggerEffects.Add(actionTypeTrigger, new List<AffectsTilesInterface>());
                        }
                        canUpdateTilesAffectedByTriggerEffects[actionTypeTrigger].Add(canUpdateTilesAffectedByTriggerEffect);
                    }
                }
            }

            if(effect is GivesTempBuffInterface canUpdateTempBuffsByTriggerEffect)
            {
                var actionTypeTriggersToActivate = canUpdateTempBuffsByTriggerEffect.ActionTypeTriggersToUpdateTempBuffs();
                if(actionTypeTriggersToActivate != null)
                {
                    foreach (var actionTypeTrigger in actionTypeTriggersToActivate)
                    {
                        if (!canUpdateTempBuffsByTriggerEffects.ContainsKey(actionTypeTrigger))
                        {
                            canUpdateTempBuffsByTriggerEffects.Add(actionTypeTrigger, new List<GivesTempBuffInterface>());
                        }
                        canUpdateTempBuffsByTriggerEffects[actionTypeTrigger].Add(canUpdateTempBuffsByTriggerEffect);
                    }
                }
            }

            if(effect is DealsDamageInterface canUpdateDamageByTriggerEffect)
            {
                var actionTypeTriggersToActivate = canUpdateDamageByTriggerEffect.ActionTypeTriggersToUpdateDamage();
                if(actionTypeTriggersToActivate != null)
                {
                    foreach (var actionTypeTrigger in actionTypeTriggersToActivate)
                    {
                        if (!canUpdateDamageByTriggerEffects.ContainsKey(actionTypeTrigger))
                        {
                            canUpdateDamageByTriggerEffects.Add(actionTypeTrigger, new List<DealsDamageInterface>());
                        }
                        canUpdateDamageByTriggerEffects[actionTypeTrigger].Add(canUpdateDamageByTriggerEffect);
                    }
                }
            }

            if(effect is HasRangeInterface canUpdateRangeByTriggerEffect)
            {
            var actionTypeTriggersToActivate = canUpdateRangeByTriggerEffect.ActionTypeTriggersToUpdateRange();
                if(actionTypeTriggersToActivate != null)
                {
                    foreach (var actionTypeTrigger in actionTypeTriggersToActivate)
                    {
                        if (!canUpdateRangeByTriggerEffects.ContainsKey(actionTypeTrigger))
                        {
                            canUpdateRangeByTriggerEffects.Add(actionTypeTrigger, new List<HasRangeInterface>());
                        }
                        canUpdateRangeByTriggerEffects[actionTypeTrigger].Add(canUpdateRangeByTriggerEffect);
                    }
                }
            }

            if(effect is HasCostInterface canUpdateCostByTriggerEffect)
            {
            var actionTypeTriggersToActivate = canUpdateCostByTriggerEffect.ActionTypeTriggersToUpdateCost();
                if(actionTypeTriggersToActivate != null)
                {
                    foreach (var actionTypeTrigger in actionTypeTriggersToActivate)
                    {
                        if (!canUpdateCostByTriggerEffects.ContainsKey(actionTypeTrigger))
                        {
                            canUpdateCostByTriggerEffects.Add(actionTypeTrigger, new List<HasCostInterface>());
                        }
                        canUpdateCostByTriggerEffects[actionTypeTrigger].Add(canUpdateCostByTriggerEffect);
                    }
                }
            }
        }

        public void AddEffects(List<Effect> effects)
        {
            if(effects == null)
            {
                return;
            }

            foreach (var effect in effects)
            {
                switch (effect)
                {
                    
                    case PlayerEffect playerEffect:
                        AddEffect(playerEffect);
                        break;
                    case EntityEffect entityEffect:
                        AddEffect(entityEffect);
                        break;
                    case BoardEffect boardEffect:
                        AddEffect(boardEffect);
                        break;
                    case TileEffect tileEffect:
                        AddEffect(tileEffect);
                        break;
                    default:
                        AddEffect(effect);
                        break;
                }
                
            }
        }

        public void AddEffect(EntityEffect entityEffect)
        {
            //Debug.Log($"Adding Entity Effect : {entityEffect}");

            if (!entityEffects.ContainsKey(entityEffect.associatedEntity))
            {
                entityEffects.Add(entityEffect.associatedEntity, new List<EntityEffect>());
            }
            entityEffects[entityEffect.associatedEntity].Add(entityEffect);
            AddEffect((Effect)entityEffect);
        }

        public void AddEffect(PlayerEffect playerEffect)
        {
            AddEffect((Effect)playerEffect);
            //TODO
        }

        public void AddEffect(BoardEffect boardEffect)
        {
            AddEffect((Effect)boardEffect);
            //TODO
        }

        public void AddEffect(TileEffect tileEffect)
        {
            AddEffect((Effect)tileEffect);
            //TODO
        }

        public void RemoveEffect(Effect effect)
        {
            if (effectsById.ContainsKey(effect.id))
            {
                effectsById.Remove(effect.id);
            }

            //Remove everywhere
        }

        private void SetupPermanentGameEffects()
        {
            AddEffect(new EntityDiesWhenHealthIsEmpty());
        }

        private void SetupPermanentPlayerEffects()
        {
            foreach (var player in players)
            {
                SetupPermanentPlayerEffects(player);
            }
        }

        private void SetupPermanentPlayerEffects(Player player)
        {   
            AddEffect(new PlayerResetManaAtTurnStartPlayerEffect(player));
            AddEffect(new PlayerIncreaseMaxManaAtTurnStartPlayerEffect(player));
            AddEffect(new DrawCardAtTurnStartPlayerEffect(player));
        }

        public void SetupPermanentEntityEffects(Entity entity)
        {
            AddEffect(new EntityIsWeightedDownByStoneHeartEffect(entity));
        }

        private void SetupPermanentBoardEffects()
        {
            
            foreach (var tile in board.tiles)
            {
                SetupPermanentTileEffects(tile);
            }
        }

        private void SetupPermanentTileEffects(Tile tile)
        {
            AddEffect(new PickNextCursedTileOnStartPlayerTurnTileEffect(tile));
            AddEffect(new ChangeWillGetCurseTypeIntoCursedTileEffect(tile));
            AddEffect(new CurseTileGivesCurseHeartEffect(tile));
            AddEffect(new CurseSourceTileGivesCurseHeartEffect(tile));
            AddEffect(new NatureTileGivesNatureHeartEffect(tile));
        }


        //Big TODO
        public void FromGameState()
        {
            throw new NotImplementedException();
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
            foreach (Effect effect in effectsById.Values)
            {
                gameState.effectStates.Add(EffectStateGenerator.GenerateEffectState(effect));
            }

            //Temp
            foreach (var listEntityEffectsByEntity in entityEffects)
            {
                PlayerState playerStateFound = null;
                foreach (PlayerState playerState in gameState.playerStates)
                {
                    if(playerState.playerNum == listEntityEffectsByEntity.Key.player.playerNum)
                    {
                        playerStateFound = playerState;
                        break;
                    }
                }

                if(playerStateFound == null)
                {
                    continue;
                }

                EntityState entityStateFound = null;

                foreach (var entityState in playerStateFound.entityStates)
                {
                    if(entityState.num == listEntityEffectsByEntity.Key.num)
                    {
                        entityStateFound = entityState;
                    }
                }

                if(entityStateFound == null)
                {
                    continue;
                }

                foreach (var entityEffect in listEntityEffectsByEntity.Value)
                {
                    entityStateFound.effectStates.Add(EffectStateGenerator.GenerateEffectState(entityEffect));
                }
            }


            return gameState;
        }
    }
}
