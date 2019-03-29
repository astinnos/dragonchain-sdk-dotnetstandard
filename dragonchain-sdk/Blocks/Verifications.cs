using System.Collections.Generic;

namespace dragonchain_sdk.Blocks
{
    public interface IVerifications { }

    public class Verifications : IVerifications
    {
        public IEnumerable<L2.L2BlockAtRest> L2 { get; set; }
        public IEnumerable<L3.L3BlockAtRest> L3 { get; set; }
        public IEnumerable<L4.L4BlockAtRest> L4 { get; set; }
        public IEnumerable<L5.L5BlockAtRest> L5 { get; set; }
    }

    public class LevelVerifications : IVerifications
    {
        public IEnumerable<IBlockAtRest> Verifications { get; set; }
    }
}
