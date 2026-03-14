# 👋 Hey, I'm [Your Name]

> **Mid-Level .NET Developer** — building AI-powered backends, clean architectures, and real-time systems.

---

## 🚀 Currently Building

**AI Shop Bot** — A conversational commerce backend where users interact with a store entirely through natural language chat.

```
"I need a summer jacket around 200 EGP"
        ↓
  Semantic Kernel detects intent
        ↓
  SearchProducts plugin fires
        ↓
  MongoDB Vector Search returns matches
        ↓
  GPT-4o formats a natural response
```

> Stack: `.NET 10` · `Semantic Kernel` · `MongoDB Atlas Vector Search` · `SignalR` · `MSSQL` · `Redis` · `ASP.NET Identity`

---

## 🧠 Tech Stack

### Backend
![.NET](https://img.shields.io/badge/.NET_10-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![SignalR](https://img.shields.io/badge/SignalR-512BD4?style=flat-square&logo=dotnet&logoColor=white)

### AI & Search
![Semantic Kernel](https://img.shields.io/badge/Semantic_Kernel-0078D4?style=flat-square&logo=microsoft&logoColor=white)
![OpenAI](https://img.shields.io/badge/OpenAI-412991?style=flat-square&logo=openai&logoColor=white)
![MongoDB](https://img.shields.io/badge/MongoDB_Atlas-47A248?style=flat-square&logo=mongodb&logoColor=white)

### Data & Infrastructure
![MSSQL](https://img.shields.io/badge/MSSQL-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=flat-square&logo=redis&logoColor=white)
![Entity Framework](https://img.shields.io/badge/EF_Core-512BD4?style=flat-square&logo=dotnet&logoColor=white)

---

## 🏗️ Architecture Principles

```
📦 Solution
 ┣ 🔵 Domain          → Entities, Value Objects, Repository Interfaces
 ┣ 🟢 Application     → CQRS Handlers (MediatR), SK Plugins, DTOs
 ┣ 🟠 Infrastructure  → EF Core, MongoDB, Redis, OpenAI clients
 ┗ 🔴 API             → SignalR Hub, Controllers, DI setup
```

- **Clean Architecture** — strict layer boundaries, dependencies point inward
- **CQRS + MediatR** — every operation is a Command or Query, SK plugins are thin dispatchers
- **SK Plugin Pattern** — LLM routes to methods via Auto Function Invocation, no manual intent switch
- **RAG Search** — `text-embedding-3-small` embeds products at ingestion; `$vectorSearch` pipeline at query time

---

## 💡 What I'm Learning on This Project

| Topic | Tool | Purpose |
|---|---|---|
| LLM Function Calling | Semantic Kernel | Auto intent detection & method routing |
| Vector Embeddings | OpenAI + MongoDB Atlas | Semantic product search (RAG) |
| Real-time Streaming | SignalR | Token-by-token chat responses |
| Conversation Memory | Redis | Per-session chat history with TTL |
| Transactional Data | MSSQL + EF Core | Orders, cart, auth — ACID consistency |

---

## 📫 Connect

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0A66C2?style=flat-square&logo=linkedin&logoColor=white)](https://linkedin.com/in/yourprofile)
[![GitHub](https://img.shields.io/badge/GitHub-181717?style=flat-square&logo=github&logoColor=white)](https://github.com/yourusername)
