using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Randoms;
using LBoL.Core;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using UnityEngine;
using LBoL.Core.Units;
using System.Linq;

/**
 * Fires of Hokkai
 * +/- Status now reduces Skills to costing 1 instead of its previous effect.
 * +/- Upgrade now adds 3 skills with Replenish to the deck.
 * 
 * Status has some overlap with Embrace the Darkness, but the lack of exiling and limitation to skills should keep them both feeling distinct enough.
 * Upgrade shuffles in some skills to the deck to provide payoffs in decks running few skills, and provides them with replenish.
 * The added skills should also help preserve the general feel of the original card.
 */
namespace LessStall {
    [OverwriteVanilla]
    public sealed class BailianFireDef : CardTemplate {
        public static ManaGroup reducedMana = ManaGroup.Anys(1);

        public override IdContainer GetId() {
            return nameof(LBoL.EntityLib.Cards.Neutral.MultiColor.BailianFire);
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
            config.Mana = reducedMana;
            config.UpgradedValue1 = 3; // Number of skill cards to add.
            return config;
        }
    }

    [EntityLogic(typeof(BailianFireDef))]
    public sealed class BailianFire : Card {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition) {
            yield return base.BuffAction<BailianFireSe>(1, 0, 0, 0, 0.2f);
            if (this.IsUpgraded) {
                // Change upgrade effect.
                if (this.Value1 > 0) {
                    Card[] cards = base.Battle.RollCards(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.AllOnes, CardTypeWeightTable.OnlySkill, false), base.Value1, null);
                    if (cards.Length > 0) {
                        foreach (var card in cards) {
                            card.IsReplenish = true;
                        }
                        yield return new AddCardsToDrawZoneAction(cards, DrawZoneTarget.Random, AddCardsType.Normal);
                    }
                }
            }
            yield break;
        }
    }

    [OverwriteVanilla]
    public sealed class BailianFireSeDef : StatusEffectTemplate {
        public override IdContainer GetId() {
            return nameof(LBoL.EntityLib.StatusEffects.Neutral.MultiColor.BailianFireSe);
        }

        public override LocalizationOption LoadLocalization() {
            var loc = new GlobalLocalization(BepinexPlugin.embeddedSource);
            loc.LocalizationFiles.AddLocaleFile(LBoL.Core.Locale.En, "LocEn.yaml");
            return loc;
        }

        [DontOverwrite]
        public override Sprite LoadSprite() {
            return null;
        }

        public override StatusEffectConfig MakeConfig() {
            var config = StatusEffectConfig.FromId(GetId());
            config.IsStackable = false;
            config.HasLevel = false;
            config.Keywords = Keyword.None;
            return config;
        }
    }

    [EntityLogic(typeof(BailianFireSeDef))]
    public sealed class BailianFireSe : StatusEffect {
        public ManaGroup Mana = BailianFireDef.reducedMana;
        protected override void OnAdded(Unit unit) {
            ReduceCosts(base.Battle.EnumerateAllCards());

            // Attach listeners to catch any new skills added to the play field.
            base.HandleOwnerEvent<CardsEventArgs>(base.Battle.CardsAddedToDiscard,
                new GameEventHandler<CardsEventArgs>(e => ReduceCosts(e.Cards)));
            base.HandleOwnerEvent<CardsEventArgs>(base.Battle.CardsAddedToHand,
                new GameEventHandler<CardsEventArgs>(e => ReduceCosts(e.Cards)));
            base.HandleOwnerEvent<CardsEventArgs>(base.Battle.CardsAddedToExile,
                new GameEventHandler<CardsEventArgs>(e => ReduceCosts(e.Cards)));
            base.HandleOwnerEvent<CardsAddingToDrawZoneEventArgs>(base.Battle.CardsAddedToDrawZone,
                new GameEventHandler<CardsAddingToDrawZoneEventArgs>(e => ReduceCosts(e.Cards)));
        }

        private void ReduceCosts(IEnumerable<Card> cards) {
            List<Card> validCards = cards.Where(card => card.CardType == CardType.Skill).ToList();
            if (validCards.Count > 0) {
                base.NotifyActivating();
                foreach (Card card in validCards) {
                    if (card.CardType == CardType.Skill && !card.BaseCost.IsSubset(Mana)) {
                        card.SetBaseCost(Mana);
                    }
                }
            }
        }
    }
}
