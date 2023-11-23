using System;

public class Heart: IEquatable<Heart>{
    public HeartType firstHalfHeartType;

    public HeartType secondHalfHeartType;

    public bool isHalf{
        get {return (secondHalfHeartType == HeartType.NoHeart || secondHalfHeartType == HeartType.RedEmpty) && firstHalfHeartType != HeartType.NoHeart && firstHalfHeartType != HeartType.RedEmpty;}
    }

    public bool isEmpty{
        get{
            return (secondHalfHeartType == HeartType.RedEmpty && firstHalfHeartType == HeartType.RedEmpty) || (secondHalfHeartType == HeartType.NoHeart && firstHalfHeartType == HeartType.NoHeart);
        }
    }

    public Heart(HeartType firstHalf, HeartType secondHalf){
        firstHalfHeartType = firstHalf;
        secondHalfHeartType = secondHalf;
    }

    public bool CheckValidity(){
        return true;
    }

    public bool Equals(Heart other){
        if(other == null)
            return false;

        return firstHalfHeartType == other.firstHalfHeartType && secondHalfHeartType == other.secondHalfHeartType;

    }

    //return damage taken
    public int TakeDamage(int damage){

        if(damage <= 0){
            return 0;
        }

        if(isEmpty){
            return 0;
        }

        if(isHalf){
            if(firstHalfHeartType == HeartType.Red){
                firstHalfHeartType = HeartType.RedEmpty;
            }
            else{
                firstHalfHeartType = HeartType.NoHeart;
            }
            return 1;
        }

        if(damage == 1){
            if(secondHalfHeartType == HeartType.Red){
                secondHalfHeartType = HeartType.RedEmpty;
            }
            else{
                secondHalfHeartType = HeartType.NoHeart;
            }

            return 1;
        }

        if(secondHalfHeartType == HeartType.Red && firstHalfHeartType == HeartType.Red){
            firstHalfHeartType = HeartType.RedEmpty;
            secondHalfHeartType = HeartType.RedEmpty;
        }
        else{
            firstHalfHeartType = HeartType.NoHeart;
            secondHalfHeartType = HeartType.NoHeart;
        }

        return 2;
    }

    public override string ToString(){
        return $"({firstHalfHeartType}/{secondHalfHeartType})";
    }

}
