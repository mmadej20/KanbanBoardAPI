# KanbanBoard API
[![CodeFactor](https://www.codefactor.io/repository/github/mmadej20/kanbanboardapi/badge)](https://www.codefactor.io/repository/github/mmadej20/kanbanboardapi)

KanbanBoard API is a simple and extensible API for managing Kanban boards, tasks, and members. 
It is built using the CQRS pattern and .NET 9, providing a clean architecture for scalable development.

## Features

- Create, update, and delete Kanban boards
- Add, update, and remove tasks (ToDos) with status management
- Assign and manage board members
- Board-member relationships with ownership and join date
- Task assignment to members
- CQRS pattern for clear separation of commands and queries
- Entity Framework Core for data access
- Integration tests for core functionality

## Technologies

- .NET 9
- C# 13
- Entity Framework Core
- MediatR (CQRS)
- SQL Server (LocalDB for development/testing)
- TUnit for testing

## Getting Started

1. **Clone the repository**
2. **Setup the database**
   - The solution uses LocalDB for development and testing.
   - Update the connection string in `appsettings.json` if needed.

3. **Run migrations**
4. **Build and run the API**
## Project Structure

- `KanbanBoard.Api` - API entry point
- `KanbanBoard.Application` - Application logic, CQRS commands/queries, services
- `KanbanBoard.DataAccess` - Entity Framework Core models, migrations
- `KanbanBoard.Domain` - Domain models and enums
- `KanbanBoard.Infrastructure` - Service implementations
- `KanbanBoard.IntegrationTests` - Integration tests

## API Endpoints

- Boards: Create, update, delete, get by ID
- Tasks: Add, update status, assign member, get all
- Members: Add, update, delete, assign to board

