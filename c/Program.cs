using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

// Function App 1: CalledFunction
public static void CalledFunction(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req,
    FunctionContext executionContext)
{
    var logger = executionContext.GetLogger("CalledFunction");
    logger.LogInformation("C# HTTP trigger function processed a request.");

    // Return a response
    var response = req.CreateResponse(HttpStatusCode.OK);
    response.WriteString("Hello from CalledFunction!");
}

// Function App 2: CallerFunction
public static void CallerFunction(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req,
    FunctionContext executionContext)
{
    var logger = executionContext.GetLogger("CallerFunction");
    logger.LogInformation("C# HTTP trigger function processed a request.");

    
    var httpClient = new HttpClient();
    var response = httpClient.GetAsync("https://calledfunction.azurewebsites.net/api/CalledFunction").Result;
    response.EnsureSuccessStatusCode();

    
    var responseBody = response.Content.ReadAsStringAsync().Result;
    logger.LogInformation($"Response from CalledFunction: {responseBody}");

    // Return a response
    var callerResponse = req.CreateResponse(HttpStatusCode.OK);
    callerResponse.WriteString($"Hello from CallerFunction! Response from CalledFunction: {responseBody}");
}