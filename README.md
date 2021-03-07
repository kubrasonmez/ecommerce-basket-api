# ecommerce-basket-api
I've implemented ecommerce-basket-api repository.

## Basket service which includes;
* ASP.NET Core Web API application 
* REST API principles, CRUD operations 
* **Redis** connection as a Database on docker
* Xunit Unit Test project.

## Installing
Follow these steps to get your development environment set up: (Before Run Start the Docker Desktop)
1. Clone the repository
2. At the root directory which include **docker-compose.yml** files, run below command:
```csharp
docker-compose -f docker-compose.yml -f docker-compose.override.yml up –d
```
3. Wait for docker compose all services.

4. You can launch 
* **Basket API -> http://localhost:8000/swagger/index.html**

## Basket Api Endpoints

I've designed Basket Api according to REST perspective. Basket Api have find 4 endpoints as below :

* Get Basket and Items with username => /api/v1/Basket GET
* Update Basket and Items (add — remove item on basket) => /api/v1/Basket POST
* Delete Basket => /api/v1/Basket DELETE
* Control Products Stock Status => /api/v1/Basket/ControlProductStock POST






