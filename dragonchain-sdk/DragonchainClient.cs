using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using dragonchain_sdk.Blocks;
using dragonchain_sdk.Contracts;
using dragonchain_sdk.Credentials;
using dragonchain_sdk.Credentials.Keys;
using dragonchain_sdk.Credentials.Manager;
using dragonchain_sdk.Framework.Errors;
using dragonchain_sdk.Framework.Lucene;
using dragonchain_sdk.Framework.Web;
using dragonchain_sdk.Interchain;
using dragonchain_sdk.Interchain.Bitcoin;
using dragonchain_sdk.Interchain.Ethereum;
using dragonchain_sdk.Interchain.Networks;
using dragonchain_sdk.Status;
using dragonchain_sdk.Transactions;
using dragonchain_sdk.Transactions.Bulk;
using dragonchain_sdk.Transactions.L1;
using dragonchain_sdk.Transactions.Types;
using System.IO;

namespace dragonchain_sdk
{
    /// <summary>
    /// HTTP Client that interfaces with the dragonchain api, using credentials stored on your machine.
    /// </summary>
    public class DragonchainClient : IDragonchainClient
    {                    
        private ICredentialService _credentialService;
        private IHttpService _httpService;

        /// <summary>
        /// Create an Instance of a DragonchainClient.
        /// </summary>
        /// <param name="dragonchainId">dragonchainId associated with these credentials</param>
        /// <param name="config">Microsoft.Extensions.Configuration implementation</param>        
        /// <param name="logger">Microsoft.Extensions.Logging implementation</param>
        public DragonchainClient(string dragonchainId = "", IConfiguration config = null, ILogger<DragonchainClient> logger = null)            
        {
            logger = logger ?? new NullLogger<DragonchainClient>();
            var credentialManager = new CredentialManager(config);            
            if (string.IsNullOrWhiteSpace(dragonchainId))
            {                
                logger.LogDebug("Dragonchain ID not explicitly provided, will search env/disk");
                dragonchainId = credentialManager.GetDragonchainId();
            }
            _credentialService = new CredentialService(dragonchainId, credentialManager: credentialManager);
            var endpoint = $"https://{dragonchainId}.api.dragonchain.com";
            _httpService = new HttpService(_credentialService, endpoint, logger);
        }

