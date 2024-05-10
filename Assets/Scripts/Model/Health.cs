using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


[Serializable]
public class Health : ICloneable{
    public HeartType[] hearts;

    public Health(int maxNumberOfHeart = 12){
        hearts = new HeartType[maxNumberOfHeart];
        for(var i = 0; i < maxNumberOfHeart; i++){
            hearts[i] = HeartType.NoHeart;
        }
    }

    public Health(HeartType[] hearts){
        this.hearts = hearts;
    }

    public bool IsEmpty(){
        foreach(HeartType heart in hearts){
            if(heart != HeartType.NoHeart || heart != HeartType.RedEmpty){
                return false;
            }
        }
        return true;
    }

    public bool IsFull(){
        foreach(HeartType heart in hearts){
            if(heart == HeartType.NoHeart){
                return false;
            }
        }
        return true;
    }

    public override string ToString(){
        var toReturn = "Health : ";

        foreach(HeartType heart in hearts){
            toReturn += heart;
        }

        return toReturn + $" vide {IsEmpty()}";
    }

    public void RemoveHeartAt(int heartInd){

        if(heartInd < 0 || heartInd >= hearts.Length){
            return;
        }

        hearts[hearts.Length - 1] = HeartType.NoHeart;

        for (int i = heartInd; i < hearts.Length - 1; i++){
            hearts[i] = hearts[i+1];
        }
    }

    public bool TakeDamage(Damage damage){
        if(IsEmpty()){
            return true;
        }

        var currentHeartIndex = hearts.Length - 1;

        var damageLeft = damage.amount;

        while(damageLeft > 0 && currentHeartIndex >= 0){
            //Debug.Log($"Damage left to be taken {damage}");
            
            var damageTaken = 0;

            switch(hearts[currentHeartIndex]){
                case HeartType.Red:
                    hearts[currentHeartIndex] = HeartType.RedEmpty;
                    damageTaken = 1;
                    break;

                case HeartType.RedEmpty:
                    break;

                case HeartType.NoHeart: 
                    break;

                default:
                    hearts[currentHeartIndex] = HeartType.NoHeart;
                    damageTaken = 1;
                    break;

            }

            //Debug.Log($"Damage taken on heart with index {currentHeartIndex} : {damageTaken}");
            damageLeft -= damageTaken;
            currentHeartIndex --;
            
        }

        return IsEmpty();
    }

    public bool TryToPayHeartCost(HeartType[] heartCost, bool canDie){
        
        var canPayHeartCost = CanPayHeartCost(heartCost, out bool willDie);

        if(canPayHeartCost && !(canDie && willDie)){
            PayHeartCost(heartCost);
        }

        return canPayHeartCost;
    }

    public bool CanPayHeartCost(HeartType[] heartCost, out bool willDie){

        willDie = false;

        if(heartCost.Length == 0){
            return true;
        }

        if(IsEmpty()){
            return false;
        }
        
        var heartCostCopy = new List<HeartType>(heartCost);

        var nbHeartsNotPayedAndNotEmpty = 0;

        foreach(HeartType heartType in hearts){
            if(heartCostCopy.Contains(heartType)){
                heartCostCopy.Remove(heartType);
                
            }
            else if(heartType != HeartType.NoHeart && heartType != HeartType.RedEmpty){
                nbHeartsNotPayedAndNotEmpty++;
            }
        }

        willDie = nbHeartsNotPayedAndNotEmpty == 0;

        return heartCostCopy.Count == 0;
    }

    private bool PayHeartCost(HeartType[] heartCost){
        if(IsEmpty()){
            return true;
        }

        var heartCostCopy = new List<HeartType>(heartCost);

        var currentHeartIndex = hearts.Length - 1;



        while(heartCostCopy.Count > 0 && currentHeartIndex >= 0){
            if(hearts[currentHeartIndex] == HeartType.Red || hearts[currentHeartIndex] == HeartType.RedEmpty){
                if(heartCostCopy.Contains(HeartType.Red)){
                    heartCostCopy.Remove(HeartType.Red);
                    //TO DO
                    //Remplacer par une action
                    RemoveHeartAt(currentHeartIndex);
                }
            }
            else if(heartCostCopy.Contains(hearts[currentHeartIndex])){
                heartCostCopy.Remove(hearts[currentHeartIndex]);
                RemoveHeartAt(currentHeartIndex);
            }

            else{
                currentHeartIndex --;
            }
        }

        return IsEmpty();
    }

    public bool TryToGainHeart(HeartType heartType){

        for(var i = 0; i < hearts.Length; i++){
            if(hearts[i] == HeartType.NoHeart){
                hearts[i] = heartType;
                return true;
            }
        }

        return false;

    }

    public object Clone(){
        var heartsClone = hearts.Clone() as HeartType[];
        var healthClone = new Health(heartsClone);
        return healthClone;
    }
}



