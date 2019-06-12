using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using dragonchain_sdk.Contracts;
using dragonchain_sdk.Framework.Web;
using dragonchain_sdk.Status;
using dragonchain_sdk.Transactions.L1;
using dragonchain_sdk.Blocks;
using dragonchain_sdk.Transactions;
using dragonchain_sdk.Framework.Errors;
using dragonchain_sdk.Credentials.Manager;
using dragonchain_sdk.Credentials.Keys;
using dragonchain_sdk.Interchain;
using dragonchain_sdk.Interchain.Ethereum;
using dragonchain_sdk.Interchain.Networks;

namespace dragonchain_sdk.tests
{
    [TestFixture]
    public class DragonchainClientUnitTests
    {
        private Mock<IHttpService> _httpService;
        private Mock<ICredentialManager> _credentialManager;
        private DragonchainClient _dragonchainClient;
        private ILogger _logger;

        public DragonchainClientUnitTests()
        {   
            _logger = new TestLogger<DragonchainClient>();            
            _httpService = new Mock<IHttpService>();
            _credentialManager = new Mock<ICredentialManager>();
            _dragonchainClient = new DragonchainClient("fakeDragonchainId", null, null, _httpService.Object, _logger);
        }

        [SetUp]
        public void BeforEach()
        {
            _httpService.Invocations.Clear();
        }

        [Test]
        public void ConstructClient_Tests()
        {
            _credentialManager.Setup(manager => manager.GetDragonchainId()).Returns("fakeDragonchainId");
            IDragonchainClient dragonchainClient;
            Assert.DoesNotThrow(() => dragonchainClient = new DragonchainClient("fakeDragonchainId"));            
            Assert.Throws<FailureByDesignException>(() => dragonchainClient = new DragonchainClient(), "No configuration provider set");
        }                

        [Test]
        public async Task GetStatus_CallswithCorrectParams_Test()
        {            
            await _dragonchainClient.GetStatus();
            _httpService.Verify(service => service.GetAsync<L1DragonchainStatusResult>("/status"), Times.Once);
        }

        [Test]
        public async Task GetApiKey_CallswithCorrectParams_Test()
        {
            await _dragonchainClient.GetApiKey("MyKeyID");
            _httpService.Verify(service => service.GetAsync<GetAPIKeyResponse>("/api-key/MyKeyID"), Times.Once);
        }

        [Test]
        public async Task ListApiKeys_CallswithCorrectParams_Test()
        {
            await _dragonchainClient.ListApiKeys();
            _httpService.Verify(service => service.GetAsync<ListAPIKeyResponse>("/api-key"), Times.Once);
        }

        [Test]
        public async Task GetTransaction_CallswithCorrectParams_Test()
        {
            var id = "batman-transaction-id";
            await _dragonchainClient.GetTransaction(id);
            _httpService.Verify(service => service.GetAsync<L1DragonchainTransactionFull>($"/transaction/{id}"), Times.Once);
        }
        
        [Test]
        public async Task GetBlock_CallswithCorrectParams_Test()
        {               
            var id = "robin-block-id";
            await _dragonchainClient.GetBlock(id);
            _httpService.Verify(service => service.GetAsync<BlockSchemaType>($"/block/{id}"), Times.Once);
        }

        [Test]
        public async Task GetSmartContract_CallswithCorrectParams_Test()
        {
            var id = "joker-smartcontract-id";
            await _dragonchainClient.GetSmartContract(id, "");
            _httpService.Verify(service => service.GetAsync<SmartContractAtRest>($"/contract/{id}"), Times.Once);
        }

        [Test]
        public async Task GetPublicBlockchainAddresses_CallswithCorrectParams_Test()
        {            
            await _dragonchainClient.GetPublicBlockchainAddresses();
            _httpService.Verify(service => service.GetAsync<PublicBlockchainAddressListResponse>("/public-blockchain-address"), Times.Once);
        }

        [Test]
        public async Task GetVerification_CallswithCorrectParams_Test()
        {
            var levelVerificationsReponse = new ApiResponse<Verifications> { Ok = true, Status = 200, Response = new Verifications() };
            _httpService.Setup(service => service.GetAsync<Verifications>(It.IsAny<string>())).ReturnsAsync(levelVerificationsReponse);
            var id = "block_id";
            await _dragonchainClient.GetVerifications(id);            
            _httpService.Verify(service => service.GetAsync<Verifications>($"/verifications/{id}"), Times.Once);

            var verificationsReponse = new ApiResponse<IEnumerable<BlockSchemaType>> { Ok = true, Status = 200, Response = new List<BlockSchemaType>() };
            _httpService.Setup(service => service.GetAsync<IEnumerable<BlockSchemaType>>(It.IsAny<string>())).ReturnsAsync(verificationsReponse);
            var level = 2;            
            await _dragonchainClient.GetVerifications(id, level);
            _httpService.Verify(service => service.GetAsync<IEnumerable<BlockSchemaType>>($"/verifications/{id}?level={level}"), Times.Once);
        }

