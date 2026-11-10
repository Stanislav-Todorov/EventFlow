using EventFlow.Models;

namespace EventFlow.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext db)
        {
            if (db.Categories.Any() ||
                db.Venues.Any() ||
                db.Events.Any() ||
                db.Rsvps.Any())
            {
                return;
            }

            var now = DateTime.Now;

            var workshop = new Category { Name = "Workshop" };
            var conference = new Category { Name = "Conference" };
            var networking = new Category { Name = "Networking" };
            var community = new Category { Name = "Community" };
            var entertainment = new Category { Name = "Entertainment" };

            db.Categories.AddRange(
                workshop,
                conference,
                networking,
                community,
                entertainment
            );

            var techHall = new Venue
            {
                Name = "Sofia Tech Hall",
                Address = "Example Street 15",
                City = "Sofia"
            };

            var conferenceCenter = new Venue
            {
                Name = "Varna Conference Center",
                Address = "Harbor Avenue 8",
                City = "Varna"
            };

            var cultureHub = new Venue
            {
                Name = "City Culture Hub",
                Address = "Central Street 22",
                City = "Plovdiv"
            };

            var innovationHub = new Venue
            {
                Name = "Innovation Hub",
                Address = "Technology Boulevard 10",
                City = "Sofia"
            };

            db.Venues.AddRange(
                techHall,
                conferenceCenter,
                cultureHub,
                innovationHub
            );

            var event1 = new Event
            {
                Title = "ASP.NET Core Workshop",
                StartDateTime = now.AddDays(7),
                EndDateTime = now.AddDays(7).AddHours(3),
                MaxAttendees = 30,
                Category = workshop,
                Venue = techHall
            };

            var event2 = new Event
            {
                Title = "Technology Conference",
                StartDateTime = now.AddDays(14),
                EndDateTime = now.AddDays(14).AddHours(6),
                MaxAttendees = 100,
                Category = conference,
                Venue = conferenceCenter
            };

            var event3 = new Event
            {
                Title = "Developer Networking Evening",
                StartDateTime = now.AddHours(-1),
                EndDateTime = now.AddHours(3),
                MaxAttendees = 25,
                Category = networking,
                Venue = innovationHub
            };

            var event4 = new Event
            {
                Title = "Community Coding Meetup",
                StartDateTime = now.AddDays(3),
                EndDateTime = now.AddDays(3).AddHours(2),
                MaxAttendees = 2,
                Category = community,
                Venue = cultureHub
            };

            var event5 = new Event
            {
                Title = "Music and Arts Evening",
                StartDateTime = now.AddDays(20),
                EndDateTime = now.AddDays(20).AddHours(4),
                MaxAttendees = 80,
                Category = entertainment,
                Venue = cultureHub
            };

            var event6 = new Event
            {
                Title = "Introduction to SQL Server",
                StartDateTime = now.AddDays(-5),
                EndDateTime = now.AddDays(-5).AddHours(3),
                MaxAttendees = 20,
                Category = workshop,
                Venue = techHall
            };

            var event7 = new Event
            {
                Title = "Open Source Community Day",
                StartDateTime = now.AddDays(-12),
                EndDateTime = now.AddDays(-12).AddHours(5),
                MaxAttendees = 40,
                Category = community,
                Venue = conferenceCenter
            };

            var event8 = new Event
            {
                Title = "Software Architecture Workshop",
                StartDateTime = now.AddDays(10),
                EndDateTime = now.AddDays(10).AddHours(3),
                MaxAttendees = 15,
                Category = workshop,
                Venue = innovationHub
            };

            db.Events.AddRange(
                event1,
                event2,
                event3,
                event4,
                event5,
                event6,
                event7,
                event8
            );

            Rsvp Register(Event eventItem, string name, string email)
            {
                return new Rsvp
                {
                    GuestName = name,
                    GuestEmail = email,
                    RegisteredOn = now,
                    Event = eventItem
                };
            }

            db.Rsvps.AddRange(
                Register(event1, "Alex Morgan", "alex@example.com"),
                Register(event1, "Emma Wilson", "emma@example.com"),

                Register(event2, "Daniel Carter", "daniel@example.com"),
                Register(event2, "Olivia Brown", "olivia@example.com"),

                Register(event3, "James Taylor", "james@example.com"),

                Register(event4, "Sophia Davis", "sophia@example.com"),
                Register(event4, "Noah Miller", "noah@example.com"),

                Register(event6, "Liam Anderson", "liam@example.com"),

                Register(event7, "Mia Thomas", "mia@example.com"),
                Register(event7, "Lucas Martin", "lucas@example.com"),

                Register(event8, "Charlotte White", "charlotte@example.com"),
                Register(event8, "Ethan Harris", "ethan@example.com")
            );

            db.SaveChanges();
        }
    }
}
