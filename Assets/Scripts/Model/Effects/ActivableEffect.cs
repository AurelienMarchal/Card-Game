public class ActivableEffect : EntityEffect
{

    public Cost cost{
        get;
        protected set;
    }

    public ActivableEffect(Entity entity, Cost cost) : base(entity){
        this.cost = cost;
    }

    public bool EntityCanPayCost(){
        return associatedEntity!= Entity.noEntity && associatedEntity.CanPayHeartCost(cost.heartCost) && associatedEntity.CanUseMovement(cost.mouvementCost);
    }

    public override bool TryToCreateEffectActivatedAction(Action costAction, out EffectActivatedAction effectActivatedAction){
        effectActivatedAction = new EffectActivatedAction(this, costAction);

        var canBeActivated = CanBeActivated();
        if(canBeActivated){
            this.effectActivatedAction = effectActivatedAction;
            Game.currentGame.PileAction(effectActivatedAction);
        }
        
        return canBeActivated;
    }
}
