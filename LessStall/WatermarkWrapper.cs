using System.Runtime.CompilerServices;

namespace LessStall {
    internal class WatermarkWrapper {
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ActivateWatermark() => AddWatermark.API.ActivateWatermark();
    }
}
