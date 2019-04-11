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
        public DragonchainCredentials GetDragonchainCredentials()
        {
            if (_config == null) { throw new FailureByDesignException("NOT_FOUND", "No configuration provider set"); }
            var authKey = _config["AUTH_KEY"];
            var authKeyId = _config["AUTH_KEY_ID"];
            if (!string.IsNullOrWhiteSpace(authKey) && !string.IsNullOrWhiteSpace(authKeyId))
            {
                return new DragonchainCredentials { AuthKey = authKey, AuthKeyId = authKeyId };
            }
            throw new FailureByDesignException("NOT_FOUND", "Config does not contain both keys 'AUTH_KEY' and 'AUTH_KEY_ID'");
        }
    }
}