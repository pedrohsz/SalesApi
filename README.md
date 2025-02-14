# 🛒 Sales API - .NET 8

## 📌 Sobre o Projeto
A **Sales API** é um serviço RESTful para gerenciar registros de vendas, produtos e carrinhos de compras. Desenvolvida em **.NET 8**, a API segue os princípios do **DDD (Domain-Driven Design)** e **Clean Architecture**, garantindo separação de responsabilidades e modularidade.

O projeto inclui:
- **CRUD completo** para produtos e vendas.
- **Aplicação de regras de desconto** para compras por quantidade.
- **Cancelamento de vendas e itens específicos**.
- **Simulação de eventos** (`SaleCreated`, `SaleModified`, `SaleCancelled`, `ItemCancelled`).
- **Paginação, ordenação e filtros avançados**.
- **Testes unitários e de integração**.
- **Swagger para documentação automática**.

## 🚀 Tecnologias Utilizadas
- **Backend:** .NET 8, ASP.NET Core Web API
- **Banco de Dados:** PostgreSQL + Entity Framework Core
- **Containerização:** Docker + Docker Compose
- **Autenticação:** JWT (JSON Web Token)
- **Logging:** Serilog
- **Testes:** xUnit, Moq
- **Mapper:** AutoMapper
- **Validação:** FluentValidation
- **Documentação:** Swagger

## 📂 Estrutura do Projeto
```
📂 WebApplication1
 ├── 📂 src
 │    ├── 📂 WebApplication1 (Camada Web/API)
 │    │    ├── Controllers
 │    ├── 📂 Application (Camada de Aplicação)
 │    │    ├── Dtos
 │    │    ├── Mappings
 │    │    ├── Middlewares
 │    ├── 📂 Domain (Camada de Domínio)
 │    │    ├── Entities
 │    │    ├── Interfaces
 │    │    ├── Services
 │    │    ├── ValueObjects
 │    ├── 📂 Infrastructure (Infraestrutura e Persistência)
 │    │    ├── Data (DbContext)
 │    │    ├── Repositories
 │    │    ├── Migrations
 │    ├── 📂 Tests (Testes Unitários)
 ├── 📂 docker
 ├── 📄 docker-compose.yml
 ├── 📄 README.md
```

## 🔥 Como Rodar o Projeto
### 🔹 1. Configurar o Ambiente
Antes de rodar a API, certifique-se de ter instalado:
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- [.NET SDK 8.0](https://dotnet.microsoft.com/)

### 🔹 2. Subir os Containers (API + PostgreSQL)
```sh
docker-compose up -d
```
> Isso iniciará o banco de dados e a aplicação dentro de containers Docker.

### 🔹 3. Rodar as Migrations e Popular o Banco
```sh
dotnet ef database update
```

### 🔹 4. Acessar a API
Após subir os containers, a API estará disponível em:
- **Swagger UI**: [http://localhost:8080/swagger](http://localhost:8080/swagger)
- **Endpoint Base**: `http://localhost:8080/api`

### 🔹 5. Rodar Testes
Para rodar todos os testes:
```sh
dotnet test
```

## 📌 Endpoints Principais
### 🔹 **Produtos**
- `GET /products` - Listar produtos com paginação e ordenação
- `POST /products` - Criar um novo produto
- `GET /products/{id}` - Obter detalhes de um produto específico
- `DELETE /products/{id}` - Remover um produto

### 🔹 **Vendas**
- `POST /sales` - Criar uma venda aplicando regras de desconto
- `GET /sales` - Listar todas as vendas
- `GET /sales/{id}` - Obter detalhes de uma venda
- `DELETE /sales/{id}` - Cancelar uma venda inteira
- `DELETE /sales/{saleId}/items/{productId}` - Cancelar um item específico da venda

### 🔹 **Autenticação (JWT)**
- `POST /auth/login` - Gerar um token de acesso JWT
- `POST /auth/register` - Criar um novo usuário

## 🎯 Regras de Negócio
- ✅ Compras de **4 a 9 unidades** do mesmo produto recebem **10% de desconto**.
- ✅ Compras de **10 a 20 unidades** recebem **20% de desconto**.
- ✅ **Máximo de 20 unidades** por produto em uma única compra.
- ✅ Não há desconto para quantidades **abaixo de 4 unidades**.
- ✅ Cancelamento de **toda a venda** ou de **um único item**.
- ✅ Eventos simulados para ações da venda (criação, modificação, cancelamento).

## 📌 Testes e Cobertura
O projeto contém **testes unitários e de integração** para garantir confiabilidade.
- `dotnet test` → Executa todos os testes
- `xUnit` + `Moq` são utilizados para mocks

---
**🔹 Desenvolvido por Pedro Souza**