        /// <summary>
        /// Create an Instance of a DragonchainClient.
        /// </summary>
        /// <param name="dragonchainId">dragonchainId associated with these credentials</param>
        /// /// <param name="credentialManager">manager to retrieve Dragonchain credentials from config provider</param>
        /// <param name="credentialService">service to retrieve Dragonchain credentials for use in API requests</param>
        /// <param name="httpService">API request service</param>
        /// <param name="logger">Microsoft.Extensions.Logging implementation</param>
        public DragonchainClient(string dragonchainId, ICredentialManager credentialManager, ICredentialService credentialService = null, IHttpService httpService = null, ILogger logger = null)
        {
            logger = logger ?? NullLogger.Instance;                       
            if (string.IsNullOrWhiteSpace(dragonchainId))
            {
                if(credentialManager == null) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, message: "Credential Manager must be provided if dragonchainid is null or empty"); }
                logger.LogDebug("Dragonchain ID not explicitly provided, will search env/disk");
                dragonchainId = credentialManager.GetDragonchainId();
            }                        
            _credentialService = credentialService ?? new CredentialService(dragonchainId, credentialManager: credentialManager);
            var endpoint = $"https://{dragonchainId}.api.dragonchain.com";
            _httpService = httpService ?? new HttpService(_credentialService, endpoint, logger);
        }
                
        /// <summary>
        /// Get the status of your dragonchain
        /// </summary>        
        public async Task<ApiResponse<L1DragonchainStatusResult>> GetStatus()
        {
            return await _httpService.GetAsync<L1DragonchainStatusResult>("/status");
        }
                
        #region -- Keys --

        /// <summary>
        /// Generate a new HMAC API key
        /// </summary>        
        public async Task<ApiResponse<CreateAPIKeyResponse>> CreateApiKey()
        {
            return await _httpService.PostAsync<CreateAPIKeyResponse>("/api-key", null);
        }

        /// <summary>
        /// List HMAC API key IDs and their associated metadata
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<ListAPIKeyResponse>> ListApiKeys()
        {
            return await _httpService.GetAsync<ListAPIKeyResponse>("/api-key");
        }

        /// <summary>
        /// Get metadata about an existing HMAC API key
        /// </summary>
        /// <param name="keyId">the key id of the key to get</param>
        /// <returns></returns>
        public async Task<ApiResponse<GetAPIKeyResponse>> GetApiKey(string keyId)
        {
            if (string.IsNullOrWhiteSpace(keyId)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `keyId` is required"); }
            return await _httpService.GetAsync<GetAPIKeyResponse>($"/api-key/{keyId}");
        }

        /// <summary>
        /// Delete an existing HMAC API key
        /// </summary>
        /// <param name="keyId">the key id of the key to delete</param>
        /// <returns></returns>
        public async Task<ApiResponse<DeleteAPIKeyResponse>> DeleteApiKey(string keyId)
        {
            if (string.IsNullOrWhiteSpace(keyId)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `keyId` is required"); }
            return await _httpService.DeleteAsync<DeleteAPIKeyResponse>($"/api-key/{keyId}");
        }

        #endregion

        #region -- Transaction Types --
        
        /// <summary>
        /// Gets an existing transaction type from the chain
        /// </summary>
        /// <param name="transactionType">The name of the transaction type to get</param>
        /// <returns></returns>
        public async Task<ApiResponse<TransactionTypeResponse>> GetTransactionType(string transactionType) {
            if (string.IsNullOrWhiteSpace(transactionType)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `transactionType` is required"); }
            return await _httpService.GetAsync<TransactionTypeResponse>($"/transaction-type/{transactionType}");
        }

        /// <summary>
        /// Create a new transaction type for ledgering transactions
        /// </summary>
        /// <param name="transactionType">The string of the transaction type to create</param>
        /// <param name="customIndexes">The custom indexes that should be associated with this transaction type</param>
        /// <returns></returns>
        public async Task<ApiResponse<TransactionTypeSimpleResponse>> CreateTransactionType(string transactionType, IEnumerable<TransactionTypeCustomIndex> customIndexes)
            {
                if (string.IsNullOrWhiteSpace(transactionType)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `transactionType` is required"); }
                var body = new TransactionTypeStructure {
                    Version = "1",
                    TransactionType = transactionType,
                    CustomIndexes = customIndexes
                };            
                return await _httpService.PostAsync<TransactionTypeSimpleResponse>("/transaction-type", body);
            }

        /// <summary>
        /// Deletes existing registered transaction type        
        /// </summary>
        /// <param name="transactionType">The name of the transaction type to delete</param>
        /// <returns></returns>
        public async Task<ApiResponse<TransactionTypeSimpleResponse>> DeleteTransactionType(string transactionType)
        {
            if (string.IsNullOrWhiteSpace(transactionType)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `transactionType` is required"); }
            return await _httpService.DeleteAsync<TransactionTypeSimpleResponse>($"/transaction-type/{transactionType}");
        }

        /// <summary>
        /// Lists currently created transaction types
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<TransactionTypeListResponse>> ListTransactionTypes()
        {
            return await _httpService.GetAsync<TransactionTypeListResponse>("/transaction-types");
        }

        /// <summary>
        /// Updates an existing transaction type with new custom indexes
        /// </summary>
        /// <param name="transactionType">The name of the transaction type to update</param>
        /// <param name="customIndexes">The custom indexes that should be updated onto the transaction type</param>
        /// <returns></returns>
        public async Task<ApiResponse<TransactionTypeSimpleResponse>> UpdateTransactionType(string transactionType, IEnumerable<TransactionTypeCustomIndex> customIndexes)
        {
            if (string.IsNullOrWhiteSpace(transactionType)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `transactionType` is required"); }
            if (!customIndexes.Any()) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `customIndexes` is required"); }
            var body = new { Version = "1", Custom_indexes = customIndexes };
            return await _httpService.PutAsync<TransactionTypeSimpleResponse>($"/transaction-type/{transactionType}", body);
        }

        #endregion

        #region -- Transactions --

        /// <summary>
        /// Create a new Transaction on your Dragonchain. This transaction, if properly structured, will be received by your dragonchain, 
        /// hashed, and put into a queue for processing into a block. The transaction_id returned from this function can be used for checking 
        /// the status of this transaction. Most importantly; the block in which it has been fixated.
        /// </summary>
        /// <param name="transactionType">The transaction type to use for this new transaction. This transaction type must already exist on the chain (via `CreateTransactionType`)</param>
        /// <param name="payload">Payload of the transaction. Must be a utf-8 encodable string or any object</param>
        /// <param name="tag">Tag of the transaction which gets indexed and can be searched on for queries</param>
        /// <param name="callbackURL">URL to callback when this transaction is processed</param>
        /// <returns></returns>
        public async Task<ApiResponse<DragonchainTransactionCreateResponse>> CreateTransaction(string transactionType, object payload = null, string tag = "", string callbackURL = "")
        {
            if (string.IsNullOrWhiteSpace(transactionType)) throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `transactionType` is required");
            if (payload == null) { payload = string.Empty; }
            var transactionBody = new DragonchainTransactionCreatePayload
            {
                Version = "1",
                TransactionType = transactionType,
                Payload = payload
            };
            if (!string.IsNullOrWhiteSpace(tag)) { transactionBody.Tag = tag; }
            return await _httpService.PostAsync<DragonchainTransactionCreateResponse>("/transaction", transactionBody, callbackURL);
        }

        /// <summary>
        /// Create a bulk transaction to send many transactions to a chain with only a single call
        /// </summary>
        /// <param name="transactionBulkObject">array of transactions</param>
        /// <returns></returns>
        public async Task<ApiResponse<DragonchainBulkTransactionCreateResponse>> CreateBulkTransaction(IEnumerable<BulkTransactionPayload> transactionList)
        {
            if (!transactionList.Any()) throw new FailureByDesignException(FailureCode.PARAM_ERROR, "parameter `transactionList` is required");
            var bulkTransactionBody = new List<DragonchainTransactionCreatePayload>();
            foreach(var transaction in transactionList)
            {
                var transactionBody = new DragonchainTransactionCreatePayload
                {
                    Version = "1",
                    TransactionType = transaction.TransactionType,
                    Payload = transaction.Payload != null ? transaction.Payload : ""
                };
                if (!string.IsNullOrWhiteSpace(transaction.Tag)) { transactionBody.Tag = transaction.Tag; }
                bulkTransactionBody.Add(transactionBody);
            }
            return await _httpService.PostAsync<DragonchainBulkTransactionCreateResponse>("/transaction_bulk", bulkTransactionBody);
        }

        /// <summary>
        /// Get a transaction by Id.
        /// </summary>
        /// <param name="transactionId">the transaction id of the transaction to get</param>
        /// <returns></returns>
        public async Task<ApiResponse<L1DragonchainTransactionFull>> GetTransaction(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `transactionId` is required"); }
            return await _httpService.GetAsync<L1DragonchainTransactionFull>($"/transaction/{transactionId}");
        }

        /// <summary>
        /// Query transactions using ElasticSearch query-string syntax For more information on how to use the ElasticSearch query-string syntax 
        /// checkout the Elastic Search documentation: https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-query-string-query.html#query-string-syntax
        /// </summary>
        /// <param name="luceneQuery">lucene query to use for this query request</param>
        /// <param name="sort">Sort syntax of 'field:direction</param>
        /// <param name="offset">Pagination offset integer of query (default 0)</param>
        /// <param name="limit">Pagination limit integer of query (default 10)</param>
        /// <returns></returns>
        public async Task<ApiResponse<QueryResult<L1DragonchainTransactionFull>>> QueryTransactions(string luceneQuery = "", string sort = "", int offset = 0, int limit = 10)
        {
            var queryParams = LuceneHelper.GetLuceneParams(luceneQuery, sort, offset, limit);
            return await _httpService.GetAsync<QueryResult<L1DragonchainTransactionFull>>($"/transaction{queryParams}");
        }

        #endregion

        #region -- Blocks --

        /// <summary>
        /// Get a single block by ID
        /// </summary>
        /// <param name="blockId">ID of the block to fetch</param>
        /// <returns></returns>
        public async Task<ApiResponse<BlockSchemaType>> GetBlock(string blockId)
        {
            if (string.IsNullOrWhiteSpace(blockId)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `blockId` is required"); }
            return await _httpService.GetAsync<BlockSchemaType>($"/block/{blockId}");
        }

        /// <summary>
        /// Query blocks using ElasticSearch query-string syntax For more information on how to use the ElasticSearch query-string syntax 
        /// checkout the Elastic Search documentation: https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-query-string-query.html#query-string-syntax
        /// </summary>
        /// <param name="luceneQuery">lucene query to use for this query request</param>
        /// <param name="sort">Sort syntax of 'field:direction'</param>
        /// <param name="offset">Pagination offset integer of query (default 0)</param>
        /// <param name="limit">Pagination limit integer of query (default 10)</param>
        /// <returns></returns>
        public async Task<ApiResponse<QueryResult<BlockSchemaType>>> QueryBlocks(string luceneQuery = "", string sort = "", int offset = 0, int limit = 10)
        {
            var queryParams = LuceneHelper.GetLuceneParams(luceneQuery, sort, offset, limit);
            return await _httpService.GetAsync<QueryResult<BlockSchemaType>>($"/block{queryParams}");
        }

        #endregion

        #region -- Contracts --

        /// <summary>
        /// Reads secrets provided to a smart contract
        /// Note: This will only work when running within a smart contract, given that the smart contract was created/updated with secrets
        /// </summary>
        /// <param name="secretName">the name of the secret to retrieve for smart contract</param>        
        public async Task<string> GetSmartContractSecret(string secretName)
        {
            if (string.IsNullOrWhiteSpace(secretName)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `secretName` is required"); }
            var path = Path.Combine("/", "var", "openfaas", "secrets", $"sc-{Environment.GetEnvironmentVariable("SMART_CONTRACT_ID")}-{secretName}");
            using (var reader = File.OpenText(path))
            {
                return await reader.ReadToEndAsync();                
            }            
        }

        /// <summary>
        /// Create a new Smart Contract on your Dragonchain.
        /// </summary>
        /// <param name="transactionType">Transaction type to assign to this new smart contract. Must not already exist as a transaction type on the chain</param>
        /// <param name="image">Docker image to use with the smart contract. Should be in the form registry/image:tag (or just image:tag if it's a docker hub image)</param>
        /// <param name="cmd">The command to run in your docker container for your application</param>
        /// <param name="args">The list of arguments to use in conjunction with cmd</param>
        /// <param name="executionOrder">The execution of the smart contract, can be `serial` or `parallel`. Will default to `parallel`
        /// If running in serial, the contract will be queued and executed in order, only one at a time
        /// If running in parallel, the contract will be executed as soon as possible after invocation, potentially out of order, and many at a time
        /// </param>
        /// <param name="environmentVariables">JSON object key-value pairs of strings for environments variables provided to the smart contract on execution</param>
        /// <param name="secrets">JSON object key-value pairs of strings for secrets provided to the smart contract on execution
        /// These are more securely stored than environment variables, and can be accessed during execution the smart contract by using the `getSmartContractSecret` method of the sdk
        /// </param>
        /// <param name="scheduleIntervalInSeconds">Schedule a smart contract to be automatically executed every `x` seconds
        /// For example: if `10` is supplied, then this contract will be automatically invoked and create a transaction once every 10 seconds
        /// Note: This is a mutually exclusive parameter with cronExpression
        /// </param>
        /// <param name="cronExpression">Schedule a smart contract to be automatically executed on a cadence via a cron expression
        /// Note: This is a mutually exclusive parameter with scheduleIntervalInSeconds
        /// </param>
        /// <param name="registryCredentials">The basic-auth credentials necessary to pull the docker container.
        /// This should be a base64-encoded string of `username:password` for the docker registry
        /// </param>
        /// <returns></returns>
        public async Task<ApiResponse<DragonchainContractResponse>> CreateSmartContract(string transactionType, 
            string image, 
            string cmd, 
            IEnumerable<string> args,
            SmartContractExecutionOrder executionOrder = SmartContractExecutionOrder.Parallel, 
            object environmentVariables = null, 
            object secrets = null, 
            int? scheduleIntervalInSeconds = null, 
            string cronExpression = null, 
            string registryCredentials = null)
        {
            if (string.IsNullOrWhiteSpace(transactionType)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `transactionType` is required"); }
            if (string.IsNullOrWhiteSpace(image)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `image` is required"); }
            if (string.IsNullOrWhiteSpace(cmd)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `cmd` is required"); }
            if (scheduleIntervalInSeconds != null && cronExpression != null) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameters `scheduleIntervalInSeconds` and `cronExpression` are mutually exclusive"); }
            var body = new SmartContractSchema {
                Version = "3",
                TransactionType = transactionType,
                Image = image,
                ExecutionOrder = executionOrder, // default execution order
                Cmd = cmd,
                Arguments = args,
                EnvironmentVariables = environmentVariables,
                Secrets = secrets,
                Seconds = scheduleIntervalInSeconds,
                Cron = cronExpression,
                Auth = registryCredentials
            };
            return await _httpService.PostAsync<DragonchainContractResponse>("/contract", body);
        }

        /// <summary>
        /// Update an existing Smart Contract on your Dragonchain
        /// Note that all parameters (aside from contract id) are optional, and only supplied parameters will be updated
        /// </summary>
        /// <param name="smartContractId">Smart contract id of which to update. Should be a guid</param>
        /// <param name="image">Docker image to use with the smart contract. Should be in the form registry/image:tag (or just image:tag if it's a docker hub image)</param>
        /// <param name="cmd">The command to run in your docker container for your application</param>
        /// <param name="args">The list of arguments to use in conjunction with cmd</param>
        /// <param name="executionOrder">The execution of the smart contract, can be `serial` or `parallel`. Will default to `parallel`
        /// If running in serial, the contract will be queued and executed in order, only one at a time     
        /// If running in parallel, the contract will be executed as soon as possible after invocation, potentially out of order, and many at a time
        /// </param>
        /// <param name="enabled">Boolean whether or not the contract should be enabled, and able to be invoked</param>
        /// <param name="environmentVariables">object key-value pairs of strings for environments variables provided to the smart contract on execution</param>
        /// <param name="secrets">object key-value pairs of strings for secrets provided to the smart contract on execution</param>
        /// <param name="scheduleIntervalInSeconds">Schedule a smart contract to be automatically executed every `x` seconds
        /// For example, if `10` is supplied, then this contract will be automatically invoked and create a transaction once every 10 seconds
        /// Note: This is a mutually exclusive parameter with cronExpression
        /// </param>
        /// <param name="cronExpression">Schedule a smart contract to be automatically executed on a cadence via a cron expression
        /// Note: This is a mutually exclusive parameter with scheduleIntervalInSeconds        
        /// </param>
        /// <param name="registryCredentials">The basic-auth credentials necessary to pull the docker container.
        /// This should be a base64-encoded string of `username:password` for the docker registry
        /// </param>
        /// <returns></returns>
        public async Task<ApiResponse<DragonchainContractResponse>> UpdateSmartContract(string smartContractId,
            string image = null,
            string cmd = null,
            string[] args = null,
            SmartContractExecutionOrder? executionOrder = null,
            bool? enabled = null,                      
            object environmentVariables = null,
            object secrets = null,
            int? scheduleIntervalInSeconds = null,
            string cronExpression = null,
            string registryCredentials = null)
        {
            if (string.IsNullOrWhiteSpace(smartContractId)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `smartContractId` is required"); }
            if (scheduleIntervalInSeconds != null && cronExpression != null) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameters `scheduleIntervalInSeconds` and `cronExpression` are mutually exclusive"); }
            var body = new SmartContractSchema
            {
                Version = "3",                
                Image = image,
                Cmd = cmd,
                ExecutionOrder = executionOrder,
                DesiredState = enabled.HasValue ? (enabled == true ? SmartContractDesiredState.Active : SmartContractDesiredState.Inactive) : default(SmartContractDesiredState?),
                Arguments = args != null && args.Length > 0 ? args : null,
                EnvironmentVariables = environmentVariables != null ? environmentVariables : null,
                Secrets = secrets != null ? secrets : null,
                Seconds = scheduleIntervalInSeconds != null ? scheduleIntervalInSeconds : null,
                Cron = !string.IsNullOrWhiteSpace(cronExpression) ? cronExpression : null,
                Auth = !string.IsNullOrWhiteSpace(registryCredentials) ? registryCredentials : null
            };

            return await _httpService.PutAsync<DragonchainContractResponse>($"/contract/{smartContractId}", body);
        }

        /// <summary>
        /// Deletes a deployed smart contract
        /// </summary>
        /// <param name="smartContractId">The id of the smart contract to delete. Should be a guid</param>
        /// <returns></returns>
        public async Task<ApiResponse<SmartContractAtRest>> DeleteSmartContract(string smartContractId)
        {
            if (string.IsNullOrWhiteSpace(smartContractId)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `smartContractId` is required"); }
            return await _httpService.DeleteAsync<SmartContractAtRest>($"/contract/{smartContractId}");
        }

        /// <summary>
        /// Get a single smart contract by one of id or transaction type
        /// </summary>
        /// <param name="smartContractId">Contract id to get, mutually exclusive with transactionType</param>
        /// <param name="transactionType">Transaction id of smart contract to get, mutually exclusive with smartContractId</param>
        /// <returns></returns>
        public async Task<ApiResponse<SmartContractAtRest>> GetSmartContract(string smartContractId, string transactionType)
        {
            if (!string.IsNullOrWhiteSpace(smartContractId) && !string.IsNullOrWhiteSpace(transactionType)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Only one of `smartContractId` or `transactionType` can be specified"); }
            if (!string.IsNullOrWhiteSpace(smartContractId)) return await _httpService.GetAsync<SmartContractAtRest>($"/contract/{smartContractId}");
            if (!string.IsNullOrWhiteSpace(transactionType)) return await _httpService.GetAsync<SmartContractAtRest>($"/contract/txn_type/{transactionType}");
            throw new FailureByDesignException(FailureCode.PARAM_ERROR, "At least one of `smartContractId` or `transactionType` must be supplied");            
        }

        /// <summary>
        /// Query smart contracts using ElasticSearch query-string syntax For more information on how to use the ElasticSearch query-string syntax 
        /// checkout the Elastic Search documentation: https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-query-string-query.html#query-string-syntax
        /// </summary>
        /// <param name="luceneQuery">lucene query to use for this query request. Example: `is_serial:true`</param>
        /// <param name="sort">Sort syntax of 'field:direction'. Example: `txn_type:asc`</param>
        /// <param name="offset">Pagination offset integer of query (default 0)</param>
        /// <param name="limit">Pagination limit integer of query (default 10)</param>
        /// <returns></returns>
        public async Task<ApiResponse<QueryResult<SmartContractAtRest>>> QuerySmartContracts(string luceneQuery = "", string sort = "", int offset = 0, int limit = 10)
        {
            var queryParams = LuceneHelper.GetLuceneParams(luceneQuery, sort, offset, limit);
            return await _httpService.GetAsync<QueryResult<SmartContractAtRest>>($"/contract{queryParams}");
        }

        /// <summary>
        /// Get an object from the smart contract heap. This is used for getting stateful data set by the outputs of smart contracts        
        /// </summary>
        /// <param name="key">Key of the object to retrieve</param>
        /// <param name="smartContractId">Smart contract to get the object from
        /// When running from within a smart contract, this is provided via the SMART_CONTRACT_ID environment variable, and doesn't need to be explicitly provided
        /// </param>        
        /// <returns></returns>
        public async Task<ApiResponse<string>> GetSmartContractObject(string key, string smartContractId)
        {
            if (string.IsNullOrWhiteSpace(key)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `key` is required"); }
            if (string.IsNullOrWhiteSpace(smartContractId))
            {
                if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("SMART_CONTRACT_ID")))
                {
                    throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `smartContractId` is required when not running within a smart contract");
                }
                smartContractId = Environment.GetEnvironmentVariable("SMART_CONTRACT_ID");
            }
            return await _httpService.GetAsync<string>($"/get/{smartContractId}/{key}");
        }

        /// <summary>
        /// List objects from a folder within the heap of a smart contract
        /// </summary>
        /// <param name="prefixKey">The folder to list from the heap. Please note this CANNOT end in a '/'
        /// If nothing is provided, it will list at the root of the heap
        /// </param>
        /// <param name="smartContractId">Smart contract to list the objects from
        /// When running from within a smart contract, this is provided via the SMART_CONTRACT_ID environment variable, and doesn't need to be explicitly provided
        /// </param>
        /// <returns></returns>
        public async Task<ApiResponse<IEnumerable<string>>> ListSmartcontractHeap(string prefixKey, string smartContractId)
        {
            if (string.IsNullOrWhiteSpace(smartContractId))
            {
                if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("SMART_CONTRACT_ID")))
                {
                    throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `smartContractId` is required when not running within a smart contract");
                }
                smartContractId = Environment.GetEnvironmentVariable("SMART_CONTRACT_ID");
            }
            var path = $"/list/{smartContractId}/";
            if (!string.IsNullOrWhiteSpace(prefixKey))
            {
                if (prefixKey.EndsWith("/")) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `prefixKey` cannot end with '/'"); }
                path = $"{path}/{prefixKey}/";
            }            
            return await _httpService.GetAsync<IEnumerable<string>>(path);
        }

        #endregion

        /// <summary>
        /// Get verifications for a block. Note that this is only relevant for level 1 chains
        /// </summary>
        /// <param name="blockId">The block ID to retrieve verifications for</param>
        /// <param name="level">The level of verifications to retrieve (2-5). If not supplied, all levels are returned</param>
        /// <returns></returns>
        public async Task<ApiResponse<IVerifications>> GetVerifications(string blockId, int level = 0)
        {
            if (string.IsNullOrWhiteSpace(blockId)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `blockId` is required"); }
            if (level > 0)
            {
                var levelresponse = await _httpService.GetAsync<IEnumerable<BlockSchemaType>>($"/verifications/{blockId}?level={level}");
                var levelVerifications = new ApiResponse<IVerifications> { Ok = levelresponse.Ok, Status = levelresponse.Status, Response = new LevelVerifications { Verifications = levelresponse.Response } };
                return levelVerifications;
            }
            var response = await _httpService.GetAsync<Verifications>($"/verifications/{blockId}");
            var verifications = new ApiResponse<IVerifications> { Ok = response.Ok, Status = response.Status, Response = response.Response };
            return verifications;
        }


        #region -- Interchain --

        /// <summary>
        /// Gets a list of the chain's interchain addresses
        /// </summary>
        public async Task<ApiResponse<PublicBlockchainAddressListResponse>> GetPublicBlockchainAddresses()
        {
            return await _httpService.GetAsync<PublicBlockchainAddressListResponse>("/public-blockchain-address");
        }

        /// <param name="network">The bitcoin network that the transaction is for (mainnet or testnet)</param>
        /// <param name="satoshisPerByte">The desired fee in satoshis/byte. If not supplied, an estimate will be automatically generated</param>
        /// <param name="data">String data to embed in the transaction as null-data output type</param>
        /// <param name="changeAddress">Change address to use for this transaction. If not supplied, this will be the source address</param>
        /// <param name="outputs">The desired bitcoin outputs to create for this transaction</param>                
        public async Task<ApiResponse<PublicBlockchainTransactionResponse>> CreateBitcoinTransaction(BitcoinNetwork? network, decimal? satoshisPerByte, string data, string changeAddress, IEnumerable<BitcoinTransactionOutputs> outputs)
        {
            if (network == null) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `network` is required"); }
            var body = new BitcoinTransactionRequest {
                Network = (BitcoinNetwork)network,
                Transaction = new BitcoinTransaction{
                    Fee = satoshisPerByte,
                    Data = data,
                    Change = changeAddress                    
                }
            };

            if (outputs.Any()) {
                var mappedOutputs = new List<Output>();
                foreach(var output in outputs)
                {
                    mappedOutputs.Add(new Output { To = output.ScriptPubKey, Value = output.Value });
                };
                body.Transaction.Outputs = mappedOutputs;
            }
            return await _httpService.PostAsync<PublicBlockchainTransactionResponse>("/public-blockchain-transaction", body);
        }

        /// <param name="network">The ethereum network that the transaction is for (ETH/ETC mainnet or testnet)</param>
        /// <param name="to">The (hex-encoded) address to send the transaction to</param>
        /// <param name="value">The (hex-encoded) number of wei to send with this transaction</param>
        /// <param name="data">The (hex-encoded) string of extra data to include with this transaction</param>
        /// <param name="gasPrice">The (hex-encoded) gas price in gwei to pay. If not supplied, this will be estimated automatically</param>                
        /// <param name="gas">The (hex-encoded) gas limit for this transaction. If not supplied, this will be estimated automatically</param>                
        public async Task<ApiResponse<PublicBlockchainTransactionResponse>> CreateEthereumTransaction(EthereumNetwork? network, string to, string value, string data, string gasPrice, string gas)
        {
            if (network == null) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `network` is required"); }
            if (string.IsNullOrWhiteSpace(to)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `to` is required"); }
            if (string.IsNullOrWhiteSpace(value)) { throw new FailureByDesignException(FailureCode.PARAM_ERROR, "Parameter `value` is required"); }
            var body = new EthereumTransactionRequest
            {
                Network = (EthereumNetwork)network,
                Transaction = new EthereumTransaction
                {
                    To = to,
                    Value = value,
                    Data = data,
                    GasPrice = gasPrice,
                    Gas = gas
                }
            };            
            return await _httpService.PostAsync<PublicBlockchainTransactionResponse>("/public-blockchain-transaction", body);
        }

        #endregion        
    }
}