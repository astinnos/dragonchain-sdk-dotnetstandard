namespace dragonchain_sdk.DragonNet
{
    public class MatchMakingUpdate
    {
        public MatchMaking MatchMaking { get; set; }
    }

    public class MatchMaking
    {
        public decimal? AskingPrice { get; set; }
        public int? BroadcastInterval { get; set; }
    }
}