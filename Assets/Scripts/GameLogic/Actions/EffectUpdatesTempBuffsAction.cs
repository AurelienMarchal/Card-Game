namespace GameLogic{

    using GameEffect;
    using GameLogic.GameState;

    namespace GameAction{
        public class EffectUpdatesTempBuffsAction : EffectAction
        {

            public EffectUpdatesTempBuffsAction(Effect effect, Action requiredAction = null) : base(effect, requiredAction)
            {
            }
            protected override bool Perform()
            {
                if (effect is GivesTempBuffInterface givesTempBuffEffect)
                {
                    givesTempBuffEffect.UpdateTempBuffs();
                    return true;
                }
                return false;
            }
            
            public override ActionState ToActionState()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
