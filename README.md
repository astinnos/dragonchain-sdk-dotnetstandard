# Dragonchain .Net SDK

[![NuGet](https://img.shields.io/badge/nuget-v1.0.0--alpha-blue.svg)](https://www.nuget.org/packages/dragonchain-sdk-dotnet/)

Talk to your dragonchain.

### Installation
First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [Dragonchain .Net SDK](https://www.nuget.org/packages/dragonchain-sdk-dotnet/) from the package manager console:

Pre -release
```
PM> Install-Package dragonchain-sdk-dotnet -Version 1.0.0-alpha
```

### Examples

### Import
```csharp
using dragonchain_sdk;
using dragonchain_sdk.Framework.Web;
```

#### OverrideCredentials

This is fine for quick tests. For actual production use, you should inject an implementation of Microsoft.Extensions.Configuration.IConfiguration. Read here for more information [Mircosoft Configuration Docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.2).   
[Examples](#configuration)

```csharp
client.OverrideCredentials("AUTH_KEY_ID", "AUTH_KEY");
```

#### GetBlock

```csharp
var myDcId = "3f2fef78-0000-0000-0000-9f2971607130";
var client = new DragonchainClient(myDcId);

const call = await client.getBlock('block-id-here');

try 
{
    var block = await client.GetBlock("block-id-here");
    var block = call.Response;
    Console.WriteLine("Successful call!");
    Console.WriteLine($"Block: {block.Header.BlockId}");
}
catch(DragonchainApiException exception)
{
    Console.WriteLine("Something went wrong!");
    Console.WriteLine($"HTTP status code from chain: {exception.Status}");
    Console.WriteLine($"Error response from chain: {exception.Message}");
}
```

#### QueryTransactions

```csharp
var searchResult = await client.QueryTransactions("tag=MyAwesomeTransactionTag");
var totalTransactionsCount = searchResult.Response.Total;
var transactions = searchResult.Response.Results;
```

#### Register Transaction Type

```csharp
var registerTransactionTypeResult = await _dragonchainLevel1Client.RegisterTransactionType(
new TransactionTypeStructure
{
    Version = "1",
    TransactionType = "apple",
    CustomIndexes = new List<CustomIndexStructure>
    {
        new CustomIndexStructure{ Key ="SomeKey", Path="SomePath" }
    }
});
```

#### Create a Transaction

```csharp
var newTransaction = new DragonchainTransactionCreatePayload
{
    TransactionType = "apple",
    Version = "1",
    Tag = "pottery",
    Payload = new {}
};
var createResult = await _dragonchainLevel1Client.CreateTransaction(newTransaction);
```

## Configuration

In order to use this SDK, you need to have an Auth Key as well as an Auth Key ID for a given dragonchain.
This can be loaded into the sdk in various ways using an IConfiguration provider:

1. The environment variables `AUTH_KEY` and `AUTH_KEY_ID` can be set with the appropriate values
2. Write a json, ini or xml file and use the required Configuration Builder extension like so:

### Environment Variables

```csharp
  var config = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();    
  var client = new DragonchainClient(myDcId, config);
```

### Json

```csharp
  var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();    
  var client = new DragonchainClient(myDcId, config);
```

```json
{
  "dragonchainId": "3f2fef78-0000-0000-0000-9f2971607130",
  "AUTH_KEY": "MyAuthKey",
  "AUTH_KEY_ID": "MyAuthKeyId"
 }
 ```
 or
 ```json
{
  "3f2fef78-0000-0000-0000-9f2971607130": {
    "AUTH_KEY": "MyAuthKey",
    "AUTH_KEY_ID": "MyAuthKeyId"
  }
 }
 ```

### INI

```csharp
  var config = new ConfigurationBuilder()
    .AddIniFile("config.ini")
    .Build();    
  var client = new DragonchainClient(myDcId, config);
```

```ini
dragonchainId=3f2fef78-0000-0000-0000-9f2971607130
AUTH_KEY=MyAuthKey
AUTH_KEY_ID=MyAuthKeyId
```
or
```ini
[3f2fef78-0000-0000-0000-9f2971607130]
AUTH_KEY=MyAuthKey
AUTH_KEY_ID=MyAuthKeyId
```

### XML

```csharp
  var config = new ConfigurationBuilder()
    .AddXmlFile("config.xml")
    .Build();    
  var client = new DragonchainClient(myDcId, config);
```

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <dragonchainId>3f2fef78-0000-0000-0000-9f2971607130</dragonchainId>
  <AUTH_KEY>MyAuthKey</AUTH_KEY>
  <AUTH_KEY_ID>MyAuthKeyId</AUTH_KEY_ID>
</configuration>
```
or
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>  
  <3f2fef78-0000-0000-0000-9f2971607130>
    <AUTH_KEY>MyAuthKey</AUTH_KEY>
    <AUTH_KEY_ID>MyAuthKeyId</AUTH_KEY_ID>  
  </3f2fef78-0000-0000-0000-9f2971607130>
</configuration>
```

## Logging

In order to get the logging output of the sdk, a logger must be set (by default all logging is thrown away).

In order to set the logger, simply inject a Microsoft.Extensions.Logging implementation. 
Read here for more information [Microsoft Logging Docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.2). 
For example, if you just wanted to log to the console you can set the logger like the following:

```csharp
var webHost = new WebHostBuilder()        
  .ConfigureLogging((hostingContext, logging) =>
  {
    logging.AddConsole();            
  })
  .UseStartup<Startup>()
  .Build();
```

You can also create your own implemnations of ILogger

```csharp
var logger = new MyLogger();
var client = new DragonchainClient(myDcId, config, logger);                
```
