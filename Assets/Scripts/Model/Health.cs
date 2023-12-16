using System;
using UnityEngine;


[Serializable]
public class Health{
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

    public override string ToString(){
        var toReturn = "Health : ";

        foreach(HeartType heart in hearts){
            toReturn += heart;
        }

        return toReturn + $" vide {IsEmpty()}";
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
                    damageTaken = 1;
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



}



