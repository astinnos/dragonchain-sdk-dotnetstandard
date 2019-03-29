using dragonchain_sdk.Blocks.Common;

namespace dragonchain_sdk.Blocks
{
    public interface IBlockAtRest
    {
        string Dcrn { get; }
        IHeader Header { get; set; }
        Proof Proof { get; set; }
        string Version { get; }
    }
}