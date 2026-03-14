# AI Shop Bot 🛍️

An intelligent e-commerce orchestration engine built with **.NET 10**, leveraging Large Language Models (LLMs) to transform natural language prompts into actionable store operations.

## 🏗️ Architecture Overview

This project follows **Clean Architecture** principles and utilizes **CQRS** for handling AI-driven intents.



### Tech Stack
* **Backend:** .NET 10 Web API
* **AI Orchestration:** Microsoft Semantic Kernel
* **Real-time:** SignalR (for chat streaming)
* **Primary Database:** MSSQL (Identity, Orders, Cart consistency)
* **Vector Database:** MongoDB Atlas Vector Search (Semantic product discovery)
* **Caching:** Redis (Distributed session and chat history persistence)

---

## 🤖 AI Logic & Implementation

### 1. Intent Detection (Function Calling)
Rather than manual string parsing, the system uses **AI Tool Definition**. The LLM acts as a reasoning engine to map user prompts to specific C# methods.

* **The Process:** 1. User sends: "Find me a summer jacket and add it to my cart."
    2. The LLM identifies two intents: `ProductSearch` and `AddToCart`.
    3. The system extracts parameters (e.g., `category: "jacket"`, `season: "summer"`) and executes the corresponding handlers.

### 2. Semantic Search (RAG)
We utilize a **Retrieval-Augmented Generation (RAG)** pattern:
* **Embedding Model:** Converts product metadata into vectors.
* **Vector Search:** Performs a mathematical similarity check in MongoDB to find products that match the "vibe" of the user's prompt, not just keywords.



---

## 🛠️ Design Patterns Applied

| Pattern | Purpose |
| :--- | :--- |
| **Mediator (MediatR)** | Decouples the AI Orchestrator from the Domain logic. Each AI "Tool" triggers a Command or Query. |
| **Strategy Pattern** | Handles different business logic for various product types (Physical vs. Digital) during the checkout process. |
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
* **Commands:** Handle state changes like `CreateOrder` or `UpdateReview`.

---

## 📚 Learning Resources & Documentation

* [Microsoft Semantic Kernel Docs](https://learn.microsoft.com/en-us/semantic-kernel/) - AI Orchestration for .NET.
* [MongoDB Vector Search Tutorial](https://www.mongodb.com/docs/atlas/atlas-vector-search/tutorials/dotnet-tutorial/) - Setting up semantic search.
* [MediatR Documentation](https://github.com/jbogard/MediatR) - Implementing CQRS in .NET.
* [Dotnet Smart Components](https://github.com/dotnet/smartcomponents) - AI-powered UI components.