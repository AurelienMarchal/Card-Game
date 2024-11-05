using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityExplodeAfterXTurnEffect : EntityEffect{


    public int turnLeft{
        get;
        protected set;
    }


    public EntityExplodeAfterXTurnEffect(int turnNb, Entity entity) : base(entity, true){
        turnLeft = turnNb;
    }

    public override bool Trigger(Action action){
        switch (action){
            case EndPlayerTurnAction endPlayerTurnAction:
                return endPlayerTurnAction.wasPerformed && endPlayerTurnAction.player == associatedEntity.player;
        }

        return false;
    }

    protected override void Activate(){
        turnLeft --;
        if(turnLeft == 0){
            //TODO
        }
    }


    public override string GetEffectText()
    {
        return $"Explode in {turnLeft} turns !! ";
    }
}
