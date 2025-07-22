using Newtonsoft.Json;

namespace GameLogic
{
    namespace GameState
    {   
        [JsonConverter(typeof(ActionStateConverter))]
        public abstract class ActionState
        {

        }
    }
}