using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;

/**
 * Remote Support
 * - Add Exile
 * + Starts with 5 choices un-upgraded (from 3).
 * + Reduce cost to W (from 1W).
 * + Upgrade adds Echo.
 * 
 * Add exile to prevent loops.
 * Reduced cost and increased choices to make the cast more consistently useful.
 * Upgrade now gives echo to give it back a little of the lost staying power.
 */
namespace LessStall {
    [OverwriteVanilla]
    public sealed class FineCollectionDef : CardTemplate {
        public override IdContainer GetId() {
            return nameof(LBoL.EntityLib.Cards.Character.Marisa.FindCollection);
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
            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile | Keyword.Echo;
            config.Value1 = 5;
            config.Cost = new ManaGroup() {White = 1};
            return config;
        }
    }
}
