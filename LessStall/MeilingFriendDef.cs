using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Character.Sakuya;
using System.Collections.Generic;

/**
 * Meiling, Neglectful Gatekeeper
 * +/- Ult now grants 3(4) Spirit instead of 12(16) health. (Firepower is untouched)
 * 
 * Spirit is a reasonably strong defensive boost that has nice symmetry with firepower.
 * The loss of healing seems anti-synergistic with the bonus trigger requirements, but you can still get those with blocked attacks so the loss isn't
 * that huge.
 */
namespace LessStall {
    [OverwriteVanilla]
    public sealed class MeilingFriendDef : CardTemplate {
        public override IdContainer GetId() {
            return nameof(LBoL.EntityLib.Cards.Character.Sakuya.MeilingFriend);
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
            config.Value1 = 3; //Spirit increase on ult.
            config.UpgradedValue1 = 4;
            return config;
        }
    }

    [EntityLogic(typeof(MeilingFriendDef))]
    public sealed class MeilingFriend : Card {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition) {
            base.Loyalty += base.UltimateCost;
            base.UltimateUsed = true;
            yield return base.SpellAnime;
            yield return base.BuffAction<Spirit>(base.Value1, 0, 0, 0, 0.2f); // Replace heal with spirit buff.
            yield return base.BuffAction<Firepower>(base.Value2, 0, 0, 0, 0.2f);
            yield break;
        }

        // Rest is vanilla copy/pasta.
        public int Graze {
            get {
                if (!this.IsUpgraded) {
                    return 1;
                }
                return 2;
            }
        }

        protected override void OnEnterBattle(BattleController battle) {
            base.ReactBattleEvent<StatisticalDamageEventArgs>(base.Battle.Player.StatisticalTotalDamageReceived, new EventSequencedReactor<StatisticalDamageEventArgs>(this.OnStatisticalTotalDamageReceived));
        }

        public override IEnumerable<BattleAction> OnTurnStartedInHand() {
            return this.GetPassiveActions();
        }

        private IEnumerable<BattleAction> OnStatisticalTotalDamageReceived(StatisticalDamageEventArgs args) {
            if (base.Zone != CardZone.Hand) {
                return null;
            }
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
                yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<Knife>() });
                num = i;
            }
            yield break;
        }

        public override IEnumerable<BattleAction> SummonActions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition) {
            yield return base.BuffAction<Graze>(this.Graze, 0, 0, 0, 0.2f);
            foreach (BattleAction battleAction in base.SummonActions(selector, consumingMana, precondition)) {
                yield return battleAction;
            }
            yield break;
        }
    }
}
