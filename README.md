In order to keep backward compatibility, the solution is not migrated to .NET Core nor any of the modern .NET versions.
Even thought, the legacy .NET framework is upgraded to 4.8.1 because Microsoft recommends customers upgrade to .NET Framework 4.8.x to receive the highest level of performance, reliability, and security.

MSTest project is replaced by xUnit framework.

Imperial.CommercialRoutes.CorssCutting project is removed.

# Naming convention referencing the following doc: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names

# Techonoly stack
- Dapper ORM
- Serilog
- RestSharp REST API client
- AutoMapper
- FluentValidation
- Autofac
- Swashbuckle Swagger/OpenAPI
- xUnit
- FluentAssertions
- Moq

# URI Endpoints
- [GET]  api/routes/                  [Gets all routes]
- [POST] api/routes/pricebreakdown    [Gets route price break down]
- [POST] api/routes/optimalaircraft   [Gets opmital aircraft for a given route]

Endpoint "api/routes/pricebreakdown" is designed to received origin, destination planets and the day of the week (optional value from 0-6). If the day is not specified; current day of week is assumed.

In order to improve the user experience of the empire, I integrated the solution with swagger and make the startup to launch: https://localhost:44366/swagger/ui/index.
I also implemented a 'GlobalExceptionHandler' to capture unhandled exceptions raised from outside the action, such as: error inside the exception filter, exception related to routing, exception in the controller constructor, etc.
I added a 'GlobalExceptionFilter' to capture unhandled exceptions in Web API.
And 'BadRequestException' to build BadRequest exceptions.

The current application handles a short dataset of data. Since millions of data could exist; none of the solutions that fetch data from the database nor from the web services are scalable.
The endpoint that fetches all the existing distances, does not scale; hence my proposal would have been not to return all the data at once; either using pagination or removing the API endpoint.
The current mocking services return a short json content; ideally these services would have allowed filtering using OData for instance.
Taking these facts into consideration I implemented/simulated filters that happens after fetching data from the web services.

For the calculation of the price breakdown, the problem does not explain how to calculate the price per lunar day. Origin and destination planets can belong to different sectors with different values; so I assumed that
pricePerLunarDay = PricePerLunarDay of origin planet + PricePerLunarDay of destination planet if they are different.
Another solution is to keep the highest value.

To calculate the optimal aircraft, I assumed that the 'MaxDistance' is given as lunar years.
Instad of returning the reference of the optimal aircraft, I built a new response object to display more information about the optimal aircraft. In case there is no aircraft that suits the route the response includes a message indicating there is no aircraft found for the given route.

The breakdown price response is converted to decimal using the expression 'Convert.ToDecimal(value, "0:00")'. It rounds the values and use the string format specified. This solution is used because using 'Math.Round(value, 2)' did not allow displaying data as the example from the README when the value is zero '0.00'. Currently I used that solution; although a better one would have been to implement a decimal JsonConverter to apply the decimal format "0:00". I didn not use it because the response looks unreadable for the sake of the examn.

Assuming the datasets could be very huge I assumed the following:
Every time a planet/distance is requested, the corresponding services look for the enity in the database first; if do not exist, search for them from external sources and save them into the local database; instead of getting all of them to avoid memory ran outs.
As a complement of this solution I would have implemented WebHooks which subscribed to any create/update/delete planet/distance/etc; allowing to create missing data when needed or updating/deleting existing ones.

I implemented only some unit tests. All the code can be unit tested and will require more effort to cover all the scenarios to be well tested.