public class SpellCard : Card
{

    public SpellCard(Player player, Cost cost, string cardName, string text, bool needsTileTarget = false, bool needsEntityTarget = false) : base(player, cost, cardName, text, needsTileTarget, needsEntityTarget){

    }

    public SpellCard(Player player, ScriptableSpellCard scriptableSpellCard) : base(player, scriptableSpellCard){

    }
}