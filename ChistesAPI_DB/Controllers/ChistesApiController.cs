using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChistesAPI_DB.Models;

namespace ChistesAPI_DB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChistesController : ControllerBase
    {
        private readonly ApiReplaceContext _context;

        public ChistesController(ApiReplaceContext context)
        {
            _context = context;
        }

        // GET: api/Chistes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chiste>>> GetChistes()
        {
            return await _context.Chistes.ToListAsync();
        }

        // GET: api/Chistes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Chiste>> GetChiste(int id)
        {
            var chiste = await _context.Chistes.FindAsync(id);

            if (chiste == null)
            {
                return NotFound();
            }

            return chiste;
        }

        // PUT: api/Chistes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChiste(int id, Chiste chiste)
        {
            if (id != chiste.Id)
            {
                return BadRequest();
            }

            _context.Entry(chiste).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChisteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Chistes
        [HttpPost]
        public async Task<ActionResult<Chiste>> PostChiste(Chiste chiste)
        {
            _context.Chistes.Add(chiste);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChiste), new { id = chiste.Id }, chiste);
        }

        // DELETE: api/Chistes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChiste(int id)
        {
            var chiste = await _context.Chistes.FindAsync(id);
            if (chiste == null)
            {
                return NotFound();
            }

            _context.Chistes.Remove(chiste);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChisteExists(int id)
        {
            return _context.Chistes.Any(e => e.Id == id);
        }
    }
}
