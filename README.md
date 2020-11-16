# Funda Back-end Challenge
## Description
This application in the shape of a RPC-style Web API is intended to retrieve the information about properties for sale from the Funda Partner API and show the top 10 real estate agents with the most properties for sale given certain criteria.

The API currently has 3 end-points:
- GetTopAgent: Returns the Agent with the most properties for sale.
- GetAgentsLeaderboard: Returns the top 10 agents with the most properties for sale.
- GetAgentsLeaderboardForPropertiesWithGarden: Returns the top 10 agents with the most properties for sale that have a garden.

## Implementation details
The application has 4 defined layers in order to segregate responsibilities:
- Api: Self descriptive, it implements Swagger as a way to show a visual interface.
- Application: Where services are implemented. Contains interfaces to interact with the Infrastructure layer and DTOs (or ViewModels).
- Domain: Really simple in this case, contains the domain models and would contain any related business logic.
- Infrastructure: Contains classes to interact with the storage (HttpClient) and their specific models.

The application does not contain a test suite, but it is fully testeable.
Responsibilities are segregated and dependencies are injected through the built-in DI container.

## Exception handling
Exceptions are first logged and then handled in a centralized way from the API layer making use of the GlobalExceptionHandler (registered at startup). A friendly message is displayed to the user in order to obfuscate details about the exception.

## Challenges
- The application had to retrieve the whole list of records before it could create a leaderboard.
- Funda Partner API partner only retrieves a maximum of 25 records per page, this required the application to make more than 100 calls to retrieve the full listing of properties to then generate a leaderboard of the top 10 real estate agents.
- Funda Partner API has a limit of 100 per minute, returning a 401 (Unauthorized) status code when the limit is exceeded. This required me to implement a middleware that would make sure that scenario doesn't occur.

## Request Rate Limit handling
The request rate limit is handled in two different places to cover two different possible scenarios.

### Scenario 1: This is the only application using the provided API key.
The application implements an HttpMessageHandler that intercepts every call that goes through the HttpClient injected into the typed client FundaListingHttpClient.
This approach allowed me to take the last 100 calls, calculate the time elapsed between the first and the last record, and then add the remaining waiting time (in milliseconds), so the number of calls never exceed the limit.

### Scenario 2: The API key is shared across applications.
In case the API key is shared across applications, the application implements an extra check into the FundaListingHttpClient.
This extra check consists of a retry mechanism that is executed when a 401 (Unauthorized) status code is returned, and in case that occurs, it will wait 5 seconds and retry the operations.
Due to the limited time for completing this assignment I didn't consider implementing an exit condition (what would happen if the API keeps returning 401 (Unauthorized) for hours?) but I took it into account.

All settings including API URL, API Key and request rate limitations are configurable from the appsettings.json file into the Api project.
