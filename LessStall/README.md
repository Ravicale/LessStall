## TL;DR:
Lost Branch of Legend contains a number of 1 card combos that grant persistent resources such as infinite healing, money, power, and/or deck removals. To perform these, one would find an enemy that lacks scaling (IE: Bats, Blue Ying Yang Orb) then sit there for an exceptionally long time replaying Breaking Boundaries, Generated Teammates, ect to accumulate resources until they have everything they want. [Not only is this play pattern exceptionally boring, it's also frequently optimal if you want to maximize your winrate.](https://youtu.be/7rqfbvnO_H0?feature=shared&t=1425). This mod attempts to change a number of cards to prevent break resource loops (usually by making them Exile themselves), while providing them with compensatory buffs to make them more attractive when used in non-degenerate play styles. While some resource loops still exist, they are now much harder to get due to requiring many disparate pieces to be intentionally assembled together.

## Manual Installation Steps
1. Ensure that you have the all dependencies installed:
   * BepInExPack
   * Watermark
   * Sideloader
2. Drop LessStall.dll into *Game Install Folder*\BepInEx\plugins.
Source code is available on [Github](https://github.com/Ravicale/LessStall/tree/master).

## All Card Changes Vs Vanilla
### Trace On
 * Add Exile
   > Breaks resource loops.
 * Gives 3 Phil Mana (from 2).
   > Increased mana gain to make the floor, even on a bad pool, a small amount of mana accelaration. Also makes it feel a bit less clunky to use, since the base version always gives enough mana to cast all options. Not giving more mana on upgrade though, then this starts to out-compete dedicated mana accelerants.

### Breaking Boundaries
 * Add Exile
   > Breaks resource loops.
 * Starts with 5 choices un-upgraded (from 3).
 * Reduce cost to 1W (from 2W).
   > Reduced cost and increased choices to make the cast more consistently useful. 
 * Upgrade adds Echo.
   > Upgrade now gives echo to give it back a little of the lost staying power, or enable a more explosive initial play.

### Remote Support
 * Add Exile
   > Breaks resource loops.
 * Starts with 5 choices un-upgraded (from 3).
 * Reduce cost to W (from 1W).
   > Reduced cost and increased choices to make the cast more consistently useful. 
 * Upgrade adds Echo.
   > Upgrade now gives echo to give it back a little of the lost staying power, or enable a more explosive initial play.

### Shikigami Protection
 * Add Exile
   > Breaks resource loops.
 * Add Echo
   > Add echo to give back a little of the lost staying power, or enable a more explosive initial play.
 * Upgrade reduces cost to 1(W/U) from WU. 
 * Slightly improved upgraded mana efficiency to increase attractiveness.

### Meditation
 * Add Exile
   > Breaks resource loops.
 * Upgrade no longer includes debut.
   > Does not make sense with Exile.
 * Generated card no longer has Ethereal and Exile, it instead has Replenish.
 * Generated card is upgraded when this card is upgraded. 
   > Make this work more closely to common attack/defense generators by giving you a freebee turn 1 that sticks around for the rest of the fight. Try to mitigate lower quality of the card pool by giving the added card replenish (to reduce brick potential), and upgrading it when this is upgraded.

### Spring is Here
 * Has Exile
   > Breaks resource loops.
 * Baselined cost change to 1 generic mana on added cards.
 * Upgrade makes added cards free.
   > Baselined cost reduction, and boosted it to free when upgraded to try and boost the raw power enough to compensate for lack of spam-ability.

### Fires of Hokkai
 * Status now reduces Skills to costing 1 instead of its previous effect.
   > New effect has no easy resource stall loops (though it does enable some spicy infinites when built around). Status has some overlap with Embrace the Darkness, but the lack of exiling and limitation to skills should keep them both feeling distinct enough.
 * Upgrade now adds 3 skills with Replenish to the deck.
   > Upgrade should help preserve the feel of the original card, and make it less of a dead pick in decks running a low density of skills.

### Unmoving Great Library
 * Has Exile
   > Breaks resource loops.
 * Has Echo
   > Add echo to give back a little of the lost staying power, or enable a more explosive initial play.
 * Upgrade makes red/blue mana Hybrid Cost
   > Upgrade is a little more tempting to compensate for nerf, and make it a bit easier to use both the Original and Echoed copies.

### Yukari, Boundary of Fantasy
 * Activated ability now gives 25 barrier instead of returning a card from exile.
   > To prevent easy reuse, exile recursion has been moved from her active ability to her ult. Barrier generation is lore accurate, and hopefully strong enough to be relevant.
 * Ultimate now lets you exile any number of cards from your hand, deck, or discard. Then return a card from exile to your hand. Previous ult provided a buff that added free 3 color rares to hand every turn.
   > Previously Yukari could be used as an infinite get-stuff-out-of-exile stall tool in addition having an ult that acts as a stall outlet itself. This still allows for stall strats- but now you need to pick up a number of other pieces to generate a renewable number of Yukaris, and have a payoff card in deck to loop in and out of exile. This requires extreme-going-out-of-the-way to do (Cirno + 2 off colors is the 'easiest'), and is unlikely to manifest accidentally in a playthrough. To keep her ultimate exciting, it now lets you exile literally anything and everything you want *before* recurring a card from exile. This has many uses- it can cleanse the deck of statuses/misfortunes, act as a rota/demonic tutor, recur a single power card, or even provide deck thinning setup for infinite combos. The cooking potential is very high here.

### Lily White, Herald of Spring
 * Ultimate now deals 5+unspent mana damage 3 times instead of restoring life. 
   > Lily was an infinite life stall-target with Teammate generators. Now she acts as a payoff+enabler for green big mana synergies. New ult also has symmetry with on-cast effect.

### Meiling, Neglectful Gatekeeper
 * Ult now grants 3(4) Spirit instead of 12(16) health. (Firepower is untouched)
   > Spirit is a reasonably strong defensive boost that has nice symmetry with firepower. The loss of healing seems anti-synergistic with the bonus trigger requirements, but you can still get those with blocked attacks so the loss isn't that huge.

### Patchouli, Elemental Alchemist
 * -4 Active no longer adds power, instead it adds an astrology to hand.
   > Power could be accumulated via stalling. Astrologies more closely match the 'potion' archetypal theme.
