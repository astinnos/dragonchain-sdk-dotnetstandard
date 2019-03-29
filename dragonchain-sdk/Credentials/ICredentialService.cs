namespace dragonchain_sdk.Credentials
{
    public interface ICredentialService
    {
        string DragonchainId { get; }
        string GetAuthorizationHeader(string method, string path, string timestamp, string contentType, string body);
        void OverrideCredentials(string authKeyId, string authKey);
    }
}