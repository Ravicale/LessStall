using HarmonyLib;

namespace LessStall {
    public static class PInfo {
        // each loaded plugin needs to have a unique GUID. usually author+generalCategory+Name is good enough
        public const string GUID = "ravi.lbol.lessStall";
        public const string Name = "Less Stall";
        public const string version = "1.0.1";
        public static readonly Harmony harmony = new Harmony(GUID);

    }
}
