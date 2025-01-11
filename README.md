## Discount Codes Generator Service
.NET Core 8 project for generating unique discount codes, using gRPC for frontend-backend communication and Redis as a data store for the generated codes.

### Usage:
It is recommended to have Docker Desktop installed so that the project can be run in the most straightforward way. 
Thanks to the .NET Aspire orchestration tool, which handles setting up the containers, the project should be ready to use just after cloning the repository.
Simply use any IDE to build and start the Discount.AppHost project from the Solution. 
It should start a dashboard in the web browser, and a separate tab with the frontend app which allows making calls to the backend.
