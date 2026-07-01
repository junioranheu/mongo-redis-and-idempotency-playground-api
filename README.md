# Mongo, Redis & Idempotency Playground

Projeto desenvolvido em .NET para estudo de MongoDB, Redis, DDD, Repository Pattern e Idempotência.

## Funcionalidades

* CRUD com MongoDB
* Cache distribuído com Redis
* Repository Pattern
* Cache Aside Pattern
* Idempotência em operações de escrita
* Relacionamentos entre entidades no MongoDB
* Documentos embutidos

## Tecnologias

* .NET 10
* ASP.NET Core
* MongoDB
* Redis
* StackExchange.Redis

## Arquitetura

```text
Controllers
    ↓
Application Services
    ↓
Repository Interfaces
    ↑
Repository Implementations
    ↓
MongoDB / Redis
```

## Objetivo

Praticar:

* Modelagem de documentos no MongoDB
* Relacionamentos em bancos NoSQL
* Cache distribuído
* Idempotência em APIs REST
* Repository Pattern
* Conceitos de Clean Architecture"# mongo-redis-and-idempotency-playground-api" 
