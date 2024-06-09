using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using System.Linq;
using LBoL.Base.Extensions;
using System;
using LBoL.Core.StatusEffects;

/**
 * Yukari, Boundary of Fantasy
 * +/- Activated ability now gives 25 barrier.
 * +/- Ultimate now lets you exile any number of cards from your hand, deck, or discard. Then return a card from exile to your hand.
 * 
 * Previously Yukari could be used as an infinite get-stuff-out-of-exile outlet in addition having an ult that acts as a stall outlet itself.
 * To prevent easy reuse, exile recursion has been moved from her active ability to her ult.
 * This still allows for stall strats- but now you need to pick up a number of other pieces to generate a renewable number of Yukaris, and have
 * a payoff card in deck to loop in and out of exile. This requires extreme-going-out-of-the-way to do (Cirno + 2 off colors is the 'easiest'),
 * and is unlikely to manifest accidentally in a playthrough.
 * 
 * Her activated ability now creates barrier- very lore accurate.
 * 
 * To keep her ultimate exciting, it now lets you exile literally anything and everything you want *before* recurring a card from exile.
 * This has many uses- it can cleanse the deck of statuses/misfortunes, act as a rota/demonic tutor, recur a single power card, or even provide 
 * deck thinning setup for infinite combos. The cooking potential is very high here.
 */
namespace LessStall {
    [OverwriteVanilla]    
    public sealed class YukariFriendDef : CardTemplate {
        public override IdContainer GetId() {
            return nameof(LBoL.EntityLib.Cards.Neutral.MultiColor.YukariFriend);
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
            config.Value1 = 25; //Barrier on activated ability.
            return config;
        }
    }

    [EntityLogic(typeof(YukariFriendDef))]
    public sealed class YukariFriend : Card {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition) {
            if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active) {
                // Now just adds barrier.
                base.Loyalty += base.ActiveCost;
                yield return base.SkillAnime;
                yield return base.DefenseAction(0, base.Value1, BlockShieldType.Direct);
            } else {
                // Now does exile.stuff.
                base.Loyalty += base.UltimateCost;
                base.UltimateUsed = true;
                yield return base.SkillAnime;

                // Now prompts the user to exile any number of cards, then lets them recur an exiled card.

                // Exile cards from hand, deck, or discard.
                List<Card> cardsCanExile = (from card in base.Battle.HandZone.Concat(base.Battle.DrawZoneToShow).Concat(base.Battle.DiscardZone)
                        where card != this select card).ToList<Card>();
                if (!cardsCanExile.Empty()) {
                    SelectCardInteraction exileInteraction = new SelectCardInteraction(0, Int32.MaxValue, cardsCanExile, SelectedCardHandling.DoNothing) {
                        Source = this
                    };
                    yield return new InteractionAction(exileInteraction, false);
                    Card[] exiledCards = exileInteraction.SelectedCards.ToArray();
                    if (exiledCards.Length > 0) {
                        yield return new ExileManyCardAction(exiledCards);
                    }
                }

                // Return card from exile.
                if (base.Battle.ExileZone.Count > 0) {
                    SelectCardInteraction interaction = new SelectCardInteraction(1, 1, base.Battle.ExileZone, SelectedCardHandling.DoNothing) {
                        Source = this
                    };
                    yield return new InteractionAction(interaction, false);
                    Card returnedCard = interaction.SelectedCards.FirstOrDefault();
                    if (returnedCard != null) {
                        yield return new MoveCardAction(returnedCard, CardZone.Hand);
                    }
                }
            }
            yield break;
        }


        // The rest is blind vanilla copy/pasta.
        public int Fire {
            get {
                if (base.Battle == null) {
                    return 0;
                }
                return base.Battle.TurnCardUsageHistory.Count((Card c) => c.CardType == CardType.Defense);
            }
        }
        // Token: 0x06000A62 RID: 2658 RVA: 0x000150C5 File Offset: 0x000132C5
        public override IEnumerable<BattleAction> OnTurnEndingInHand() {
            return this.GetPassiveActions();
        }

        // Token: 0x06000A63 RID: 2659 RVA: 0x000150CD File Offset: 0x000132CD
        public override IEnumerable<BattleAction> GetPassiveActions() {
            if (!base.Summoned || base.Battle.BattleShouldEnd) {
                yield break;
            }
            base.NotifyActivating();
            base.Loyalty += base.PassiveCost;
            if (this.Fire > 0) {
                int num;
                for (int i = 0; i < base.Battle.FriendPassiveTimes; i = num + 1) {
                    if (base.Battle.BattleShouldEnd) {
                        yield break;
                    }
                    yield return base.BuffAction<Firepower>(this.Fire, 0, 0, 0, 0.2f);
                    num = i;
                }
            }
            yield break;
        }

        protected override void OnEnterBattle(BattleController battle) {
            base.HandleBattleEvent<CardUsingEventArgs>(base.Battle.CardUsed, delegate (CardUsingEventArgs _) {
                if (base.Zone == CardZone.Hand) {
                    this.NotifyChanged();
                }
            }, (GameEventPriority)0);
        }
    }
}
