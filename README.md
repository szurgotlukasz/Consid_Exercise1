# Exercise §

The Process & instructions: Deadline: 27.02.2024 (Tuesday)
1. Please, use the coding best practices and patterns.
2. Please, implement unit-tests.
3. Please, describe each task separately:

- How much time it took for you to complete it (hours from start to finish)
- Some challenges/ideas/comments on each task;

Must use:
- Azure Function (Cloud/Local)
- Azure Storage (Cloud /Local storage emulator)
a. Table
b. Blob
- .Net Core 6
Achieve:
- Every minute, fetch data from https://api.publicapis.org/random?auth=null and store success/failure attempt log in the table and full payload in the blob.
- Create a GET API call to list all logs for the specific time period (from/to)
- Create a GET API call to fetch a payload from blob for the specific log entry
- Publish code on GitHub (public)

---

# Summary
This exercise took me about 12 hours to complete. 

## Challenges 
- Some of the time was spent on setting up azurite on my mac.
- There is no health check for the azure storage. I wanted to code a middleware and simply turn off the app if the connection fails or introduce a retry policy using Polly. It is easy in ASP.NET, but it wasn't easy in Function app as the runtime seems to manage the lifecycle slightly different than ASP.NET app.
- One of the differences that I've found is that you cannot access ILogger by building ServiceProvider in Startup.cs. In order to access logger, you have to wait for the app to start.

## Ideas/comments
- I would code integration tests to ensure that I am uploading logs/payloads in a correct configuration. For example I have decided to use partition "1". Neither of unit tests cover that decision.
- I would suggest to use 'Category' as a partition rather than "1", but since we have no control of the API we call, I have left the decision for later.
- CloudClient class checks whether the Blob container or table exists everytime the service is called. If this class grows, this might lead to code duplication. It could be refactored in a factory method in Startup.cs so the IoC container does these checks when injecting Blob/Table client.
- PublicApis class has hardcoded URL. Changing the url requires rebuild and redeploy. If the url might change frequently, then it would be better to move the url to configuration.

## How to run

Ensure the azurite is running. Never had to run it manually on Windows but in case you need it just type `azurite` in cmd or powershell. You should see the urls and ports in response of that command. If azurite is unrecognized(unlikely on Windows as it is part of VS), then type `npm i azurite`. If you don't have npm then install node from https://nodejs.org/en/download

Once azurite is started, you can run the app in Visual studio.

### Functions
The timer function will start automatically and fetch data every minute. The result of a running function
#### if the response from external API was unsuccessful:
- A table log has been uploaded. PartitionKey:1, RowKey: {string}.
#### if the response from external API was successful:
- A table log has been uploaded. PartitionKey:1, RowKey: {string}.
- A blob ({Guid}) has been uploaded.

once the data is added, you can fetch it using two endpoints:
- FetchPayload (requires blob Guid from the log above)
    - `curl 'http://localhost:7071/api/FetchPayloadFunction?id=<Guid>'`

- ListAllLogs (requires date from and to in ISO8601 format)
    - `curl 'http://localhost:7071/api/ListAllLogsFunction?from=2024-02-26T21:24:03&to=2024-02-29T21:25:01'`

The curl commands above might require a port change on your machine if port 7071 on your machine is taken.


