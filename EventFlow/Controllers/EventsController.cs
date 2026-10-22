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

        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Venue)
                .OrderBy(e => e.StartDateTime)
                .ToListAsync();

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
    }
}