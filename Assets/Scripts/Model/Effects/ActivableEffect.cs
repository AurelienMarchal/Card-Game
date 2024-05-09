public class ActivableEffect : EntityEffect
{

    public Cost cost{
        get;
        protected set;
    }

    protected bool costPaid;

    public ActivableEffect(Entity entity, Cost cost) : base(entity){
        this.cost = cost;
        costPaid = false;
    }

    public override bool CanBeActivated(){
        return base.CanBeActivated() && associatedEntity.CanPayHeartCost(cost.heartCost) && associatedEntity.CanUseMovement(cost.mouvementCost) || costPaid;
    }

    public override bool TryToCreateEffectActivatedAction(Action costAction, out EffectActivatedAction effectActivatedAction){
        effectActivatedAction = new EffectActivatedAction(this, costAction);
        costPaid = costAction.wasPerformed;
        var canBeActivated = CanBeActivated();
        if(canBeActivated){
            this.effectActivatedAction = effectActivatedAction;
            Game.currentGame.PileAction(effectActivatedAction);
        }

        costPaid = false;
        
        return canBeActivated;
    }
}
