using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameLogic{

    namespace GameAction{
        public class Action{

            public bool wasPerformed{
                get;
                protected set;
            }

            public bool wasCancelled{
                get;
                protected set;
            }

            public Action requiredAction{
                get;
                protected set;
            }

            public Action(Action requiredAction = null){
                wasPerformed = false;
                wasCancelled = false;
                this.requiredAction = requiredAction;
            }

            public bool TryToPerform(){
                var canPerform = CanPerform();

                if(canPerform){
                    wasPerformed = Perform();
                }

                return wasPerformed;
            }

            private bool CanPerform(){
                if(requiredAction != null){
                    if(!requiredAction.wasPerformed){
                        Debug.Log("Required action was not performed");
                        return false;
                    }

                    if(requiredAction.wasCancelled){
                        Debug.Log("Required action was cancelled");
                        return false;
                    }
                }

                return !wasCancelled && !wasPerformed;
            }


            protected virtual bool Perform(){
                return true;
            }

            public bool Cancel(){
                if(!wasPerformed){
                    wasCancelled = true;
                }

                return wasCancelled;
            }
            
        }
    }
}