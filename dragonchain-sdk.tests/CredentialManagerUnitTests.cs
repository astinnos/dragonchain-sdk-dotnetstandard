using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using dragonchain_sdk.Credentials.Manager;
using System.Collections.Generic;
using System;
using dragonchain_sdk.Framework.Errors;

namespace dragonchain_sdk.tests
{
    [TestFixture]
    public class CredentialManagerUnitTests
    {
        [Test]
        public void NullConfigTest()
        {
            var credentialManager = new CredentialManager(null);
            Assert.Throws<FailureByDesignException>(() => credentialManager.GetDragonchainId(), "No configuration provider set");
            Assert.Throws<FailureByDesignException>(() => credentialManager.GetDragonchainCredentials(), "No configuration provider set");
        }

        [Test]
        public void MissingKeysConfigTest()
        {
            var credentials = new Dictionary<string, string> { {"AUTH_KEY", "configAuthKey"} };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(credentials)
                .Build();
            var credentialManager = new CredentialManager(null);
            Assert.Throws<FailureByDesignException>(() => credentialManager.GetDragonchainId(), "Config does not contain key 'dragonchainId");
            Assert.Throws<FailureByDesignException>(() => credentialManager.GetDragonchainCredentials(), "Config does not contain both keys 'AUTH_KEY' and 'AUTH_KEY_ID");
            credentials.Clear();
            credentials.Add("AUTH_KEY_ID", "configAuthKeyId");
            Assert.Throws<FailureByDesignException>(() => credentialManager.GetDragonchainCredentials(), "Config does not contain both keys 'AUTH_KEY' and 'AUTH_KEY_ID");
        }

        [Test]
        public void InMemoryConfig_Test()
        {
            var credentials = new Dictionary<string, string>
            {
                {"dragonchainId", "configTestId"},
                {"AUTH_KEY", "configAuthKey"},
                {"AUTH_KEY_ID", "configAuthKeyId"},
                {"fakeDragonchainId:AUTH_KEY", "fakeDragonchainIdConfigAuthKey"},
                {"fakeDragonchainId:AUTH_KEY_ID", "fakeDragonchainIdConfigAuthKeyId"}                
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(credentials)
                .Build();

            var credentialManager = new CredentialManager(config: config);
            Assert.AreEqual("configTestId", credentialManager.GetDragonchainId());

            var dragonchainCredentials = credentialManager.GetDragonchainCredentials();
            Assert.AreEqual("configAuthKeyId", dragonchainCredentials.AuthKeyId);
            Assert.AreEqual("configAuthKey", dragonchainCredentials.AuthKey);

            var fakeDragonchainIdDragonchainCredentials = credentialManager.GetDragonchainCredentials("fakeDragonchainId");
            Assert.AreEqual("fakeDragonchainIdConfigAuthKeyId", fakeDragonchainIdDragonchainCredentials.AuthKeyId);
            Assert.AreEqual("fakeDragonchainIdConfigAuthKey", fakeDragonchainIdDragonchainCredentials.AuthKey);
        }

        [Test]
        public void EnvironmentVariablesConfig_Test()
        {
            Environment.SetEnvironmentVariable("dragonchainId", "configTestId");
            Environment.SetEnvironmentVariable("AUTH_KEY", "configAuthKey");
            Environment.SetEnvironmentVariable("AUTH_KEY_ID", "configAuthKeyId");
            Environment.SetEnvironmentVariable("fakeDragonchainId:AUTH_KEY", "fakeDragonchainIdConfigAuthKey");
            Environment.SetEnvironmentVariable("fakeDragonchainId:AUTH_KEY_ID", "fakeDragonchainIdConfigAuthKeyId");
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var credentialManager = new CredentialManager(config: config);
            Assert.AreEqual("configTestId", credentialManager.GetDragonchainId());

            var dragonchainCredentials = credentialManager.GetDragonchainCredentials();
            Assert.AreEqual("configAuthKeyId", dragonchainCredentials.AuthKeyId);
            Assert.AreEqual("configAuthKey", dragonchainCredentials.AuthKey);

            var fakeDragonchainIdDragonchainCredentials = credentialManager.GetDragonchainCredentials("fakeDragonchainId");
            Assert.AreEqual("fakeDragonchainIdConfigAuthKeyId", fakeDragonchainIdDragonchainCredentials.AuthKeyId);
            Assert.AreEqual("fakeDragonchainIdConfigAuthKey", fakeDragonchainIdDragonchainCredentials.AuthKey);
        }

        [Test]
        public void JsonConfig_Test()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var credentialManager = new CredentialManager(config: config);
            Assert.AreEqual("configTestId", credentialManager.GetDragonchainId());

            var dragonchainCredentials = credentialManager.GetDragonchainCredentials();
            Assert.AreEqual("configAuthKeyId", dragonchainCredentials.AuthKeyId);
            Assert.AreEqual("configAuthKey", dragonchainCredentials.AuthKey);

            var fakeDragonchainIdDragonchainCredentials = credentialManager.GetDragonchainCredentials("fakeDragonchainId");
            Assert.AreEqual("fakeDragonchainIdConfigAuthKeyId", fakeDragonchainIdDragonchainCredentials.AuthKeyId);
            Assert.AreEqual("fakeDragonchainIdConfigAuthKey", fakeDragonchainIdDragonchainCredentials.AuthKey);
        }

        [Test]
        public void XmlConfig_Test()
        {
            var config = new ConfigurationBuilder()
                .AddXmlFile("config.xml")
                .Build();

            var credentialManager = new CredentialManager(config: config);
            Assert.AreEqual("configTestId", credentialManager.GetDragonchainId());

            var dragonchainCredentials = credentialManager.GetDragonchainCredentials();
            Assert.AreEqual("configAuthKeyId", dragonchainCredentials.AuthKeyId);
            Assert.AreEqual("configAuthKey", dragonchainCredentials.AuthKey);

            var fakeDragonchainIdDragonchainCredentials = credentialManager.GetDragonchainCredentials("fakeDragonchainId");
            Assert.AreEqual("fakeDragonchainIdConfigAuthKeyId", fakeDragonchainIdDragonchainCredentials.AuthKeyId);
            Assert.AreEqual("fakeDragonchainIdConfigAuthKey", fakeDragonchainIdDragonchainCredentials.AuthKey);
        }

        [Test]
        public void IniConfig_Test()
        {
            var config = new ConfigurationBuilder()
                .AddIniFile("config.ini")
                .Build();

            var credentialManager = new CredentialManager(config: config);
            Assert.AreEqual("configTestId", credentialManager.GetDragonchainId());

            var dragonchainCredentials = credentialManager.GetDragonchainCredentials();
            Assert.AreEqual("configAuthKeyId", dragonchainCredentials.AuthKeyId);
            Assert.AreEqual("configAuthKey", dragonchainCredentials.AuthKey);

            var fakeDragonchainIdDragonchainCredentials = credentialManager.GetDragonchainCredentials("fakeDragonchainId");
            Assert.AreEqual("fakeDragonchainIdConfigAuthKeyId", fakeDragonchainIdDragonchainCredentials.AuthKeyId);
            Assert.AreEqual("fakeDragonchainIdConfigAuthKey", fakeDragonchainIdDragonchainCredentials.AuthKey);
        }
    }
}
