using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using dragonchain_sdk.Blocks;
using dragonchain_sdk.Contracts;
using dragonchain_sdk.Contracts.SmartContractAtRest;
using dragonchain_sdk.Credentials;
using dragonchain_sdk.DragonNet;
using dragonchain_sdk.Framework.Lucene;
using dragonchain_sdk.Framework.Web;
using dragonchain_sdk.Status;
using dragonchain_sdk.Shared;
using dragonchain_sdk.Transactions;
using dragonchain_sdk.Transactions.L1;
using dragonchain_sdk.Framework.Errors;

namespace dragonchain_sdk
{
    /// <summary>
    /// HTTP Client that interfaces with the dragonchain api, using credentials stored on your machine.
    /// </summary>
    public class DragonchainClient
    {
        private readonly ILogger _logger;
        private static readonly IConfiguration _config;
        private string _endpoint;        
        private ICredentialService _credentialService;
        private IHttpService _httpService;

        /// <summary>
        /// Create an Instance of a DragonchainClient.
        /// </summary>
        /// <param name="dragonchainId">id of a target dragonchain</param>
        /// <param name="verify">verify SSL Certs when talking to local dragonchains</param>
        /// <param name="injected">used only for testing</param>
        public DragonchainClient(string dragonchainId = "", ILogger logger = null, ICredentialService credentialService = null, IConfiguration config = null, IHttpService httpService = null)
        {
            _logger = logger ?? NullLogger.Instance;           
            
            if (string.IsNullOrWhiteSpace(dragonchainId))
            {
                _logger.LogDebug("Dragonchain ID not explicitly provided, will search env/disk");
                dragonchainId = CredentialService.GetDragonchainId();
            }            
            _endpoint = $"https://{dragonchainId}.api.dragonchain.com";
            _credentialService = credentialService ?? new CredentialService(dragonchainId);
            _httpService = httpService ?? new HttpService(_credentialService);
        }

        #region -- IDragonchainClient Members --

        /// <summary>
        /// Create a bulk transaction by string together a bunch of transactions as JSON objects into an array
        /// </summary>
        /// <param name="transactionBulkObject">array of transactions</param>
        /// <returns></returns>
        public async Task<ApiResponse<DragonchainTransactionCreateResponse>> CreateBulkTransaction(DragonchainBulkTransactions transactionBulkObject)
        {
            return await _httpService.PostAsync<DragonchainTransactionCreateResponse>(ConstructUrl("/transaction_bulk"), transactionBulkObject);
        }

        /// <summary>
        /// Create a new Smart Contract on your Dragonchain.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<ApiResponse<DragonchainContractCreateResponse>> CreateContract(ContractCreationSchema body)
        {
            return await _httpService.PostAsync<DragonchainContractCreateResponse>(ConstructUrl("/contract"), body);            
        }
                
        /// <summary>
        /// Create a new Transaction on your Dragonchain. This transaction, if properly structured, will be received by your dragonchain, 
        /// hashed, and put into a queue for processing into a block. The transaction_id returned from this function can be used for checking 
        /// the status of this transaction. Most importantly; the block in which it has been fixated.
        /// </summary>
        /// <param name="transactionObject"></param>
        /// <returns></returns>
        public async Task<ApiResponse<DragonchainTransactionCreateResponse>> CreateTransaction(DragonchainTransactionCreatePayload transactionObject)
        {
            return await _httpService.PostAsync<DragonchainTransactionCreateResponse>(ConstructUrl("/transaction"), transactionObject);            
        }

        /// <summary>
        /// Deletes a smart contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<UpdateResponse>> DeleteSmartContract(string contractId)
        {
            return await _httpService.DeleteAsync<UpdateResponse>(ConstructUrl($"/contract/{contractId}"));
        }

        /// <summary>
        /// Deletes existing registered transaction type        
        /// </summary>
        /// <param name="transactionType"></param>
        /// <returns></returns>
        public async Task<ApiResponse<UpdateResponse>> DeleteTransactionType(string transactionType)
        {
            return await _httpService.DeleteAsync<UpdateResponse>(ConstructUrl($"/transaction-type/{transactionType}"));            
        }

