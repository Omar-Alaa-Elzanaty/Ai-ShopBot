# AI Shop Bot 🛍️

An intelligent e-commerce orchestration engine built with **.NET 10**, leveraging Large Language Models (LLMs) to transform natural language prompts into actionable store operations.

## 🏗️ Architecture Overview

This project follows **Clean Architecture** principles and utilizes **CQRS** for handling AI-driven intents.



### Tech Stack
* **Backend:** .NET 10 Web API
* **AI Orchestration:** Microsoft Semantic Kernel (SK Plugins) and Microsot.AI.Extension
* **gpt-4.1-mini** Model for chat completion.
* **text-embedding-3-small** for text embedding.
* **Real-time:** SignalR (for chat streaming)
* **Primary Database:** MSSQL (Identity, Orders, Cart consistency)
* **Vector Database:** MongoDB Atlas Vector Search (Semantic product discovery)
* **Caching:** Redis (Distributed session and chat history persistence)

---

## 🤖 AI Logic & Implementation

### 1. Intent Detection (Function Calling)
Rather than manual string parsing, the system uses **AI Tool Definition**. The LLM acts as a reasoning engine to map user prompts to specific C# methods.

* **The Process:**
    1. User sends: "Find me a summer jacket and add it to my cart."
    2. The LLM identify needed desired function: `ProductSearch`.
    3. The system using embedding generate array of 1024 float number and executes vector search for the corresponding handlers.

### 2. Semantic Search (RAG)
We utilize a **Retrieval-Augmented Generation (RAG)** pattern:
* **Embedding Model:** Converts product metadata into vectors.
* **Vector Search:** Performs a mathematical similarity check in MongoDB to find products that match the "vibe" of the user's prompt, not just keywords.



---

## 🛠️ Design Patterns Applied

| Pattern | Purpose |
| :--- | :--- |
| **Mediator (MediatR)** | Decouples the AI Orchestrator from the Domain logic. Each AI "Tool" triggers a Command or Query. |
| **Plugin Architecture** | Uses Semantic Kernel's plugin system to encapsulate specialized AI capabilities. |
| **Repository Pattern** | Abstracts data access for MSSQL and MongoDB to maintain a clean Domain layer. |

---

## 🚀 Key Challenges & Solutions

### How to detect the required method?
We use **Semantic Kernel Plugins**. By decorating our Application Layer services with `[KernelFunction]` attributes, the LLM receives a schema of our API and knows exactly which parameters to provide.

### Managing Conversation Context
To ensure the AI remembers that "it" refers to the jacket mentioned two messages ago, we store the **ChatId** in **Redis**. This context is injected into the LLM prompt window on every request.

### Implementation form: CQRS vs. Services
To avoid "Fat Services," every AI action is implemented as a standalone **Command** or **Query**.
* **Queries:** Handle Vector Search and product fetching.
* **Commands:** Handle state changes like `CreateOrder` or `GetUserOrderById`.

---

## 🛠️ Tech Stack

* **Runtime:** .NET 10 (ASP.NET Core)
* **Architecture:** Clean Architecture
* **SQL Database:** Entity Framework Core (MSSQL)
* **NOSQL Database** MongoDb
* **Concurrency Control:** Redis (using `Redlock.net`)
* **DevOps:** Docker

---

## 🚦 Getting Started

### 1. Prerequisites
* .NET 10 SDK
* Docker Desktop

### 2. Setup Infrastructure
Spin up Redis instances:
```bash
docker run -d --name redis-stack -p 6379:6379 -p 8001:8001 redis/redis-stack:latest
```

### 3. Get API key for LLM models
> **Your Github profile** > **Settings** > **Developer settings** > **Personal Access tokens** > **Fine-grined tokens**
> **Generate new token** > **Fill required data** > **Add Permissions** > **Models**
then past as a value for key **AzureOpenAI:ApiKey**
