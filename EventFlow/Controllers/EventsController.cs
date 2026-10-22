using EventFlow.Data;
using Microsoft.AspNetCore.Mvc;
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
    }
}