        /// <summary>
        /// Get a single block by ID
        /// </summary>
        /// <param name="blockId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<L1DragonchainTransactionFull>> GetBlock(string blockId)
        {
            return await _httpService.GetAsync<L1DragonchainTransactionFull>(ConstructUrl($"/block/{blockId}"));            
        }

        /// <summary>
        /// Reads secrets given to a smart contract
        /// </summary>
        /// <param name="secretName">the name of the secret to retrieve for smart contract</param>
        /// <returns></returns>
        public string GetSecret(string secretName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a single smart contract by id
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<SmartContractAtRest>> GetSmartContract(string contractId)
        {
            return await _httpService.GetAsync<SmartContractAtRest>(ConstructUrl($"/contract/{contractId}"));            
        }

        /// <summary>
        /// Get from the smart contract heap This function, (unlike other SDK methods) returns raw utf-8 text by design. 
        /// If you expect the result to be parsed json pass true as the jsonParse parameter.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="scName"></param>
        /// <param name="jsonParse"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> GetSmartContractHeap(string key, string scName, bool jsonParse = false)
        {
            return await _httpService.GetAsync<string>(ConstructUrl($"/get/{scName}/{key}"));            
        }

        /// <summary>
        /// Get the status of your dragonchain
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<L1DragonchainStatusResult>> GetStatus()
        {
            return await _httpService.GetAsync<L1DragonchainStatusResult>(ConstructUrl("/status"));
        }

        /// <summary>
        /// Get a transaction by Id.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<L1DragonchainTransactionFull>> GetTransaction(string transactionId)
        {
            return await _httpService.GetAsync<L1DragonchainTransactionFull>(ConstructUrl($"/transaction/{transactionId}"));            
        }

        /// <summary>
        /// Get all the verifications for one block_id.
        /// </summary>
        /// <param name="blockId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public async Task<ApiResponse<IVerifications>> GetVerifications(string blockId, int level = 0)
        {            
            if (level > 0)
            {
                var levelresponse = await _httpService.GetAsync<LevelVerifications>(ConstructUrl($"/verifications/{blockId}?level={level}"));
                var levelVerifications = new ApiResponse<IVerifications> { Ok = levelresponse.Ok, Status = levelresponse.Status, Response = levelresponse.Response };
                return levelVerifications;
            }
            var response = await _httpService.GetAsync<Verifications>(ConstructUrl($"/verifications/{blockId}"));
            var verifications = new ApiResponse<IVerifications> { Ok = response.Ok, Status = response.Status, Response = response.Response };
            return verifications;            
        }

        /// <summary>
        /// List objects from a smart contract heap
        /// </summary>
        /// <param name="scName">the name of smart contract</param>
        /// <param name="key">the sub-key ('folder') to list in the SC heap (optional. Defaults to root of SC heap)</param>
        /// <returns></returns>
        public async Task<ApiResponse<IEnumerable<string>>> ListSmartcontractHeap(string scName, string key = "")
        {
            var path = $"/list/{scName}/";
            if (!string.IsNullOrWhiteSpace(key))
            {
                path += key;
            }            
            return await _httpService.GetAsync<IEnumerable<string>>(path);            
        }

        /// <summary>
        /// Lists current accepted transaction types for a chain
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<IEnumerable<TransactionTypeResponse>>> ListTransactionTypes()
        {
            return await _httpService.GetAsync<IEnumerable<TransactionTypeResponse>>(ConstructUrl("/transaction-types"));            
        }

        /// <summary>
        /// This method is used to override this SDK's attempt to automatically fetch credentials automatically with manually specified creds
        /// </summary>
        /// <param name="authKeyId">Auth Key ID used in HMAC</param>
        /// <param name="authKey">Auth Key used in HMAC</param>
        public void OverrideCredentials(string authKeyId, string authKey)
        {
            _credentialService.OverrideCredentials(authKeyId, authKey);            
        }

        /// <summary>
        /// Query blocks using ElasticSearch query-string syntax For more information on how to use the ElasticSearch query-string syntax 
        /// checkout the Elastic Search documentation: https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-query-string-query.html#query-string-syntax
        /// </summary>
        /// <param name="luceneQuery"></param>
        /// <param name="sort"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<ApiResponse<DragonchainBlockQueryResult>> QueryBlocks(string luceneQuery = "", string sort = "", int offset = 0, int limit = 10)
        {
            var queryParams = LuceneHelper.GetLuceneParams(luceneQuery, sort, offset, limit);
            return await _httpService.GetAsync<DragonchainBlockQueryResult>(ConstructUrl($"/block{queryParams}"));            
        }

        /// <summary>
        /// Query smart contracts using ElasticSearch query-string syntax For more information on how to use the ElasticSearch query-string syntax 
        /// checkout the Elastic Search documentation: https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-query-string-query.html#query-string-syntax
        /// </summary>
        /// <param name="luceneQuery"></param>
        /// <param name="sort"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<ApiResponse<SmartContractAtRest>> QuerySmartContracts(string luceneQuery = "", string sort = "", int offset = 0, int limit = 10)
        {
            var queryParams = LuceneHelper.GetLuceneParams(luceneQuery, sort, offset, limit);
            return await _httpService.GetAsync<SmartContractAtRest>(ConstructUrl($"/contract{queryParams}"));
        }

        /// <summary>
        /// Query transactions using ElasticSearch query-string syntax For more information on how to use the ElasticSearch query-string syntax 
        /// checkout the Elastic Search documentation: https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-query-string-query.html#query-string-syntax
        /// </summary>
        /// <param name="luceneQuery"></param>
        /// <param name="sort"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<ApiResponse<L1DragonchainTransactionQueryResult>> QueryTransactions(string luceneQuery = "", string sort = "", int offset = 0, int limit = 10)
        {
            var queryParams = LuceneHelper.GetLuceneParams(luceneQuery, sort, offset, limit);
            return await _httpService.GetAsync<L1DragonchainTransactionQueryResult>(ConstructUrl($"/transaction{queryParams}"));
        }

        /// <summary>
        /// Registers a new transaction type
        /// </summary>
        /// <param name="txnTypeStructure"></param>
        /// <returns></returns>
        public async Task<ApiResponse<UpdateResponse>> RegisterTransactionType(TransactionTypeStructure txnTypeStructure)
        {
            return await _httpService.PostAsync<UpdateResponse>(ConstructUrl("/transaction-type"), txnTypeStructure);            
        }

        /// <summary>
        /// Change the dragonchainId for this DragonchainClient instance.
        /// After using this command, subsequent requests to your dragonchain will attempt to re-locate credentials for the new dragonchain
        /// </summary>
        /// <param name="dragonchainId">The id of the dragonchain you want to set</param>
        /// <param name="setEndpoint">Whether or not to set a new endpoint automatically (for managed chains at .api.dragonchain.com)</param>
        public void SetDragonchainId(string dragonchainId, bool setEndpoint = true)
        {
            _credentialService = new CredentialService(dragonchainId);
            if (setEndpoint) { SetEndpoint($"https://{dragonchainId}.api.dragonchain.com"); }            
        }

        /// <summary>
        /// Change the endpoint for this DragonchainClient instance.
        /// </summary>
        /// <param name="endpoint">The endpoint of the dragonchain you want to set</param>
        public void SetEndpoint(string endpoint) {
            _endpoint = endpoint;
        }

        /// <summary>
        /// Update your maximum price for each level of verification. 
        /// This method is only relevant for L1 nodes.
        /// </summary>
        /// <param name="maximumPrices">maximum prices (0-1000) to set for each level (in DRGNs) If this number is too low, other nodes will not 
        /// verify your blocks. Changing this number will affect older unverified blocks first.</param>
        /// <returns></returns>
        public async Task<ApiResponse<UpdateResponse>> UpdateDragonnetConfig(DragonnetConfigSchema maximumPrices)
        {
            var dragonnet = maximumPrices.ToDragonnet();
            return await _httpService.PutAsync<UpdateResponse>("/update-matchmaking-data", dragonnet);
        }

        /// <summary>
        /// Update your maximum price for each level of verification.
        /// This method is only relevant for L1 nodes.
        /// </summary>
        /// <param name="askingPrice">(0.0001-1000.0000) the price in DRGN to charge L1 nodes for your verification of their data. Setting this number too high will cause L1's to ignore you more often.</param>
        /// <param name="broadcastInterval">Broadcast Interval is only for level 5 chains</param>
        /// <returns></returns>
        public async Task<ApiResponse<UpdateResponse>> UpdateMatchmakingConfig(decimal? askingPrice = null, int? broadcastInterval = null)
        {
            if (askingPrice != null)
            {                
                if (askingPrice < 0.0001m || askingPrice > 1000) { throw new FailureByDesignException("BAD_REQUEST", "askingPrice must be between 0.0001 and 1000"); }
            }
            var matchmakingUpdate = new MatchMakingUpdate { MatchMaking = new MatchMaking { AskingPrice = askingPrice, BroadcastInterval = broadcastInterval } };
            return await _httpService.PutAsync<UpdateResponse>("/update-matchmaking-data", matchmakingUpdate);
        }

        /// <summary>
        /// Updates existing contract fields
        /// </summary>
        /// <param name="contractId">The id of the existing contract you want to update</param>
        /// <param name="image">The docker image containing the smart contract logic</param>
        /// <param name="cmd">Entrypoint command to run in the docker container</param>
        /// <param name="executionOrder">Order of execution. Valid values 'parallel' or 'serial'</param>
        /// <param name="desiredState">Change the state of a contract. Valid values are "active" and "inactive". You may only change the state of an active or inactive contract.</param>
        /// <param name="args"></param>
        /// <param name="env">mapping of environment variables for your contract</param>
        /// <param name="secrets">mapping of secrets for your contract</param>
        /// <param name="seconds">The seconds of scheduled execution</param>
        /// <param name="cron">The rate of scheduled execution specified as a cron</param>
        /// <param name="auth">basic-auth for pulling docker images, base64 encoded (e.g. username:password)</param>
        /// <returns></returns>
        public async Task<ApiResponse<UpdateResponse>> UpdateSmartContract(string contractId, 
            string image = null, 
            string cmd = null, 
            SmartContractExecutionOrder? executionOrder = null, 
            SmartContractDesiredState? desiredState = null,
            string[] args = null, 
            object env = null, 
            object secrets = null, 
            int? seconds = null, 
            string cron = null, 
            string auth = null)
        {
            var body = new
            {
                Version = "3",
                Dcrn = "SmartContract::L1::Update",
                Image = !string.IsNullOrWhiteSpace(image) ? image : null,

                Cmd = !string.IsNullOrWhiteSpace(cmd) ? cmd : null,
                ExecutionOrder = executionOrder != null ? executionOrder : null,
                DesiredState = desiredState != null ? desiredState : null,
                Args = args != null && args.Length > 0 ? args : null,
                Env = env != null ? env : null,
                Secrets = secrets != null ? secrets : null,
                Seconds = seconds != null ? seconds : null,
                Cron = !string.IsNullOrWhiteSpace(cron) ? cron : null,
                Auth = !string.IsNullOrWhiteSpace(auth) ? auth : null
            };           

            return await _httpService.PutAsync<UpdateResponse>(ConstructUrl($"/contract/{contractId}"), body);            
        }
                
        /// <summary>
        /// Updates a given transaction type structure
        /// </summary>
        /// <param name="transactionType"></param>
        /// <param name="customIndexes"></param>
        /// <returns></returns>
        public async Task<ApiResponse<UpdateResponse>> UpdateTransactionType(string transactionType, IEnumerable<CustomIndexStructure> customIndexes)
        {
            var @params = new { Version = "1", Custom_indexes = customIndexes };
            return await _httpService.PutAsync<UpdateResponse>(ConstructUrl($"/transaction-type/{transactionType}"), @params);            
        }

        #endregion  
        
        private string ConstructUrl(string path)
        {
            return $"{_endpoint}{path}";
        }
    }
}