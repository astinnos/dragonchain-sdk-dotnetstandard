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
using dragonchain_sdk.Shared;
using dragonchain_sdk.Framework.Errors;
using dragonchain_sdk.Credentials.Manager;

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
        public void SetDragonchainId_AllowsResettingtheDragonchainId_Test()
        {
            var dragonchainClient = new DragonchainClient("fakeDragonchainId", null, null, _httpService.Object, _logger);
            dragonchainClient.SetDragonchainId("hotBanana");
            _httpService.Verify(service => service.SetEndpoint("https://hotBanana.api.dragonchain.com"), Times.Once);
        }

        [Test]
        public void SetEndpoint_AllowsSettingtheEnpointManually_Test()
        {
            var dragonchainClient = new DragonchainClient("fakeDragonchainId", null, null, _httpService.Object, _logger);
            var endpoint = "https://some.domain.com";
            dragonchainClient.SetEndpoint(endpoint);
            _httpService.Verify(service => service.SetEndpoint($"{endpoint}"), Times.Once);
        }

        [Test]
        public async Task GetStatus_CallswithCorrectParams_Test()
        {            
            await _dragonchainClient.GetStatus();
            _httpService.Verify(service => service.GetAsync<L1DragonchainStatusResult>("/status"), Times.Once);
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
            await _dragonchainClient.GetSmartContract(id);
            _httpService.Verify(service => service.GetAsync<SmartContractAtRest>($"/contract/{id}"), Times.Once);
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
            _httpService.Verify(service => service.GetAsync<DragonchainBlockQueryResult>($"/block?q={@params}&offset=0&limit=10"), Times.Once);

            await _dragonchainClient.QueryBlocks(@params, offset: 5, limit: 12);
            _httpService.Verify(service => service.GetAsync<DragonchainBlockQueryResult>($"/block?q={@params}&offset=5&limit=12"), Times.Once);
        }

        [Test]
        public async Task QueryTransactions_CallswithCorrectParams_Test()
        {
            var @params = "banana";
            await _dragonchainClient.QueryTransactions(@params);
            _httpService.Verify(service => service.GetAsync<L1DragonchainTransactionQueryResult>($"/transaction?q={@params}&offset=0&limit=10"), Times.Once);

            await _dragonchainClient.QueryTransactions(@params, offset: 5, limit: 12);
            _httpService.Verify(service => service.GetAsync<L1DragonchainTransactionQueryResult>($"/transaction?q={@params}&offset=5&limit=12"), Times.Once);
        }

        [Test]
        public async Task QuerySmartContracts_CallswithCorrectParams_Test()
        {
            var @params = "banana";
            await _dragonchainClient.QuerySmartContracts(@params);
            _httpService.Verify(service => service.GetAsync<DragonchainSmartContractQueryResult>($"/contract?q={@params}&offset=0&limit=10"), Times.Once);

            await _dragonchainClient.QuerySmartContracts(@params, offset: 5, limit: 12);
            _httpService.Verify(service => service.GetAsync<DragonchainSmartContractQueryResult>($"/contract?q={@params}&offset=5&limit=12"), Times.Once);
        }

        [Test]
        public async Task DeleteSmartContract_CallswithCorrectParams_Test()
        {
            var id = "banana";
            await _dragonchainClient.DeleteSmartContract(id);
            _httpService.Verify(service => service.DeleteAsync<UpdateResponse>($"/contract/{id}"), Times.Once);
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
            await _dragonchainClient.CreateTransaction(transactionCreatePayload);
            _httpService.Verify(service => service.PostAsync<DragonchainTransactionCreateResponse>("/transaction", transactionCreatePayload), Times.Once);
        }

        [Test]
        public async Task CreateContract_CallswithCorrectParams_Test()
        {
            var contractPayload = new ContractCreationSchema
            {
                TransactionType = "name",
                Image = "ubuntu:latest",
                ExecutionOrder = SmartContractExecutionOrder.Serial,
                EnvironmentVariables = new { Banana = "banana", Apple = "banana" },
                Cmd = "banana",
                Arguments = new string[] { "-m cool" }
            };
            await _dragonchainClient.CreateContract(contractPayload);
            _httpService.Verify(service => service.PostAsync<DragonchainContractCreateResponse>("/contract", contractPayload), Times.Once);
        }

        [Test]
        public async Task UpdateSmartContract_CallswithCorrectParams_Test()
        {
            var id = "616152367378";
            var status = SmartContractDesiredState.Active;        
            
            await _dragonchainClient.UpdateSmartContract(id, null, null, null, status);
            _httpService.Verify(service => service.PutAsync<UpdateResponse>($"/contract/{id}", It.IsAny<object>()), Times.Once);
        }
    }
}

/**
 * All Humans are welcome.
 */
