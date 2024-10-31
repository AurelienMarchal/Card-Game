using System;

public class RangeBuff : EntityBuff
{
    public int amount{
        get;
        private set;
    }

    public RangeBuff(int amount) : base("Range Buff"){
        this.amount = amount;
    }


    public override string GetText(){
        return $"Range {(Math.Sign(amount) >= 0 ? "increased" : "decreased")} by {amount}";
    }

    public override int IsPositive(){
        return Math.Sign(amount);
    }
}
