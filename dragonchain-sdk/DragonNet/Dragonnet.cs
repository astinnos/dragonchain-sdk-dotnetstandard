namespace dragonchain_sdk.DragonNet
{
    public class Dragonnet
    {
        public DragonnetPrice L2 { get; set; }
        public DragonnetPrice L3 { get; set; }
        public DragonnetPrice L4 { get; set; }
        public DragonnetPrice L5 { get; set; }
    }

    public class DragonnetPrice
    {
        public decimal MaximumPrice { get; set; }
    }
}