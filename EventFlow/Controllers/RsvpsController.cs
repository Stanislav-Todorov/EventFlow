using EventFlow.Data;
using EventFlow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Controllers
{
    public class RsvpsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RsvpsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid eventId)
        {
            var eventItem = await _context.Events
                .Include(e => e.Rsvps)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventItem == null)
            {
                return NotFound();
            }

            if (eventItem.EndDateTime < DateTime.Now)
            {
                TempData["ErrorMessage"] = "You cannot register for an event that has already ended.";

                return RedirectToAction(
                    "Details",
                    "Events",
                    new { id = eventId });
            }

            if (eventItem.Rsvps.Count >= eventItem.MaxAttendees)
            {
                TempData["ErrorMessage"] = "This event has reached its maximum number of attendees.";

                return RedirectToAction(
                    "Details",
                    "Events",
                    new { id = eventId });
            }

            var rsvp = new Rsvp
            {
                EventId = eventId
            };

            return View(rsvp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rsvp rsvp)
        {
            var eventItem = await _context.Events
                .Include(e => e.Rsvps)
                .FirstOrDefaultAsync(e => e.Id == rsvp.EventId);

            if (eventItem == null)
            {
                return NotFound();
            }

            if (eventItem.EndDateTime < DateTime.Now)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "You cannot register for an event that has already ended.");
            }

            if (eventItem.Rsvps.Count >= eventItem.MaxAttendees)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "This event has reached its maximum number of attendees.");
            }

            var emailAlreadyRegistered = await _context.Rsvps
                .AnyAsync(r =>
                    r.EventId == rsvp.EventId &&
                    r.GuestEmail == rsvp.GuestEmail);

            if (emailAlreadyRegistered)
            {
                ModelState.AddModelError(
                    nameof(rsvp.GuestEmail),
                    "This email is already registered for this event.");
            }

            if (!ModelState.IsValid)
            {
                return View(rsvp);
            }

            rsvp.RegisteredOn = DateTime.Now;

            _context.Rsvps.Add(rsvp);
            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                "Events",
                new { id = rsvp.EventId });
        }
    }
}