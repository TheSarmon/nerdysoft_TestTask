using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nerdysoft_TestTask.Data;
using nerdysoft_TestTask.Models;

namespace nerdysoft_TestTask.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly AnnouncementContext _context;

        public AnnouncementsController(AnnouncementContext context)
        {
            _context = context;
        }

        [Route("announcements")]
        [Route("announcements/index")]
        public async Task<IActionResult> Index()
        {
            var announcements = await _context.Announcements.ToListAsync();
            return View(announcements);
        }

        [HttpGet]
        [Route("announcements/create")]
        [Route("announcements/new")]
        public IActionResult Create()
        {
            return View(new AnnouncementModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("announcements/create")]
        [Route("announcements/new")]
        public async Task<IActionResult> Create(AnnouncementModel announcement)
        {
            if (ModelState.IsValid)
            {
                announcement.DateAdded = DateTime.Now;
                _context.Announcements.Add(announcement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(announcement);
        }

        [HttpGet]
        [Route("announcements/edit/{id:int}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }
            return View(announcement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("announcements/edit/{id:int}")]
        public async Task<IActionResult> Edit(AnnouncementModel announcement)
        {
            if (!ModelState.IsValid)
            {
                return View(announcement);
            }

            try
            {
                _context.Update(announcement);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Announcements.Any(e => e.Id == announcement.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("announcements/details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            var similarAnnouncements = await GetSimilarAnnouncements(announcement);
            ViewBag.SimilarAnnouncements = similarAnnouncements;

            return View(announcement);
        }

        [Route("announcements/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<AnnouncementModel>> GetSimilarAnnouncements(AnnouncementModel announcement)
        {
            var allAnnouncements = await _context.Announcements
                .Where(a => a.Id != announcement.Id)
                .ToListAsync();

            var stopWords = new List<string>
            {
                "the", "is", "in", "at", "of", "and", "to", "on",
                "for", "new", "your", "from", "get", "now", "our",
                "us", "you", "find", "out", "shop", "up", "off", "-"
            };

            var keywords = announcement.Title.Split(' ')
                .Concat(announcement.Description.Split(' '))
                .Select(w => w.ToLower())
                .Distinct()
                .Where(k => !string.IsNullOrWhiteSpace(k) && !stopWords.Contains(k))
                .ToList();

            var similarAnnouncements = allAnnouncements
                .Where(a => keywords.Any(k =>
                    a.Title.ToLower().Contains(k)) &&
                    keywords.Any(k => a.Description.ToLower().Contains(k)))
                .Take(3)
                .ToList();

            return similarAnnouncements;
        }
    }
}