## Yandex Practicum. C#/.NET Middle. Course Project.

### Instructions:

#### To explore the API with Swagger UI:
```bash
dotnet run --project .\src\Events.Api\Events.Api.csproj
```
then open http://localhost:5111/swagger/index.html in the browser  

#### To run tests:
```
dotnet test .\src\EventService.Tests\EventService.Tests.csproj
```

### Endpoints:
- `GET /events?title={title}&from={from}&to={to}&page={page}&pageSize={pageSize}` - возвращает полный список событий с фильтрацией по названию и дате, и возможностью пагинации  
  - `200` - все события найдены, список событий в теле ответа  
  - `400` - параметры фильтрации заданы неверно  
- `GET /events/{id}` - возвращает событие под номером `{id}`  
  - `200` - событие найдено, детали события в теле ответа  
  - `404` - событие под номером `{id}` не найдено  
- `POST /events` - создаёт новое событие и возвращает ссылку на него в заголовке `Location`  
  - `201` - событие создано  
  - `400` - детали события в теле запроса заданы неверно  
- `PUT /events/{id}` - обновляет детали события под номером `{id}`  
  - `204` - детали события были успешно сохранены  
  - `400` - детали события заданы неверно  
  - `404` - событие под номером `{id}` не найдено  
- `DELETE /events/{id}` - удаляет событие под номером `{id}`  
  - `204` - событие было удалено успешно  
  - `404` - событие под номером `{id}` не найдено  

### Exceptions:
- `BadRequestDto` - ошибка валидации (формируется валидатором контроллера)  
- `ProblemDetails` - остальные ошибки (формируется в глобальном обработчике ошибок)  

### Prerequisites:
.NET 10 SDK

### TODOs:
- [ ] (maybe) throw exceptions from Validators instead of returning BadRequest to let exception handing middleware convert them into ProblemDetails
- [ ] move seeding of test data from Program.cs to a helper class

### Scaffolding:
```bash
dotnet new webapi -n Events.Api -f net10.0 --use-controllers --no-openapi --no-https

dotnet new solution -n Events
dotnet solution .\Events.slnx add .\Events.Api\Events.Api.csproj

dotnet add .\Events.Api\Events.Api.csproj package Swashbuckle.AspNetCore

dotnet new xunit -o .\src\EventService.Tests\
dotnet solution .\Events.slnx add .\EventService.Tests\EventService.Tests.csproj
```
