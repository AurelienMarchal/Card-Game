using Newtonsoft.Json;

namespace GameLogic
{
    namespace GameState
    {   
        //TODO add a flag to know if the action was cancelled or peformed 
        [JsonConverter(typeof(ActionStateConverter))]
        public abstract class ActionState
        {
            public ActionState() {
            }
        }
    }
}