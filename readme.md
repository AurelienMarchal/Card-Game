# Rules

## Energy

:large_blue_diamond::large_blue_diamond::large_blue_diamond::large_blue_diamond::large_blue_diamond:

Every `entity` has `Energy` ::small_orange_diamond:. `Energy` is spent to move or activate `entity` effects. 
Every `entity` has a max `energy` count. At the begining of each turn, every `entity` restore 1 (can be updgraded) `energy`

## Hearts

:heart::green_heart::white_heart::blue_heart:

Every `entity` has a certain combinaison of hearts. When they have no more hearts or have only empty red hearts they die.

## Attacks

- Entities need a weapon to attack. If they attack an `entity` : If the attacked `entity` faces the attacking `entity`, the attacked `entity` recieves damages and then fights back; otherwise, it cannot fight back.

## Weapons 

- Weapons have a range stat, an attack stat (and a durability ?) and cost of utilisation (how much energy is required to attack with it). Entities can attack with weapons if their target are in the range of their weapon. They deal the amount of attack of the weapon when they successfully attack.


# Ideas

Board needs to be 9x9 or 7x7 to have a middle tile for late game !! the middle tile might be immune to any modifications 

## Hearts

- `Red` :heart:: Can be healed
- `Nature` :green_heart:: 
- `Cursed` :purple_heart:: If a cursed heart is damaged, every adjacent cursed heart is destroyed aswell
- `Energy` :blue_heart:: Can be substituted for energy
- `Stone` :white_heart:: Tanks any amount of damage. Weights down the `entity`

## Tiles

- `Default` : does nothing
- `Nature` :green_square: : 
  - Can only play certain entities on it
  - Gives one :green_heart: when `entity` walk over it ?

- `Cursed` :purple_square::
  - Can only play certain entities on it
  - Tranform the right most heart into :purple_heart: when `entity` walk over it
- `Curse source` :purple_square::
    - At the start of every turn, chooses a tile to be converted to a curse tile at the end of the turn
  
- `Energy` :blue_square:: 
- `Stone` :white_large_square::
    - Cannot be changed
- `Pink` :red_square::
  - heals a red heart when `entity` walk over it


## Heroes

After leveling up `hero`, the player can unlocks card direclty associated to the `hero` (buddies in bg) that synergies only with the `hero` and that are useable only in a deck with the associated here

### Mage

### Cursed

### Druid

### Warior

### Toupie Man

- Envoie des toupies
- Fait la toupie et gagne des buffs en fct du nb de toupies

## Cards

- Spider Web : Cast on `entity` : the `entity` cannot move until the next turn

## Entities

- Archetype `Automate` : entities that move or attack automaticaly at the end of the turn but can't be moved or controlled by the player directly

## Weapons

- Bow 
- Sield

## Mechanics 

### for entities

- Flight : avoid every tile effect
- Weighted down : cost 1 more :large_blue_diamond: to move.

### for heroes

### for players

- Blindness : Can only see things next to the hero

### for the board

- Fog : Like Blindness for both players 




