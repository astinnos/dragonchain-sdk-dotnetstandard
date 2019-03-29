namespace dragonchain_sdk.Blocks.Common
{
    public interface IHeader
    {
        string BlockId { get; set; }        
        string DcId { get; set; }
        string Level { get; }        
        string PrevProof { get; set; }
        string Timestamp { get; set; }
    }
}