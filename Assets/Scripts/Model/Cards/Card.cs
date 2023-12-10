

public class Card{

    public bool needsEntityTarget{
        get;
        protected set;
    }

    public bool needsTileTarget{
        get;
        protected set;
    }

    public Player player{
        get;
        protected set;
    }

    public Cost cost{
        get;
        protected set;
    }

    public string name{
        get;
        protected set;
    }

    public string text{
        get;
        protected set;
    }

    public Card(Player player, Cost cost, string name, string text, bool needsTileTarget = false, bool needsEntityTarget = false){
        this.player = player;
        this.cost = cost;
        this.name = name;
        this.text = text;
        this.needsTileTarget = needsTileTarget;
        this.needsEntityTarget = needsEntityTarget;
    }

    public bool TryToActivate(Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity){
        
        return false;
    }

    public virtual bool Activate(){
        return true;
    }

    public virtual bool CanBeActivated(Player player){
        
        return false;
    }
}
