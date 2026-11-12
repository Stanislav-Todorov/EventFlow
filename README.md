# EventFlow

EventFlow is an ASP.NET Core MVC web application designed for organizing events, managing venues and categories, and handling attendee registrations.

Users can browse events, search and filter them by various criteria, view event details, and register as attendees. The application also provides event management functionality, including creating, editing, and deleting events, along with tools for managing categories, venues, and registrations.

## Features

- Event management (CRUD — Create, Read, Update, Delete)
- Category management (Create and Read)
- Venue management (Create and Read)
- RSVP registration and removal
- Event capacity management and attendee counting
- Prevention of registration for past and fully booked events
- Event status indicators (Upcoming, Ongoing, Past, Full)
- Search and filtering of events by title, category, venue, and status
- Server-side and client-side validation using Data Annotations and ASP.NET Core MVC
- Event date validation
- Default sample data seeded automatically in the Development environment when the relevant database tables are empty
- Responsive user interface using Bootstrap and reusable Razor partial views

## Tech Stack

- ASP.NET Core MVC (.NET 8)
- SQL Server LocalDB
- Razor Views
- Bootstrap 5
- HTML, CSS

## Data Model

The application contains four main entities:

- **Category**
  - One Category can have many Events.

- **Venue**
  - One Venue can host many Events.
  - Contains venue name, address, and city.

- **Event**
  - Belongs to one Category.
  - Takes place at one Venue.
  - Has many RSVP registrations.
  - Contains event title, start and end times, and maximum attendee capacity.

- **Rsvp**
  - Belongs to one Event.
  - Contains guest name, email address, and registration date.

All entity primary keys use `Guid`. Relationships are represented through foreign keys and Entity Framework Core navigation properties.

## Getting Started

### Prerequisites

- Visual Studio with the ASP.NET and web development workload
- .NET 8 SDK
- SQL Server Express LocalDB

### Setup & Run

1. Clone the repository:

2. Open `EventFlow.sln` in Visual Studio.

3. Restore the NuGet packages if they are not restored automatically.

4. Open **Tools → NuGet Package Manager → Package Manager Console**.

5. Select the `EventFlow` project and apply the database migrations:

   ```powershell
   Update-Database
   ```

6. Ensure the application is running in the `Development` environment (it is set by default in `launchSettings.json`).

7. Run the application using Visual Studio.

### Database Configuration

EventFlow uses SQL Server LocalDB and the `DefaultConnection` connection string defined in `appsettings.json`.

The application uses Windows Authentication for the local database. No separate SQL Server username or password is required when using the default LocalDB configuration.

The database schema is created through Entity Framework Core migrations.

### Sample Data

The application includes a database initializer located in `Data/DbInitializer.cs`.

When the application starts in the `Development` environment, the initializer checks the Categories, Venues, Events, and RSVPs tables.

If all four tables are empty, it automatically inserts sample data, including:

- Event categories
- Venues
- Events with different dates and statuses
- RSVP registrations

The initializer preserves existing data and avoids duplicating the sample dataset on subsequent launches.

Event statuses depend on the current date and time, so the status of sample events may change over time.

## Design Decisions

- **Identifiers**
  - Entity primary keys use `Guid` rather than integer IDs.
  - Foreign keys also use `Guid` to maintain consistent relationships between entities.

- **Architecture**
  - The project follows the ASP.NET Core MVC architectural pattern.
  - Models represent application data, controllers handle requests and application operations, and Razor views display the interface.
  - Dependency Injection is used to provide `ApplicationDbContext` to controllers.

- **Data Access**
  - Entity Framework Core is used for database operations and schema management.
  - Related data is loaded using `Include()` where required.
  - Asynchronous database operations are used in the application controllers.

- **Database**
  - SQL Server LocalDB is used for local development and evaluation.
  - Database migrations are included in the repository.
  - Sample records are created through a custom database initializer.

- **Validation**
  - Data Annotations define field validation rules.
  - Custom server-side validation ensures that an event ends after it starts.
  - RSVP registration checks event availability and capacity.

- **User Interface**
  - Bootstrap provides a responsive layout.
  - Razor layouts and partial views are used to maintain a consistent interface and reuse UI components.

- **Authentication**
  - Authentication and role-based authorization are not implemented in this version.
  - The project focuses on ASP.NET Fundamentals concepts, including MVC, CRUD, validation, database relationships, and Dependency Injection.

## Project Purpose

EventFlow demonstrates the practical use of ASP.NET Core MVC, Entity Framework Core, SQL Server, dependency injection, relational data modeling, form validation, Razor views, and Git-based version control in a functional web application.