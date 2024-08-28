public class Buff{
    
    public string name{
        get;
        private set;
    }

    public Buff(string name) {
        this.name = name;
    }

    public virtual string GetText(){
        return string.Empty;
    }

    public override string ToString() {
        return name + "buff";
    }

    public virtual int IsPositive(){
        return 0;
    }
}