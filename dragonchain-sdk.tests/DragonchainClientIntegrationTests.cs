using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using dragonchain_sdk.Credentials;
using dragonchain_sdk.Status;

namespace dragonchain_sdk.tests
{
    [TestFixture]
    public class DragonchainClientIntegrationTests
    {
        private string _id = "";
        private string _authKeyId = "";
        private string _authKey = "";
        private DragonchainClient _dragonchainClient;

        public DragonchainClientIntegrationTests()
        {
            if (IsTestConfigured())
            {
                var credentialService = new CredentialService(_id, _authKey, _authKeyId);
                _dragonchainClient = new DragonchainClient(_id, null, credentialService);
            }
        }        

        [Test]
        public async Task GetStatus_Test()
        {            
            if (IsTestConfigured())
            {                
                var result = await _dragonchainClient.GetStatus();                
                Assert.AreEqual("200", result.Status);
                Assert.IsTrue(result.Ok);
                Assert.IsInstanceOf<L1DragonchainStatusResult>(result.Response);
            }            
        }

        public bool IsTestConfigured()
        {
            return (!string.IsNullOrWhiteSpace(_id) & !string.IsNullOrWhiteSpace(_authKey) && !string.IsNullOrWhiteSpace(_authKeyId));
        }
    }
}
