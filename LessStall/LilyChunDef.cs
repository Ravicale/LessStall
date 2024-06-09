using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Randoms;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using System.Linq;

/**
 * Spring is Here
 * - Has Exile
 * + Baselined cost change to 1 generic mana on added cards.
 * + Upgrade makes added cards free.
 * 
 * Add exile to prevent ease of stalling for resource granting skills.
 * Baselined cost reduction, and boosted it to free when upgraded to try and boost the raw power enough to compensate.
 */
namespace LessStall {
    [OverwriteVanilla]
    public sealed class LilyChunDef : CardTemplate {
        public override IdContainer GetId() {
            return nameof(LBoL.EntityLib.Cards.Neutral.Green.LilyChun);
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
            config.Mana = new ManaGroup() { Any = 1 };
            config.UpgradedMana = new ManaGroup() { Any = 0 };
            return config;
        }
    }
    [EntityLogic(typeof(LilyChunDef))]
    public sealed class LilyChun : Card {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition) {
            // Remove upgrade check conditionals, since base and upgrade now work the same- just with different ManaGroups.
            List<Card> cards = base.Battle.RollCards(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.AllOnes, CardTypeWeightTable.CanBeLoot, false), base.Value1, (CardConfig config) => config.Cost.Amount == 1 && config.Id != base.Id).ToList<Card>();
            if (cards.Count > 0) {
                foreach (Card card in cards) {
                    card.SetTurnCost(base.Mana);
                    card.IsEthereal = true;
                    card.IsExile = true;
                }
                yield return new AddCardsToHandAction(cards, AddCardsType.Normal);
            }
            yield break;
        }
    }
}
