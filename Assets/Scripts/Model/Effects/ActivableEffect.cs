public class ActivableEffect : EntityEffect
{

    public Cost cost{
        get;
        protected set;
    }

    public bool costPaid{
        get;
        protected set;
    }

    public ActivableEffect(Entity entity, Cost cost) : base(entity){
        this.cost = cost;
    }

    public override bool CanBeActivated(){
        return base.CanBeActivated() && associatedEntity.player.CanPayCost(cost) || costPaid;
    }

    public override bool TryToCreateEffectActivatedAction(bool depile, Action costAction, out EffectActivatedAction effectActivatedAction){
        effectActivatedAction = new EffectActivatedAction(this, costAction);
        costPaid = costAction.wasPerformed;
        var canBeActivated = CanBeActivated();
        if(canBeActivated){
            this.effectActivatedAction = effectActivatedAction;
            Game.currentGame.PileAction(effectActivatedAction, depile);
        }
        
        return canBeActivated;
    }
}
