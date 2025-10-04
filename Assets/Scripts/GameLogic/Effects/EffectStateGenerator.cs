

namespace GameLogic
{
    using GameState;

    namespace GameEffect
    {
        public static class EffectStateGenerator
        {
            public static EffectState GenerateEffectState(Effect effect)
            {
                var effectState = new EffectState();

                
                //effectState.canBeActivated = effect.CanBeActivated();
                effectState.effectText = effect.GetEffectText();

                

                return effectState;
            }
        }
    }
}
