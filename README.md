# Location Search Code Exercise

Solution for Location Search Code Exercise

The solution is developed as Web API with 2 endpoints:
- GET /Search?latitude=45&longitude=90&maxDistance=1000&maxResults=10
- POST /Search
```json
{
  "latitude": 45,
  "longitude": 90,
  "maxDistance": 1000,
  "maxResults": 10
}
```

Internaly both endpoints use exactly same service classes, i.e. returns same output.
When you run the solution, please open Swagger page (https://localhost:7144/swagger/) to see more details.

## Idea
To meet the main requirement and prevent the system to slow down if the number of locations is increasing, the application loads all locations from CSV into memory at startup and builds K-d Tree index for fast searches. This approach has been chosen for the following reasons:
- 1 million of locations can fit into memory since this is not a large data structure (only 3 fields). Even bigger number of locations can fit into memory, especially if the system has no strict hardware limitations. Having all locations in memory significantly improves search time, since we have no I/O latency
- Another assumption is that locations do not update frequently. If this assumption is wrong, the system could periodically update in-memory cache basing on timer or external signal/event/message (potential use for a message brocker). If keeping all locations in memory is not an option, location storage can be changed relatively easy without changing most of the system
- Code Exercise must be easy to build and run. Usage of external data storage like Elasticsearch or SQL database will increase the complexity to run the application

## Components
- **LocationSearch.WebApi** is ASP.NET Web API application which exposes interfaces for users and external systems
- **LocationSearch.Domain** contains domain model (data and behavior) which represents real-world objects and completely uncoupled of specific application logic
- **LocationSearch.Infrastructure** contains logic to communicate with infrustructure systems, like data storage, web services, etc. In this excercise, we have only CSV file as an external data storage
- **LocationSearch.Application** contains application logic to connect all other components together and peform the work

## Unit Tests
The solution doesn't have 100% unit test converage, but existing tests rather verify the correctness of most important pieces. Plus, they verify corner cases. You will find unit tests in projects with prefix UnitTests.

## Pefromance Tests
The solution also contains performance tests to ensure that the application will not slow down with the number of locations or number of users increased.

- **LocationSearch.Infrastructure.PerformanceTests** contains tests to ensure that location search execution time doesn't change significanly if the number of locations is increasing
- **LocationSearch.WebApi.PerformanceTests** contains stress test to measure and ensure that Web API can handle reasonable amout of concurrent users

## Requirements
- .NET 6
- Jetbrains Rider or Microsoft Visual Studio