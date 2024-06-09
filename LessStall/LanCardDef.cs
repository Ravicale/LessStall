using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;

/**
 * Shikigami Protection
 * - Add Exile
 * + Add Echo
 * + Upgrade reduces cost to 1(W/U) from WU.
 * 
 * Add exile to prevent easy loops.
 * Add echo to soften the blow.
 * Slightly improved upgraded mana efficiency to further soften blow.
 */
namespace LessStall {
    [OverwriteVanilla]
    public sealed class LanCardDef : CardTemplate {
        public override IdContainer GetId() {
            return nameof(LBoL.EntityLib.Cards.Neutral.TwoColor.LanCard);
        }

        [DontOverwrite]
        public override CardImages LoadCardImages() {
            return null;
        }

        [DontOverwrite]
        public override LocalizationOption LoadLocalization() {
            return null;
        }

        public override CardConfig MakeConfig() {
            var config = CardConfig.FromId(GetId());
            config.Keywords = Keyword.Exile | Keyword.Echo;
            config.UpgradedKeywords = Keyword.Exile | Keyword.Echo;
            config.UpgradedCost = new ManaGroup() {Any = 1, Hybrid = 1, HybridColor = 0};
            return config;
        }
    }
}
