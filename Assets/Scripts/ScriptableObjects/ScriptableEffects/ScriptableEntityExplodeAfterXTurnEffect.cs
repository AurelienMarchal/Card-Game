using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableEntityExplodeAfterXTurnEffect : ScriptableEffect{

    public int turnNB;



    public override Effect GetEffect(){
        return new EntityExplodeAfterXTurnEffect(turnNB, Entity.noEntity);
    }

}
