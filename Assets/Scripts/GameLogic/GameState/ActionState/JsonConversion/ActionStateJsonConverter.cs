

using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameLogic.GameState
{
    public class ActionStateConverter : JsonConverter<ActionState>
    {
        public override ActionState ReadJson(JsonReader reader, Type objectType, ActionState existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            string typeName = jo["type"]?.ToString();

            if (!ActionStateRegistry.Types.TryGetValue(typeName, out Type targetType))
                throw new JsonSerializationException($"Unknown GameAction type: {typeName}");

            ActionState instance = (ActionState)Activator.CreateInstance(targetType);
            serializer.Populate(jo.CreateReader(), instance);
            return instance;
        }


        public override void WriteJson(JsonWriter writer, ActionState value, JsonSerializer serializer)
        {
            //Crashes Unity
            //JObject jo = JObject.FromObject(value);

            var jo = new JObject();

            string typeName = ActionStateRegistry.Types.FirstOrDefault(x => x.Value == value.GetType()).Key;

            //if (typeName != null)
            //jo.AddFirst(new JProperty("type", typeName));

            if (typeName == null)
            {
                throw new JsonSerializationException($"Unregistered type: {value.GetType().Name}");
            }
            
            jo["type"] = typeName;

            if (value is StartTurnActionState startTurnActionState)
            {
                jo["newTurnCount"] = startTurnActionState.newTurnCount;
            }

            if (value is PlayerActionState playerActionState)
            {
                jo["playerNum"] = playerActionState.playerNum;
                if (value is PlayerIncreaseMaxManaActionState playerIncreaseMaxManaActionState)
                {
                    jo["newMaxMana"] = playerIncreaseMaxManaActionState.newMaxMana;
                }
                if (value is PlayerUseManaActionState playerUseManaActionState)
                {
                    jo["newManaLeft"] = playerUseManaActionState.newManaLeft;
                }
                if (value is PlayerResetManaActionState playerResetManaActionState)
                {
                    jo["newManaLeft"] = playerResetManaActionState.newManaLeft;
                }
                if (value is PlayerPlayCardActionState playerPlayCardActionState)
                {
                    jo["card"] = JToken.FromObject(playerPlayCardActionState.card);
                    jo["targetTileNum"] = playerPlayCardActionState.targetTileNum;
                    jo["targetEntityNum"] = playerPlayCardActionState.targetEntityNum;
                    jo["targetEntityPlayerNum"] = playerPlayCardActionState.targetEntityPlayerNum;
                }
                if (value is PlayerAddCardToHandActionState playerAddCardToHandActionState)
                {
                    jo["card"] = JToken.FromObject(playerAddCardToHandActionState.card);
                    jo["position"] = playerAddCardToHandActionState.position;
                    jo["newHandState"] = JToken.FromObject(playerAddCardToHandActionState.newHandState);
                }
                if (value is PlayerDrawCardActionState playerDrawCardActionState)
                {
                    jo["card"] = JToken.FromObject(playerDrawCardActionState.card);
                    jo["cardWasAddedToHand"] = playerDrawCardActionState.cardWasAddedToHand;
                }

                if (value is PlayerSpawnEntityActionState playerSpawnEntityActionState)
                {
                    jo["entitySpawned"] = JToken.FromObject(playerSpawnEntityActionState.entitySpawned);
                    jo["tileNum"] = playerSpawnEntityActionState.tileNum;
                }
            }
            if (value is EntityActionState entityActionState)
            {
                jo["playerNum"] = entityActionState.playerNum;
                jo["entityNum"] = entityActionState.entityNum;
                if (entityActionState is EntityMoveActionState entityMoveActionState)
                {
                    jo["startTileNum"] = entityMoveActionState.startTileNum;
                    jo["endTileNum"] = entityMoveActionState.endTileNum;
                }
                if (entityActionState is EntityAttackActionState entityAttackActionState)
                {
                    jo["attackedEntityNum"] = entityAttackActionState.attackedEntityNum;
                    jo["attackedEntityPlayerNum"] = entityAttackActionState.attackedEntityPlayerNum;
                    jo["isCounterAttack"] = entityAttackActionState.isCounterAttack;
                }
                if (entityActionState is EntityChangeDirectionActionState entityChangeDirectionActionState)
                {
                    jo["newDirection"] = JToken.FromObject(entityChangeDirectionActionState.newDirection);
                }
                if (entityActionState is EntityUseMovementActionState entityUseMovementActionState)
                {
                    jo["movementUsed"] = entityUseMovementActionState.movementUsed;
                    jo["newMovementLeft"] = entityUseMovementActionState.newMovementLeft;
                }
                if (entityActionState is EntityResetMovementActionState entityResetMovementActionState)
                {
                    jo["newMovementLeft"] = entityResetMovementActionState.newMovementLeft;
                }
                if (entityActionState is EntityTakesDamageActionState entityTakesDamageActionState)
                {
                    jo["damageState"] = JToken.FromObject(entityTakesDamageActionState.damageState);
                    jo["newHealthState"] = JToken.FromObject(entityTakesDamageActionState.newHealthState);
                }
                if (entityActionState is EntityGainHeartActionState entityGainHeartActionState)
                {
                    jo["heartType"] = JToken.FromObject(entityGainHeartActionState.heartType);
                    jo["newHealthState"] = JToken.FromObject(entityGainHeartActionState.newHealthState);
                }
                if (entityActionState is EntityHealsActionState entityHealsActionState)
                {
                    jo["numberOfHeartsHealed"] = entityHealsActionState.numberOfHeartsHealed;
                    jo["newHealthState"] = JToken.FromObject(entityHealsActionState.newHealthState);
                }
                if (entityActionState is EntityPayHeartCostActionState entityPayHeartCostActionState)
                {
                    jo["heartCost"] = JToken.FromObject(entityPayHeartCostActionState.heartCost);
                    jo["newHealthState"] = JToken.FromObject(entityPayHeartCostActionState.newHealthState);
                }
                if (entityActionState is EntityIncreaseMaxMovementActionState entityIncreaseMaxMovementActionState)
                {
                    jo["newMaxMovement"] = entityIncreaseMaxMovementActionState.newMaxMovement;
                }
                
            }
            if (value is TileActionState tileActionState)
            {
                jo["tileNum"] = tileActionState.tileNum;

                if (tileActionState is TileChangeTypeActionState tileChangeTypeActionState)
                {
                    jo["newType"] = (int)tileChangeTypeActionState.newType;
                }
            }

            jo.WriteTo(writer);
        }
        
    }
}
