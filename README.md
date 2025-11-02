# 🎮 FIAP Cloud Games – Tech Challenge (Fase 1)

![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-green)
![JWT](https://img.shields.io/badge/Auth-JWT-orange)
![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow)

---

## 📌 Sobre o Projeto
Este projeto faz parte do **Tech Challenge – Pós Tech FIAP (.NET)**.  
Na **Fase 1**, o objetivo é criar uma **API REST em .NET 8** para gerenciar **usuários** e seus **jogos adquiridos**.  

A plataforma **FIAP Cloud Games (FCG)** será expandida nas próximas fases, mas nesta entrega focamos em:
- ✅ Cadastro de usuários (com validação de e-mail e senha forte).  
- ✅ Autenticação e autorização via **JWT** (Usuário x Administrador).  
- ✅ Biblioteca de jogos adquiridos.  
- ✅ Persistência com **Entity Framework Core**.  
- ✅ Documentação da API com **Swagger**.  
- ✅ Testes unitários aplicados às principais regras de negócio.  

---

## 🏗️ Arquitetura
- **Backend:** .NET 8 (Minimal API ou Controllers MVC)  
- **Banco de Dados:** SQL Server com Entity Framework Core  
- **Autenticação:** JWT  
- **Documentação:** Swagger  
- **Testes:** xUnit / MSTest / NUnit  

Opcionalmente:  
- MongoDB para persistência alternativa  
- Dapper para consultas de alta performance  
- GraphQL para consultas avançadas  

---

## 📂 Estrutura do Projeto

---

## 🐳 Instruções para o Docker Compose
Versão SQL utilizada:
- docker pull mcr.microsoft.com/mssql/server:2022-latest

Crie um arquivo ".env" na mesma pasta que o "docker-compose.yml" com as variáveis, defina os valores como preferir:
    ACCEPT_EULA=Y
    MSSQL_SA_PASSWORD=YourStrong!Passw0rd
    TZ=America/Sao_Paulo
     
Como subir o container:
- Navegue até a pasta onde está o "docker-compose.yml"
- Execute o comando: docker compose up -d
- Verifique se está rodando: docker ps

---