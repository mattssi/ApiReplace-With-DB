using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChistesAPI_DB.Models;

namespace ChistesAPI_DB.Controllers
{
    public class ChistesController : Controller
    {
        private readonly ApiReplaceContext _context;

        public ChistesController(ApiReplaceContext context)
        {
            _context = context;
        }

        // GET: Chistes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Chistes.ToListAsync());
        }

        // GET: Chistes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiste = await _context.Chistes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chiste == null)
            {
                return NotFound();
            }

            return View(chiste);
        }

        // GET: Chistes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chistes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Contenido")] Chiste chiste)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiste);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chiste);
        }

        // GET: Chistes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiste = await _context.Chistes.FindAsync(id);
            if (chiste == null)
            {
                return NotFound();
            }
            return View(chiste);
        }

        // POST: Chistes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Contenido")] Chiste chiste)
        {
            if (id != chiste.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiste);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChisteExists(chiste.Id))
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
            return View(chiste);
        }

        // GET: Chistes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiste = await _context.Chistes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chiste == null)
            {
                return NotFound();
            }

            return View(chiste);
        }

        // POST: Chistes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chiste = await _context.Chistes.FindAsync(id);

            if (chiste == null)
            {
                return NotFound();
            }

            _context.Chistes.Remove(chiste);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ChisteExists(int id)
        {
            return _context.Chistes.Any(e => e.Id == id);
        }
    }
}
