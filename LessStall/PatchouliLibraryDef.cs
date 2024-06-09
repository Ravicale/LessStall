using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;

/**
 * Unmoving Great Library
 * - Has Exile
 * + Has Echo
 * + Upgrade makes red/blue mana Hybrid Cost
 * 
 * Exile to prevent infinite reuse, but echo to soften the blow.
 * Make the upgrade a little more tempting to help compensate for the nerf.
 * This card offers an extremely high quality pool, so I don't think further buffs are needed.
 */
namespace LessStall {
    [OverwriteVanilla]
    public sealed class PatchouliLibrary : CardTemplate {
        public override IdContainer GetId() {
            return nameof(LBoL.EntityLib.Cards.Neutral.MultiColor.PatchouliLibrary);
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
            config.UpgradedCost = new ManaGroup() {White = 1, Hybrid = 2, HybridColor = 5};
            return config;
        }
    }
}
