using NUnit.Framework;
using dragonchain_sdk.Credentials;

namespace dragonchain_sdk.tests
{
    [TestFixture]
    public class CredentialServiceTests
    {        
        [Test]        
        public void GetAuthorizationHeader_Test()
        {
            var service = new CredentialService("testId", "key", "keyId");
            Assert.AreEqual("DC1-HMAC-SHA256 keyId:8Bc+h0parZxGeMB9rYzzRUuNxxHSIjGqSD4W/635A9k=",
                service.GetAuthorizationHeader("GET", "/path", "timestamp", "application/json", ""));

            Assert.AreEqual("DC1-HMAC-SHA256 keyId:PkVjUxWZr6ST4xh+JxYFZresaFhQbk8sggWqyWv/XkU=",
                service.GetAuthorizationHeader("POST", "/new_path", "timestamp", "application/json", "\"body\""));
        }
    }
}
