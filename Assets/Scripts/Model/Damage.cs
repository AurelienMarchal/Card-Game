using System;

[Serializable]
public struct Damage{

    public int amount;

    public Damage(int amount){
        this.amount = amount;
    }

    public override string ToString()
    {
        return $"Damage with amount {amount}";
    }

}