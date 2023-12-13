public struct Damage{

    public int amount{
        get;
        private set;
    }

    public Damage(int amount){
        this.amount = amount;
    }

    public override string ToString()
    {
        return $"Damage with amount {amount}";
    }

}