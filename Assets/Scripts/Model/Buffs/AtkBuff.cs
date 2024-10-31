using System;

public class AtkBuff : EntityBuff
{
    public int amount{
        get;
        private set;
    }

    public AtkBuff(int amount) : base("Atk Buff"){
        this.amount = amount;
    }


    public override string GetText(){
        return $"Atk {(Math.Sign(amount) >= 0 ? "increased" : "decreased")} by {amount}";
    }

    public override int IsPositive(){
        return Math.Sign(amount);
    }
}
