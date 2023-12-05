

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

    public Card(bool needsTileTarget = false, bool needsEntityTarget = false){
        this.needsTileTarget = needsTileTarget;
        this.needsEntityTarget = needsEntityTarget;
    }

    public virtual bool TryToActivate(Player player, Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity){
        
        return false;
    }

public virtual bool CanBeActivated(Player player){
        
        return false;
    }
}
