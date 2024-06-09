using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;

/**
 * Lily White, Herald of Spring
 * +/- Ultimate now deals 5+unspent mana damage 3 times.
 * 
 * Lily was an infinite life stall-target.
 * Now she acts as a payoff+enabler for green big mana synergies.
 * New ult has symmetry with on-cast effect.
 */
namespace LessStall {
    [OverwriteVanilla]
    public sealed class LilyFriendDef : CardTemplate {
        public override IdContainer GetId() {
            return nameof(LBoL.EntityLib.Cards.Character.Cirno.Friend.LilyFriend);
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
            config.Value2 = 1; //Damage Increase per Unspent mana.
            return config;
        }
    }

    [EntityLogic(typeof(LilyFriendDef))]
    public sealed class LilyFriend : Card {
        public int UltDamage {
            get {
                if (base.Battle == null) {
                    return 0;
                } else {
                    return base.Value2 * Math.Max(base.Battle.BattleMana.Amount, 0);
                }
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition) {
            if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active) {
                base.Loyalty += base.ActiveCost;
                yield return base.SkillAnime;
                yield return new GainManaAction(this.ActiveMana);
            } else {
                // Replace heal with another multi-attack- this time scaling with unspent mana.
                base.Loyalty += base.UltimateCost;
                base.UltimateUsed = true;
                yield return base.SkillAnime;
                base.DeltaDamage += UltDamage;
                base.CardGuns = new Guns(base.GunName, base.Value1, true);
                foreach (GunPair gunPair in base.CardGuns.GunPairs) {
                    yield return base.AttackRandomAliveEnemyAction(gunPair);
                }
            }
            yield break;
        }

        // Rest is vanilla copy/pasta.
        public ManaGroup TurnMana {
            get {
                return base.Mana;
            }
        }

        public ManaGroup ActiveMana {
            get {
                if (!this.IsUpgraded) {
                    return ManaGroup.Blues(1) + ManaGroup.Greens(2);
                }
                return ManaGroup.Philosophies(1) + ManaGroup.Greens(2);
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
                yield return PerformAction.Effect(base.Battle.Player, "LilyFairy", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                yield return new GainManaAction(base.Mana);
                num = i;
            }
            yield break;
        }

        public override IEnumerable<BattleAction> SummonActions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition) {
            base.CardGuns = new Guns(base.GunName, base.Value1, true);
            foreach (GunPair gunPair in base.CardGuns.GunPairs) {
                yield return base.AttackRandomAliveEnemyAction(gunPair);
            }
            List<GunPair>.Enumerator enumerator = default(List<GunPair>.Enumerator);
            foreach (BattleAction battleAction in base.SummonActions(selector, consumingMana, precondition)) {
                yield return battleAction;
            }
            yield break;
        }
    }
}
