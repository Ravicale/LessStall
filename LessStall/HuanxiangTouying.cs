using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;

/**
 * Trace On
 * - Add Exile
 * + Gives 3 Phil Mana (from 2).
 * 
 * Added exile to prevent loops.
 * Increased mana gain to make the floor, even on a bad pool, a small amount of mana accelaration to soften the blow.
 * Also makes it feel a bit nicer to use, since the base version always gives enough mana to cast all options.
 * Not giving more mana on upgrade though, then this starts to out-compete dedicated mana accelerants.
 */
namespace LessStall {

    [OverwriteVanilla]
    public sealed class HuanxiangTouying : CardTemplate {
        public override IdContainer GetId() {
            return nameof(LBoL.EntityLib.Cards.Character.Reimu.HuanxiangTouying);
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
            config.UpgradedKeywords = Keyword.Exile;
            config.Mana = new ManaGroup() {Philosophy = 3};
            return config;
        }
    }
}
