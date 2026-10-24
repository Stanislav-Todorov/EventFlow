using EventFlow.Data;
using EventFlow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? searchString,
            Guid? categoryId,
            Guid? venueId,
            string? eventStatus)
        {
            var eventsQuery = _context.Events
                .Include(e => e.Category)
                .Include(e => e.Venue)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                eventsQuery = eventsQuery
                    .Where(e => e.Title.Contains(searchString));
            }

            if (categoryId.HasValue)
            {
                eventsQuery = eventsQuery
                    .Where(e => e.CategoryId == categoryId.Value);
            }

            if (venueId.HasValue)
            {
                eventsQuery = eventsQuery
                    .Where(e => e.VenueId == venueId.Value);
            }

            if (eventStatus == "upcoming")
            {
                eventsQuery = eventsQuery
                    .Where(e => e.StartDateTime >= DateTime.Now);
            }
            else if (eventStatus == "ongoing")
            {
                eventsQuery = eventsQuery
                    .Where(e =>
                        e.StartDateTime <= DateTime.Now &&
                        e.EndDateTime >= DateTime.Now);
            }
            else if (eventStatus == "past")
            {
                eventsQuery = eventsQuery
                    .Where(e => e.EndDateTime < DateTime.Now);
            }

            var events = await eventsQuery
                .OrderBy(e => e.StartDateTime)
                .ToListAsync();

            ViewBag.SearchString = searchString;

            ViewBag.Categories = new SelectList(
                _context.Categories.OrderBy(c => c.Name),
                "Id",
                "Name",
                categoryId);

            ViewBag.Venues = new SelectList(
                _context.Venues.OrderBy(v => v.Name),
                "Id",
                "Name",
                venueId);

            ViewBag.EventStatus = eventStatus;

            return View(events);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(
                _context.Categories.OrderBy(c => c.Name),
                "Id",
                "Name");

            ViewBag.Venues = new SelectList(
                _context.Venues.OrderBy(v => v.Name),
                "Id",
                "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event eventItem)
        {
            if (eventItem.EndDateTime <= eventItem.StartDateTime)
            {
                ModelState.AddModelError(
                    nameof(eventItem.EndDateTime),
                    "End date and time must be later than the start date and time.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(
                    _context.Categories.OrderBy(c => c.Name),
                    "Id",
                    "Name",
                    eventItem.CategoryId);

                ViewBag.Venues = new SelectList(
                    _context.Venues.OrderBy(v => v.Name),
                    "Id",
                    "Name",
                    eventItem.VenueId);

                return View(eventItem);
            }

            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(
                _context.Categories.OrderBy(c => c.Name),
                "Id",
                "Name",
                eventItem.CategoryId);

            ViewBag.Venues = new SelectList(
                _context.Venues.OrderBy(v => v.Name),
                "Id",
                "Name",
                eventItem.VenueId);

            return View(eventItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Event eventItem)
        {
            if (id != eventItem.Id)
            {
                return NotFound();
            }

            if (eventItem.EndDateTime <= eventItem.StartDateTime)
            {
                ModelState.AddModelError(
                    nameof(eventItem.EndDateTime),
                    "End date and time must be later than the start date and time.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(
                    _context.Categories.OrderBy(c => c.Name),
                    "Id",
                    "Name",
                    eventItem.CategoryId);

                ViewBag.Venues = new SelectList(
                    _context.Venues.OrderBy(v => v.Name),
                    "Id",
                    "Name",
                    eventItem.VenueId);

                return View(eventItem);
            }

            try
            {
                _context.Events.Update(eventItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _context.Events.AnyAsync(e => e.Id == eventItem.Id);

                if (!exists)
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}