        [Test]
        public async Task QueryBlocks_CallswithCorrectParams_Test()
        {
            var @params = "banana";
            await _dragonchainClient.QueryBlocks(@params);
            _httpService.Verify(service => service.GetAsync<QueryResult<BlockSchemaType>>($"/block?q={@params}&offset=0&limit=10"), Times.Once);

            await _dragonchainClient.QueryBlocks(@params, offset: 5, limit: 12);
            _httpService.Verify(service => service.GetAsync<QueryResult<BlockSchemaType>>($"/block?q={@params}&offset=5&limit=12"), Times.Once);
        }

        [Test]
        public async Task QueryTransactions_CallswithCorrectParams_Test()
        {
            var @params = "banana";
            await _dragonchainClient.QueryTransactions(@params);
            _httpService.Verify(service => service.GetAsync<QueryResult<L1DragonchainTransactionFull>>($"/transaction?q={@params}&offset=0&limit=10"), Times.Once);

            await _dragonchainClient.QueryTransactions(@params, offset: 5, limit: 12);
            _httpService.Verify(service => service.GetAsync<QueryResult<L1DragonchainTransactionFull>>($"/transaction?q={@params}&offset=5&limit=12"), Times.Once);
        }

        [Test]
        public async Task QuerySmartContracts_CallswithCorrectParams_Test()
        {
            var @params = "banana";
            await _dragonchainClient.QuerySmartContracts(@params);
            _httpService.Verify(service => service.GetAsync<QueryResult<SmartContractAtRest>>($"/contract?q={@params}&offset=0&limit=10"), Times.Once);

            await _dragonchainClient.QuerySmartContracts(@params, offset: 5, limit: 12);
            _httpService.Verify(service => service.GetAsync<QueryResult<SmartContractAtRest>>($"/contract?q={@params}&offset=5&limit=12"), Times.Once);
        }

        [Test]
        public async Task DeleteSmartContract_CallswithCorrectParams_Test()
        {
            var id = "banana";
            await _dragonchainClient.DeleteSmartContract(id);
            _httpService.Verify(service => service.DeleteAsync<SmartContractAtRest>($"/contract/{id}"), Times.Once);
        }

        [Test]
        public async Task DeleteApiKey_CallswithCorrectParams_Test()
        {            
            await _dragonchainClient.DeleteApiKey("MyKeyID");
            _httpService.Verify(service => service.DeleteAsync<DeleteAPIKeyResponse>("/api-key/MyKeyID"), Times.Once);
        }

        [Test]
        public async Task CreatedApiKey_CallswithCorrectParams_Test()
        {
            await _dragonchainClient.CreateApiKey();
            _httpService.Verify(service => service.PostAsync<CreateAPIKeyResponse>("/api-key", null, ""), Times.Once);
        }

        [Test]
        public async Task CreateTransaction_CallswithCorrectParams_Test()
        {
            var transactionCreatePayload = new DragonchainTransactionCreatePayload
            {
                Version = "1",
                TransactionType= "transaction",
                Payload = "hi!" ,
                Tag = "Awesome!"
            };
            await _dragonchainClient.CreateTransaction(transactionCreatePayload.TransactionType, transactionCreatePayload.Payload, transactionCreatePayload.Tag);
            _httpService.Verify(service => service.PostAsync<DragonchainTransactionCreateResponse>("/transaction", It.IsAny<DragonchainTransactionCreatePayload>(), ""), Times.Once);
        }

        [Test]
        public async Task CreateContract_CallswithCorrectParams_Test()
        {
            var body = new SmartContractSchema
            {
                TransactionType = "name",
                Image = "ubuntu:latest",
                ExecutionOrder = SmartContractExecutionOrder.Serial,
                EnvironmentVariables = new { Banana = "banana", Apple = "banana" },
                Cmd = "banana",
                Arguments = new string[] { "-m cool" }
            };
            await _dragonchainClient.CreateSmartContract(body.TransactionType, body.Image, body.Cmd, body.Arguments, (SmartContractExecutionOrder)body.ExecutionOrder, body.EnvironmentVariables);
            _httpService.Verify(service => service.PostAsync<DragonchainContractResponse>("/contract", It.IsAny<SmartContractSchema>(), ""), Times.Once);
        }

        [Test]
        public async Task CreateEthereumTransaction_CallswithCorrectParams_Test()
        {
            var request = new EthereumTransactionRequest
            {
                Network = EthereumNetwork.ETC_MAINNET,
                Transaction = new EthereumTransaction
                {
                    To = "0x0000000000000000000000000000000000000000",
                    Value = "0x0",
                    Data = "0x222",
                    GasPrice = "0x222",
                    Gas = "0x333"
                }
            };
            await _dragonchainClient.CreateEthereumTransaction(request.Network, request.Transaction.To, request.Transaction.Value, request.Transaction.Data, request.Transaction.GasPrice, request.Transaction.Gas);
            _httpService.Verify(service => service.PostAsync<PublicBlockchainTransactionResponse>("/public-blockchain-transaction", It.IsAny<EthereumTransactionRequest>(), ""), Times.Once);
        }

        [Test]
        public async Task UpdateSmartContract_CallswithCorrectParams_Test()
        {
            var id = "616152367378";                           
            await _dragonchainClient.UpdateSmartContract(id);
            _httpService.Verify(service => service.PutAsync<DragonchainContractResponse>($"/contract/{id}", It.IsAny<object>()), Times.Once);
        }
    }
}

/**
 * All Humans are welcome.
 */
