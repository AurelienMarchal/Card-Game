

public class EffectActivatedAction : Action{
    
    Effect effect;

    public EffectActivatedAction(Effect effect, Action requiredAction = null) : base(requiredAction){
        this.effect = effect;
    }

    protected override bool Perform()
    {
        return effect.TryToActivate();
    }
}