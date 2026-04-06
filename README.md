## Yandex Practicum. C#/.NET Middle. Course Project.

### This project was created with the following steps:
```bash
dotnet new webapi -n Events.Api -f net10.0 --use-controllers --no-openapi --no-https

dotnet new solution -n Events
dotnet solution .\Events.slnx add .\Events.Api\Events.Api.csproj

dotnet add .\Events.Api\Events.Api.csproj package Swashbuckle.AspNetCore
```
