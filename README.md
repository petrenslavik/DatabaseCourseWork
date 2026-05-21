# ASP.NET SignalR + MongoDB Messenger

A real-time web messaging application built in 2019 as the
deliverable for a university database course. The brief was to
demonstrate working with a NoSQL store (MongoDB) in a non-trivial
application context.

## Architecture

- **Backend** (ASP.NET Core): Controllers (Accounts, Conversations,
  Messages, Users), Services (Conversation, Email, File, Message,
  User), custom Middleware, and a SignalR Hub (`MessengerHub`) for
  real-time message push (WebSocket transport, `[Authorize]`-
  protected).
- **Data layer**: MongoDB as the sole datastore, accessed through
  an EF-DbContext-style wrapper plus a generic `Repository<T>` over
  `IMongoCollection<T>`. The motivation was to make MongoDB access
  feel familiar to .NET developers used to Entity Framework
  patterns — trading off some of MongoDB's native idioms (aggregation
  pipelines, projections) for ergonomics.
- **Frontend** (Vue.js SPA) consuming the REST API + SignalR client.
- **File uploads** for attached images, **Email service** for account
  flows.

## What it demonstrates

- **Real-time push via SignalR** — `[Authorize]`-protected hub for
  authenticated WebSocket messaging
- **MongoDB integration in ASP.NET Core** — generic repository
  pattern over the MongoDB .NET driver
- **Layered ASP.NET Core architecture** — Controllers → Services →
  Repositories with proper separation of concerns
- **Vue.js SPA integration** with an ASP.NET Core backend

## Stack

- C# / ASP.NET Core
- SignalR (WebSocket transport)
- MongoDB driver
- Vue.js
- AutoMapper-style mapping layer

## Status

University coursework, no longer maintained. Kept as a portfolio
reference for real-time messaging via SignalR and MongoDB
integration in ASP.NET Core full-stack applications.