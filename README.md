# TvMazeScraper
The solution consists of five projects:
- TvMazeScraper.Collector - a windows service that grabs data from TVmaze in background and keeps it in the local storage (MS SQL database). 
- MTvMazeScraper.WebApi - a web service that returns paged TV shows from the local storage (http://localhost:57035/api/shows?page=3).
- TvMazeScraper.Core - a library that encapsulate basic application logic
- TvMazeScraper.DataAccess - implementation of access to the local storage and to the TVmaze API.
- TvMazeScraper.Core.Tests

# Technologies
- Asp.Net Core for WebAppi
- Topshelf for windows service
- Entity Framework Core to access to MS SQL
- NLog for logging
- Autofac as a DI container in the windows service
- AutoMapper
- XUnit
- Moq