using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action{

    public bool wasPerfomed{
        get;
        protected set;
    }

    public Action(){
        wasPerfomed = false;
    }

    public virtual void Perform(){

        wasPerfomed = true;
    }
    
}
