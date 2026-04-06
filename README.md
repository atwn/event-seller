## Yandex Practicum. C#/.NET Middle. Course Project.

### Instructions:
```bash
dotnet run --project .\src\Events.Api\Events.Api.csproj
```
open http://localhost:5111/swagger/index.html in the browser  

### Endpoints:
- `GET /api/events` - возвращает полный список событий  
  - `200` - все события найдены, список событий в теле ответа  
- `GET /api/events/{id}` - возвращает событие под номером `{id}`  
  - `200` - событие найдено, детали события в теле ответа  
  - `404` - событие под номером `{id}` не найдено  
- `POST /api/events` - создаёт новое событие и возвращает ссылку на него в заголовке `Location`  
  - `201` - событие создано  
  - `400` - детали события в теле запроса заданы неверно  
- `PUT /api/events/{id}` - обновляет детали события под номером `{id}`  
  - `204` - детали события были успешно сохранены  
  - `400` - детали события заданы неверно  
  - `404` - событие под номером `{id}` не найдено  
- `DELETE /api/events/{id}` - удаляет событие под номером `{id}`  
  - `204` - событие было удалено успешно  
  - `404` - событие под номером `{id}` не найдено  

### Prerequisites:
.NET 10 SDK

### This project was created with the following steps:
```bash
dotnet new webapi -n Events.Api -f net10.0 --use-controllers --no-openapi --no-https

dotnet new solution -n Events
dotnet solution .\Events.slnx add .\Events.Api\Events.Api.csproj

dotnet add .\Events.Api\Events.Api.csproj package Swashbuckle.AspNetCore
```
