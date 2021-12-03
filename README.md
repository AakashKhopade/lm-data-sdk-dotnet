# LogicMonitor.DataSDK - the C# library for the LogicMonitor API-Ingest Rest API
[![CodeQL](https://github.com/AakashKhopade/lm-data-sdk-dotnet/actions/workflows/codeql-analysis.yml/badge.svg?branch=main&event=workflow_run)](https://github.com/AakashKhopade/lm-data-sdk-dotnet/actions/workflows/codeql-analysis.yml)

LogicMonitor is a SaaS-based performance monitoring platform that provides full visibility into complex, hybrid 
infrastructures, offering granular performance monitoring and actionable data and insights. API-Ingest provides the 
entry point in the form of public rest APIs for ingesting metrics into LogicMonitor. For using this application users 
have to create LMAuth token using access id and key from santaba.

- SDK version: 0.0.5-alpha

<a name="frameworks-supported"></a>
## Frameworks supported
- .NET Core >= 3.1

<a name="dependencies"></a>
## Dependencies

- [RestSharp](https://www.nuget.org/packages/RestSharp) - 106.13.0 or later
- [Json.NET](https://www.nuget.org/packages/Newtonsoft.Json/) - 12.0.3 or later
- [Microsoft.Extenstion.Logging](https://www.nuget.org/packages/Microsoft.Extensions.Logging/) - 5.0.0 or later
- [Microsoft.Extenstion.Hosting](https://www.nuget.org/packages/Microsoft.Extensions.Hosting/) - 5.0.0 or later



<a name = "Configration"></a>
## Configration
SDK must be configured with LogicMonitor.DataSDK Configuration. An API LmAccessId, LmAccessKey and Type are required.
Authenticate class is to used set the values and its object will be passed to configration class along with account(company) name.

```csharp
Authenticate authenticate = new Authenticate();
authenticate.Id = Environment.GetEnvironmentVariable("LmId");
authenticate.Key = Environment.GetEnvironmentVariable("LmKey");
authenticate.Type = Environment.GetEnvironmentVariable("LmType");
Configuration configuration = new Configuration(company: Environment.GetEnvironmentVariable("LmCompany"), authentication: authenticate);
```

<a name = "Batching Metrics & Log Ingestion"></a>
## Batching Metrics & Log Ingestion.

When their is requirement that data is not to be ingested immediately after generation, user can ingest data in bulk i.e multiple metrics and log is to be ingested in single request.

To enable batching, batchs is to be set to 'True' along with proper interval(in seconds) which determine the time between the variable.
For the set interval, SDK waits for all the data to generate and ingest all the data after completion of interval. 

NOTE:Make sure data should not be older than 10 mins.
```csharp
ApiClients apiClients = new ApiClients(configuration);

Resource resource = new Resource(name: resourceName, ids: resourceIds, create: true);
DataSource dataSource = new DataSource(Name: dataSourceName, Group: dataSourceGroupName);
DataSourceInstance dataSourceInstance = new DataSourceInstance(name: dataSouceInstanceName);
DataPoint open = new DataPoint(name: "High");
Dictionary<string, string> highValue = new Dictionary<string, string>();
    
    
Metrics metrics = new Metrics(batchs: true, intervals: 100, responseInterface, apiClients);
highValue.Add(DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), item.SelectToken("high").ToString());
    
metrics.SendMetrics(resource: resource, dataSource: dataSource, dataSourceInstance: dataSourceInstance, dataPoint: open, values: openValue);

Logs logs = new Logs(batchs: true, intervals: 100, responseCallbacks: responseInterface, apiClients: apiClients);
logs.SendLogs(message: msg, resource: resource);
```

Then Run the program as:
```csharp
export id=<LM_AccessID>  api_key=<LM_AccessKey> type=<'LMv1'/'Bearer'> Lm_company=<LM_AccountName>
dotnet run
```

<a name="Model"></a>
## Model
- Resource

```csharp
Resource resource = new Resource(Ids,Name,Description,Properties,Create);
```

<b>Ids(Dictonary<string,string>):</b> An Dictionary of existing resource properties that will be used to identify the resource. See Managing Resources 
that Ingest Push Metrics for information on the types of properties that can be used. If no resource is matched and the 
create parameter is set to TRUE, a new resource is created with these specified resource IDs set on it. If the 
system.displayname and/or system.hostname property is included as resource IDs, they will be used as host name and 
display name respectively in the resulting resource.

<b>Name(string):</b> Resource unique name. Only considered when creating a new resource.

<b>Properties(Dictonary<string,string>):</b> New properties for resource. Updates to existing resource properties are not considered. Depending on the property name,
we will convert these properties into system, auto, or custom properties.

<b>Description(string):</b>  Resource description. Only considered when creating a new resource.

<b>Create(bool):</b> Do you want to create the resource.

- DataSource

```csharp
DataSource dataSource = new DataSource(DataSourceName,DataSourceGroup,DisplayName,Id );
```

<b>Name(string):</b>  DataSource unique name. Used to match an existing DataSource. If no existing DataSource matches the name provided
here, a new DataSource is created with this name.

<b>DisplayName(string):</b> DataSource display name. Only considered when creating a new DataSource.

<b>Group(string):</b> DataSource group name. Only considered when DataSource does not already belong to a group. Used to organize the
DataSource within a DataSource group. If no existing DataSource group matches, a new group is created with this name 
and the DataSource is organized under the new group.

<b>Id(int):</b> DataSource unique ID. Used only to match an existing DataSource. If no existing DataSource matches the provided ID, 
an error results.


- DatasourceInstance

```csharp
DataSourceInstance dataSourceInstance = new DataSourceInstance(Name,DisplayName,Description,Properties);
```
<b>Name(string):</b> Instance name. If no existing instance matches, a new instance is created with this name.

<b>DisplayName(string):</b> Instance display name. Only considered when creating a new instance.

<b>Properties(Dictionary<string,string>):</b> New properties for instance. Updates to existing instance properties are not considered. Depending on the 
property name, we will convert these properties into system, auto, or custom properties.

<b>Description(string):</b>  Resource description. Only considered when creating a new resource.

- DataPoint

```csharp
DataPoint dataPoint = new DataPoint(Name,Description,AggregationType,Description);
```
<b>Name(string):</b> Datapoint name. If no existing datapoint matches for specified DataSource, a new datapoint is created with this 
name.

<b>AggreationType(string):</b>The aggregation method, if any, that should be used if data is pushed in sub-minute intervals. Allowed options are 
“sum”, “average” and “none”(default) where “none” would take last value for that minute. 
Only considered when creating a new datapoint. See the About the Push Metrics REST API section of this guide for more 
information on datapoint value aggregation intervals.

<b>Description(string):</b> Datapoint description. Only considered when creating a new datapoint.

<b>Type(string):</b> Metric type as a number in string format. Allowed options are “guage” (default) and “counter”. Only considered 
when creating a new datapoint.

<a name="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *https://AccountName.logicmonitor.com/rest*
