using JetBrains.Annotations;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core;
using LBoL.EntityLib.Cards.Character.Marisa;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using LBoL.EntityLib.Cards.Neutral.NoColor;

/**
 * Patchouli, Elemental Alchemist
 * +/- -4 Active no longer adds power, instead it adds an astrology to hand.
 * 
 * Power can be accumulated via stalling.
 * Astrologies more closely match the 'potion' archetypal theme.
 */
namespace LessStall {
    [OverwriteVanilla]
    public sealed class PatchouliFriendDef : CardTemplate {
        public override IdContainer GetId() {
            return nameof(LBoL.EntityLib.Cards.Character.Marisa.PatchouliFriend);
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
            config.Value1 = 1; //Number of Astrologies to add.
            config.UpgradedValue1 = 1;
            config.RelativeCards = new List<string>(){"Astrology", "Potion"};
            config.UpgradedRelativeCards = new List<string>() {"Astrology", "Potion"};
            return config;
        }
    }

    [EntityLogic(typeof(PatchouliFriendDef))]
    public sealed class PatchouliFriend : Card {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition) {
            if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active) {
                base.Loyalty += base.ActiveCost;
                yield return base.SkillAnime;
                yield return base.BuffAction<Firepower>(base.Value1, 0, 0, 0, 0.2f);
                yield return base.BuffAction<Spirit>(base.Value1, 0, 0, 0, 0.2f);
                // Add Astrology to hand instead of generating power.
                yield return new AddCardsToHandAction(Library.CreateCards<Astrology>(base.Value1, false), AddCardsType.Normal);
            } else {
                base.Loyalty += base.UltimateCost;
                yield return base.SkillAnime;
                yield return new AddCardsToHandAction(Library.CreateCards<Potion>(base.Value2, false), AddCardsType.Normal);
            }
            yield break;
        }

        // Rest is vanilla copy/pasta.
        public override FriendCostInfo FriendU {
            get {
                return new FriendCostInfo(base.UltimateCost, FriendCostType.Active);
            }
        }

        public int PassiveColor {
            get {
                return base.PassiveCost;
            }
        }

        public override IEnumerable<BattleAction> OnTurnStartedInHand() {
            return this.GetPassiveActions();
        }

        public override IEnumerable<BattleAction> GetPassiveActions() {
            if (!base.Summoned || base.Battle.BattleShouldEnd) {
                yield break;
            }
            base.NotifyActivating();
            base.Loyalty += base.PassiveCost;
            int num;
            for (int i = 0; i < base.Battle.FriendPassiveTimes; i = num + 1) {
                if (base.Battle.BattleShouldEnd) {
                    yield break;
                }
                yield return PerformAction.Sfx("FairySupport", 0f);
                yield return ConvertManaAction.PhilosophyRandomMana(base.Battle.BattleMana, this.PassiveColor, base.GameRun.BattleRng);
                num = i;
            }
            yield break;
        }
    }
}
