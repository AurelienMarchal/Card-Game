

public class RemoveEntityEffectFromEntityAction : EntityAction
{
    public EntityEffect effect{
        get; 
        private set; 
    }

    public RemoveEntityEffectFromEntityAction(Entity entity, EntityEffect effect, Action requiredAction = null) : base(entity, requiredAction)
    {
        this.effect = effect;
    }


    protected override bool Perform()
    {
        if(entity.effects.Contains(effect)){
            entity.RemoveEffect(effect);
            return true;
        }

        return false;
    }
}