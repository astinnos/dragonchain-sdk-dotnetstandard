using System.Collections.Generic;
using System.Threading.Tasks;
using dragonchain_sdk.Blocks;
using dragonchain_sdk.Contracts;
using dragonchain_sdk.DragonNet;
using dragonchain_sdk.Framework.Web;
using dragonchain_sdk.Shared;
using dragonchain_sdk.Status;
using dragonchain_sdk.Transactions;
using dragonchain_sdk.Transactions.L1;
using dragonchain_sdk.Transactions.Types;

namespace dragonchain_sdk
{
    public interface IDragonchainClient
    {
        Task<ApiResponse<DragonchainTransactionCreateResponse>> CreateBulkTransaction(IEnumerable<DragonchainTransactionCreatePayload> transactionObjects);
        Task<ApiResponse<DragonchainContractCreateResponse>> CreateContract(ContractCreationSchema body);
        Task<ApiResponse<DragonchainTransactionCreateResponse>> CreateTransaction(DragonchainTransactionCreatePayload transactionObject);
        Task<ApiResponse<UpdateResponse>> DeleteSmartContract(string contractId);
        Task<ApiResponse<UpdateResponse>> DeleteTransactionType(string transactionType);
        Task<ApiResponse<BlockSchemaType>> GetBlock(string blockId);        
        Task<ApiResponse<SmartContractAtRest>> GetSmartContract(string contractId);
        Task<ApiResponse<string>> GetSmartContractHeap(string key, string scName, bool jsonParse = false);
        Task<ApiResponse<L1DragonchainStatusResult>> GetStatus();
        Task<ApiResponse<L1DragonchainTransactionFull>> GetTransaction(string transactionId);
        Task<ApiResponse<IVerifications>> GetVerifications(string blockId, int level = 0);
        Task<ApiResponse<IEnumerable<string>>> ListSmartcontractHeap(string scName, string key = "");
        Task<ApiResponse<DragonchainTransactionTypeQueryResult>> ListTransactionTypes();
        void OverrideCredentials(string authKeyId, string authKey);
        Task<ApiResponse<DragonchainBlockQueryResult>> QueryBlocks(string luceneQuery = "", string sort = "", int offset = 0, int limit = 10);
        Task<ApiResponse<DragonchainSmartContractQueryResult>> QuerySmartContracts(string luceneQuery = "", string sort = "", int offset = 0, int limit = 10);
        Task<ApiResponse<L1DragonchainTransactionQueryResult>> QueryTransactions(string luceneQuery = "", string sort = "", int offset = 0, int limit = 10);
        Task<ApiResponse<UpdateResponse>> RegisterTransactionType(TransactionTypeStructure txnTypeStructure);
        void SetDragonchainId(string dragonchainId, bool setEndpoint = true);
        void SetEndpoint(string endpoint);
        Task<ApiResponse<UpdateResponse>> UpdateDragonnetConfig(DragonnetConfigSchema maximumPrices);
        Task<ApiResponse<UpdateResponse>> UpdateMatchmakingConfig(decimal? askingPrice = null, int? broadcastInterval = null);
        Task<ApiResponse<UpdateResponse>> UpdateSmartContract(string contractId, string image = null, string cmd = null, SmartContractExecutionOrder? executionOrder = null, SmartContractDesiredState? desiredState = null, string[] args = null, object env = null, object secrets = null, int? seconds = null, string cron = null, string auth = null);
        Task<ApiResponse<UpdateResponse>> UpdateTransactionType(string transactionType, IEnumerable<CustomIndexStructure> customIndexes);
    }
}