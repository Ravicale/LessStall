using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Randoms;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;

/**
 * Meditation
 * - Add Exile
 * - No longer includes debut.
 * +/- Generated card no longer has Ethereal and Exile, it instead has Replenish.
 * + Generated card is upgraded when this card is upgraded.
 * 
 * Make this work more closely to common attack/defense generators by giving you a freebee turn 1 that sticks around for the rest of the fight.
 * Try to mitigate lower quality of the card pool by giving the added card replenish (to reduce brick potential), and upgrading it when this is upgraded.
 */
namespace LessStall {
    [OverwriteVanilla]
    public sealed class BailianNingshenDef : CardTemplate {
        public override IdContainer GetId() {
            return nameof(LBoL.EntityLib.Cards.Neutral.White.BailianNingshen);
        }

        [DontOverwrite]
        public override CardImages LoadCardImages() {
            return null;
        }

        public override LocalizationOption LoadLocalization() {
            var loc = new GlobalLocalization(BepinexPlugin.embeddedSource);
            loc.LocalizationFiles.AddLocaleFile(LBoL.Core.Locale.En, "LocEn.yaml");
            return loc;
        }

        public override CardConfig MakeConfig() {
            var config = CardConfig.FromId(GetId());
            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;
            return config;
        }
    }

    [EntityLogic(typeof(BailianNingshenDef))]
    public sealed class BailianNingshen : Card {
        // Card No longer has a 'Debut' effect, so only return the base description.
        protected override string GetBaseDescription() {
            return base.GetBaseDescription();
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition) {
            Card[] cards = base.Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyCommon, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot, false), base.Value1, null);
            if (cards.Length != 0) {
                if (this.IsUpgraded) { // When this card is upgraded, upgrade all selectable options.
                    foreach (Card card in cards) {
                        card.Upgrade();
                    }
                }
                MiniSelectCardInteraction interaction = new MiniSelectCardInteraction(cards, false, false, false) {
                    Source = this
                };
                yield return new InteractionAction(interaction, false);
                Card selectedCard = interaction.SelectedCard;
                selectedCard.SetTurnCost(base.Mana);
                selectedCard.IsReplenish = true;
                yield return new AddCardsToHandAction(new Card[] { selectedCard });
                // Remove debut effect.
            }
            yield break;
        }
    }
}
