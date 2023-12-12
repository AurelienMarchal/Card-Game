

using UnityEngine;

public class Health{
    public Heart[] hearts{
        get;
        private set;
    }


    public Health(int maxNumberOfHeart = 12){
        hearts = new Heart[maxNumberOfHeart];
        for(var i = 0; i < maxNumberOfHeart; i++){
            hearts[i] = new Heart(HeartType.NoHeart, HeartType.NoHeart);
        }
    }

    public Health(Heart[] hearts){
        this.hearts = hearts;
    }

    public bool IsEmpty(){
        foreach(Heart heart in hearts){
            if(!heart.isEmpty){
                return false;
            }
        }
        return true;
    }

    public override string ToString(){
        var toReturn = "Health : ";

        foreach(Heart heart in hearts){
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
            var damageTaken = hearts[currentHeartIndex].TakeDamage(damageLeft);
            //Debug.Log($"Damage taken on heart with index {currentHeartIndex} : {damageTaken}");
            damageLeft -= damageTaken;

            if(damageTaken == 0){
                currentHeartIndex --;
            }
        }

        return IsEmpty();
    }



}



