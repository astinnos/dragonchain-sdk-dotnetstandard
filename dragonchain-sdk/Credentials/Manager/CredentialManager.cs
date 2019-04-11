using Microsoft.Extensions.Configuration;
using dragonchain_sdk.Framework.Errors;

namespace dragonchain_sdk.Credentials.Manager
{
    public class CredentialManager : ICredentialManager
    {
        private readonly IConfiguration _config;        

        public CredentialManager(IConfiguration config)
        {
            _config = config;            
        }
        
        /// <summary>
        /// Get a dragonchainId from environment/config file
        /// </summary>  
        public string GetDragonchainId()
        {
            if (_config == null) { throw new FailureByDesignException("NOT_FOUND", "No configuration provider set"); }
            var id = _config["dragonchainId"];
            if (!string.IsNullOrWhiteSpace(id)) { return id; }
            throw new FailureByDesignException("NOT_FOUND", "Config does not contain key 'dragonchainId'");
        }

        /// <summary>
        /// Get an authKey/authKeyId pair
        /// </summary>   
        /// <param name="dragonchainId">(optional) dragonchainId to get keys for</param>
        public DragonchainCredentials GetDragonchainCredentials(string dragonchainId = "")
        {
            var authKeyIdentifier = string.IsNullOrWhiteSpace(dragonchainId) ? "AUTH_KEY" : $"{dragonchainId}:AUTH_KEY";
            var authKeyIdIdentifier = string.IsNullOrWhiteSpace(dragonchainId) ? "AUTH_KEY_ID" : $"{dragonchainId}:AUTH_KEY_ID";
            if (_config == null) { throw new FailureByDesignException("NOT_FOUND", "No configuration provider set"); }
            var authKey = _config[authKeyIdentifier];
            var authKeyId = _config[authKeyIdIdentifier];
            if (!string.IsNullOrWhiteSpace(authKey) && !string.IsNullOrWhiteSpace(authKeyId))
            {
                return new DragonchainCredentials { AuthKey = authKey, AuthKeyId = authKeyId };
            }
            throw new FailureByDesignException("NOT_FOUND", $"Config does not contain both keys '{authKeyIdentifier}' and '{authKeyIdIdentifier}'");
        }
    }
}