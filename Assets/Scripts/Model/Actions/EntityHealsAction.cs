public class EntityHealsAction : EntityAction{
    
    public int numberOfHeartsHealed{
        get;
        private set;
    }
    
    
    public EntityHealsAction(int numberOfHeartsHealed, Entity entity, Action requiredAction = null) : base(entity, requiredAction) {
        this.numberOfHeartsHealed = numberOfHeartsHealed;
    }



    protected override bool Perform(){
        var oneHeartWasHealed = false;
        for (int i = 0; i < numberOfHeartsHealed; i++){
            var didHeal = entity.health.TryToHeal();
            if(didHeal){
                oneHeartWasHealed = true;
            }
            else{
                break;
            }
        }


        return oneHeartWasHealed;
    }
}