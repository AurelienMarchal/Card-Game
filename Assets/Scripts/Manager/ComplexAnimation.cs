using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class ComplexAnimation {

    public List<UnityAction> actions{
        get;
        private set;
    }


    public ComplexAnimation(){
        actions = new List<UnityAction>();
    }


    public void AddAction(UnityAction action){
        actions.Add(action);
    }

    public void PlayNextAction(){
        if(actions.Count == 0){
            return;
        }

        actions[0] += ActionsFinished;

        actions[0].Invoke();
        actions.RemoveAt(0);
    }

    private void ActionsFinished(){
        Debug.Log("Action finished");
    }
}
